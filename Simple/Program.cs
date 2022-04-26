using System;
using System.Linq;
using System.Threading;
using librazerblade;

namespace RazerBladeSharp
{
    public class Program
    {
        private const string keyBright = "keybright=";
        private const string keyColor = "keycolor=";
        private const double percentToByte = 255.0 / 100.0;
        public static float RetryInterval = 1f;
        private static Laptop _laptop;
        private static UsbDevice _usbDevice;
            
        private static int ParsePowerMode(string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;

            str = str.Trim();
            if (str.Equals("balanced", StringComparison.OrdinalIgnoreCase))
                return 0;

            if (str.Equals("custom", StringComparison.OrdinalIgnoreCase))
                return 1;

            if (int.TryParse(str, out var pm))
            {
                if (pm < 0)
                    pm = 0;

                return pm;
            }
            
            return 0;
        }
        
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: Simple.exe FanSpeed [powerMode:balanced/custom] [FanSpeed 2]");
                return;
            }
            
            UsbInterface.Initialize();
            AppDomain.CurrentDomain.ProcessExit += OnExit;

            var fanSpeed = FanSpeed.Parse(args[0]);
            int powerMode = ParsePowerMode(args.Length >= 2 ? args[1] : null);
            var fanSpeed2 = FanSpeed.Parse(args.Length >= 3 ? args[2] : null);

            byte kbBrightness = 32;
            bool disableGetDescription = false;
            int[] cp = new int[3] { 30, 255, 30 };
            
            for (int i = 0; i < args.Length; i++)
            {
                var a = args[i]?.Trim();
                if(string.IsNullOrEmpty(a))
                    continue;

                if (a.Equals("disable_get_description"))
                    disableGetDescription = true;

                if (a.StartsWith(keyBright))
                {
                    a = a.Substring(keyBright.Length);
                    if (int.TryParse(a, out var kbdb))
                    {
                        kbBrightness = (byte)Math.Max(Math.Min((int)(kbBrightness * percentToByte), 255), 0);
                        Console.WriteLine($"Parsed keyboard brightness {kbBrightness / percentToByte:0.00}%");
                    }

                    continue;
                }

                if (a.StartsWith(keyColor))
                {
                    a = a.Substring(keyColor.Length);
                    var colors = a.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    if (colors.Length != 3)
                    {
                        Console.WriteLine("Key colors invalid, example is 30,255,30");
                        continue;
                    }

                    for (int j = 0; j < 3; j++)
                    {
                        colors[j] = colors[j].Trim();
                        if (int.TryParse(colors[j], out cp[j]))
                            cp[j] = Math.Clamp(cp[j], 0, 255);
                    }
                    
                    Console.WriteLine($"Parsed key color rgb({cp[0]}, {cp[1]}, {cp[2]})");
                }
            }
            
            Console.WriteLine($"Parsed: Speed: {fanSpeed}" +
                              $", PowerMode: {powerMode}" +
                              $", Second Fan Speed {fanSpeed2}" +
                              $", KeyBrightness: {kbBrightness / percentToByte:0.00}%" +
                              $", Key Colors: ({cp[0]}, {cp[1]}, {cp[2]})");
            
            while (true)
            {
                _laptop = DetectLaptop();
                if (_laptop != null)
                    break;

                Console.WriteLine($"Razer Blade Laptop not detected, retrying in {RetryInterval:0.00}s");
                Thread.Sleep((int)TimeSpan.FromSeconds(RetryInterval).TotalMilliseconds);
            }

            if (_laptop == null)
                return;

            if (!disableGetDescription)
            {
                var desc = _laptop.GetDescription();
                Console.WriteLine($"Found laptop {desc.name}, fan = {desc.fan}, capabilities = {desc.capabilities}");
                Console.WriteLine("Requesting status");

                //Simple query of current state, will be written in internal Laptop* object
                var result = _laptop.Query(BladeQuery.QueryAll, 999).GetResults(BladeQuery.QueryAll);
                if (!result.IsAllSucceded())
                {
                    Console.WriteLine($"Unable to request status, results:");
                    Console.WriteLine(string.Join("\n", result.GetNotSucceded().Select(x => $"{x.Key} = {x.Value}")));
                    return;
                }

                //Get LaptopState object from Laptop* where all the data is stored
                var status = _laptop.GetState();
                Console.WriteLine(
                    $"Got Status:\n\t" +
                    $"FanSpeed: {status.fanSpeed * 100}" +
                    $"\n\tPowerMode: {status.powerMode}" +
                    $"\n\tManualFanSpeed: {status.IsManualFanSpeed}" +
                    $"\n\tKeyboardBrightness: {status.keyboardInfo.brightness}");
            }
            
            /*
             * This effectively setting 5000RPM manually,
             * AND selecting Gaming power mode (0 - Balanced, 1 - Gaming, 2 - Creator (only supported on some laptops))
             */
            _laptop.SetFanSpeed(fanSpeed.GetSpeed(), 0, 1, fanSpeed.clamp);
            
            if (fanSpeed2.hasValue)
                _laptop.SetFanSpeed(fanSpeed2.GetSpeed(), 0, 2, fanSpeed2.clamp);
            
            _laptop.SetPowerMode((byte)powerMode, fanSpeed.GetSpeed() == 0);
            _laptop.SetBrightness(kbBrightness);
            
            for (int i = 0; i < 6; i++)
            {
                var row = new KeyboardRow((byte)i);
                for (int j = 0; j < 15; j++)
                    row.keys[j] = new Rgb24((byte)cp[0], (byte)cp[1], (byte)cp[2]);
                
                _laptop.SendKeyboardRow(row);
            }

            //Apply sent colors
            _laptop.ApplyChroma();
            
            Console.WriteLine("Set. Exiting in 5 seconds");
            Thread.Sleep(5000);
        }

        private static Laptop DetectLaptop()
        {
            using (var devices = UsbInterface.ListDevices())
            {
                (LaptopDescription, UsbDevice)? foundDescription = null;
                foreach (var dev in devices.GetDevices())
                {
                    var d = DescriptionStorage.Get(dev.usbId);
                    if (d == null)
                        continue;

                    _usbDevice = dev;
                    var handle = UsbInterface.OpenDevice(ref _usbDevice);
                    if (handle == IntPtr.Zero)
                    {
                        Console.WriteLine($"Can't open usb device {dev.usbId} ({dev.device})");
                        return null;
                    }

                    var h = new UsbHandle()
                    {
                        autoRelease = 1,
                        handle = handle
                    };

                    return new Laptop(d.Value, h, _usbDevice);
                }
            }

            return null;
        }

        private static void OnExit(object sender, EventArgs e)
        {
            if (_laptop != null)
            {
                _laptop.Dispose();
                _laptop = null;
            }

            //Console.WriteLine("Exiting");
            UsbInterface.Deinitialize();
        }
    }
}