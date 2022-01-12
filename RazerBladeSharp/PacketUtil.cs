namespace librazerblade
{
    public static class PacketUtil
    {
        public static byte GetFanValueRaw(ref RazerPacket pkt)
        {
            return LibRazerBladeNative.librazerblade_PacketUtil_getFanValueRaw(ref pkt);
        }

        public static int GetFanValue(ref RazerPacket pkt)
        {
            return LibRazerBladeNative.librazerblade_PacketUtil_getFanValue(ref pkt);
        }

        public static byte GetBrightness(ref RazerPacket pkt)
        {
            return LibRazerBladeNative.librazerblade_PacketUtil_getBrightness(ref pkt);
        }

        public static byte GetManualFanSpeed(ref RazerPacket pkt)
        {
            return LibRazerBladeNative.librazerblade_PacketUtil_getManualFanSpeed(ref pkt);
        }

        public static bool GetIsManualFanSpeed(ref RazerPacket pkt)
        {
            return GetManualFanSpeed(ref pkt) > 0;
        }

        public static KeyboardRow GetRow(ref RazerPacket pkt)
        {
            return LibRazerBladeNative.librazerblade_PacketUtil_getRow(ref pkt);
        }
    }
}