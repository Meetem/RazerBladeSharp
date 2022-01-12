using System;
using System.Linq;
using System.Threading;
using librazerblade;

namespace RazerBladeSharp
{
    public class Program
    {
        public static float RetryInterval = 1f;
        private static Laptop _laptop;
        private static UsbDevice _usbDevice;

        public static void Main(string[] args)
        {
            UsbInterface.Initialize();
            AppDomain.CurrentDomain.ProcessExit += OnExit;

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
                $"Got Status:\n\tFanSpeed: {status.fanSpeed * 100}\n\tPowerMode: {status.powerMode}\n\tKeyboardBrightness: {status.keyboardInfo.brightness}");

            /*
             * This effectively setting 5000RPM manually,
             * AND selecting Gaming power mode (0 - Balanced, 1 - Gaming, 2 - Creator (only supported on some laptops))
             */
            Console.WriteLine("Setting 5000RPM and Gaming power mode");
            _laptop.SetFanSpeed(5000);
            _laptop.SetPowerMode(1, false);
            
            Console.WriteLine("Setting Max Keyboard Brightness");
            _laptop.SetBrightness(255);
            
            Console.WriteLine("Set diagonal key colors");
            for (int i = 0; i < 6; i++)
            {
                var row = new KeyboardRow((byte)i);
                row.keys[i] = new Rgb24(255, 255, 255);
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