using System;
using System.Runtime.InteropServices;
using librazerblade;

namespace librazerblade
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LaptopFan
    {
        public const int DEFAULT_MIN = 3100;
        public const int DEFAULT_MAX = 5300;

        public int minFanSpeed;
        public int maxFanSpeed;

        public LaptopFan(int min, int max)
        {
            minFanSpeed = min;
            maxFanSpeed = max;
        }
        
        public override string ToString()
        {
            return $"Fan({minFanSpeed}, {maxFanSpeed})";
        }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Size = 280)]
    public struct LaptopDescription
    {
        [StructLayout(LayoutKind.Explicit, Size = 280)]
        public struct Proxy
        {
            public LaptopDescription Struct => this.ToStruct<LaptopDescription, Proxy>();
        }

        public UsbId id;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string name;

        public LaptopFan fan;

        [MarshalAs(UnmanagedType.I4)]
        public BladeCapabilities capabilities;

        public UserData userData;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct LaptopState
    {
        public bool Equals(LaptopState other)
        {
            return fanSpeed == other.fanSpeed && powerMode == other.powerMode && manualFanSpeed == other.manualFanSpeed && keyboardInfo.Equals(other.keyboardInfo);
        }

        public override bool Equals(object obj)
        {
            return obj is LaptopState other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = fanSpeed.GetHashCode();
                hashCode = (hashCode * 397) ^ powerMode.GetHashCode();
                hashCode = (hashCode * 397) ^ manualFanSpeed.GetHashCode();
                hashCode = (hashCode * 397) ^ keyboardInfo.GetHashCode();
                return hashCode;
            }
        }

        [StructLayout(LayoutKind.Explicit, Size = 280)]
        public struct Proxy
        {
            public LaptopState Struct => this.ToStruct<LaptopState, Proxy>();
        }

        public byte fanSpeed;
        public byte powerMode;
        public byte manualFanSpeed;
        public KeyboardInfo keyboardInfo;

        public bool IsManualFanSpeed
        {
            get => manualFanSpeed > 0;
            set => manualFanSpeed = (byte)(value ? 1 : 0);
        }
    };
}