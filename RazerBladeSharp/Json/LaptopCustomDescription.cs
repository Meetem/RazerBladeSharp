using System;
using System.Runtime.InteropServices;
using System.Text;

namespace librazerblade.Json
{
    public class LaptopCustomDescription
    {
        public string Name { get; set; }
        public int VendorId { get; set; }
        public int ProductId { get; set; }
        public LaptopFan Fan { get; set; } = new LaptopFan(3500, 5000);
        public BladeCapabilities Capabilities { get; set; } = BladeCapabilities.None;
        public string UserData { get; set; } = null;

        public LaptopDescription GetStruct(Encoding userDataEncoding = null)
        {
            if(userDataEncoding == null)
                userDataEncoding = Encoding.UTF8;
            
            if (string.IsNullOrEmpty(Name))
                throw new Exception("Name must not be null or empty");

            var nBytes = Encoding.ASCII.GetBytes(Name.Trim() + "\0");
            var n = Encoding.ASCII.GetString(nBytes);

            if (nBytes.Length >= 256)
                throw new Exception("Name should not take more than 256 bytes in ASCII");

            if (Fan.minFanSpeed < 0)
                throw new Exception("Fan.minFanSpeed should be >= 0");
            
            if (Fan.maxFanSpeed < 0)
                throw new Exception("Fan.maxFanSpeed should be >= 0");

            if (Fan.maxFanSpeed <= Fan.minFanSpeed)
                throw new Exception("Fan.maxFanSpeed should be > Fan.minFanSpeed");

            var data = new UserData();
            if (!string.IsNullOrEmpty(UserData))
            {
                var encoded = userDataEncoding.GetBytes(UserData);
                data = librazerblade.UserData.FromMemory(encoded, true);
            }

            return new LaptopDescription()
            {
                capabilities = Capabilities,
                fan = Fan,
                id = new UsbId((ushort)VendorId, (ushort)ProductId),
                name = n,
                userData = data
            };
        }
    }
}