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

        public IntPtr userData;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct LaptopState
    {
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