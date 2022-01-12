namespace librazerblade
{
    public static class PacketFactory
    {
        public static byte GetDataSize(BladePacketType type)
        {
            return LibRazerBladeNative.librazerblade_PacketFactory_getDataSize(type);
        }

        public static RazerPacket Empty()
        {
            return LibRazerBladeNative.librazerblade_PacketFactory_empty().Struct;
        }

        public static RazerPacket CreateRazerPacket(byte commandClass, BladePacketType type,
            BladePacketDirection direction = BladePacketDirection.Set,
            byte size = 0)
        {
            return LibRazerBladeNative.librazerblade_PacketFactory_createRazerPacket(commandClass, type, direction,
                size).Struct;
        }

        public static RazerPacket Fan(byte fanSpeedDiv100,
            BladePacketDirection direction = BladePacketDirection.Set)
        {
            return LibRazerBladeNative.librazerblade_PacketFactory_fan(fanSpeedDiv100, direction).Struct;
        }

        public static RazerPacket Power(byte powerMode, bool autoFanSpeed,
            BladePacketDirection direction = BladePacketDirection.Set)
        {
            return LibRazerBladeNative.librazerblade_PacketFactory_power(powerMode, autoFanSpeed ? 1 : 0, direction).Struct;
        }

        public static RazerPacket Row(KeyboardRow row,
            BladePacketDirection direction = BladePacketDirection.Set)
        {
            return LibRazerBladeNative.librazerblade_PacketFactory_row(row, direction).Struct;
        }

        public static RazerPacket ApplyChroma()
        {
            return LibRazerBladeNative.librazerblade_PacketFactory_applyChroma().Struct;
        }

        public static RazerPacket Brightness(byte brightness,
            BladePacketDirection direction = BladePacketDirection.Set)
        {
            return LibRazerBladeNative.librazerblade_PacketFactory_brightness(brightness, direction).Struct;
        }
    }
}