using System;

namespace librazerblade
{
    [Flags]
    public enum BladeCapabilities : int
    {
        None = 0x00,
        Boost = 0x01,
        Creator = 0x02,
        All = 0x7FFFFFFF
    };

    [Flags]
    public enum BladeQuery : int
    {
        QueryNone = 0,
        QueryFanSpeed = 0b00000001,
        QueryPowerMode = 0b00000010,
        QueryBrightness = 0b00000100,
        QueryBoost = 0b00001000,
        QueryAll = 0x7FFFFFFF,
        QueryCount = 3,
    };

    [Flags]
    public enum BladeQueryRows : int
    {
        QueryRow0 = 0b00000001,
        QueryRow1 = 0b00000010,
        QueryRow2 = 0b00000100,
        QueryRow3 = 0b00001000,
        QueryRow4 = 0b00010000,
        QueryRow5 = 0b00100000,
        QueryCount = 6,
        QueryRowAll = 0x7FFFFFFF
    };

    public enum BladeResponse : int
    {
        RazerResponseNone = 0x00,
        RazerResponseBusy = 0x01,
        RazerResponseSuccess = 0x02,
        RazerResponseFailure = 0x03,
        RazerResponseTimeout = 0x04,
        RazerResponseNotSupported = 0x05,

        RazerResponseDoesntMatch = 0xFF
    };

    public enum BladeBoostId : int
    {
        Cpu = 0x01,
        Gpu = 0x02
    };

    public enum BladePacketDirection : int
    {
        Set = 0x00,
        Get = 0x01,
        HostToDevice = Set,
        DeviceToHost = Get
    };

    public enum BladePacketType : int
    {
        PktFan = 0x01,
        PktPower = 0x02,
        PktBoost = 0x07,
        PktGetBrightness = 0x03,
        PktSetBrightness = 0x04,
        PktChromaApply = 0x0a,
        PktChromaSetRow = 0x0b,
    };
}