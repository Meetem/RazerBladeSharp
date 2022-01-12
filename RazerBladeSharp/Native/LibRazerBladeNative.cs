using System;
using System.Runtime.InteropServices;

namespace librazerblade
{
    public class LibRazerBladeNative
    {
        
        public const string LibraryPath = "librazerblade";
        public const CallingConvention CallType = CallingConvention.Cdecl;
        public const int RAZER_USB_REPORT_LEN = 90;
        public const int DEFAULT_TIMEOUT = 5000;

        #region Usb Interface

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern int librazerblade_initialize();

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern void librazerblade_deinitialize();

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern void
            librazerblade_UsbInterface_setImplementation(UsbInterfaceImplementation anImplementation);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern UsbInterfaceImplementation librazerblade_UsbInterface_getImplementation();

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern int
            librazerblade_UsbInterface_controlTransfer(IntPtr handle, uint requestType, uint request, uint value,
                uint reportIndex,
                IntPtr buffer, ushort bufferSize, uint timeout, IntPtr aux);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern RazerPacket
            librazerblade_UsbInterface_sendPayload(IntPtr handle, ref RazerPacket request, uint delayAfter,
                ref UsbPacketResult result, IntPtr aux);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern RazerPacket
            librazerblade_UsbInterface_sendPayload(IntPtr handle, IntPtr RazerPacket_request, uint delayAfter,
                IntPtr UsbPacketResult_result, IntPtr aux);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern int
            librazerblade_UsbInterface_getUsbResponse(IntPtr handle, ref RazerPacket request,
                ref RazerPacket responseBuffer,
                ulong maxWait, IntPtr aux);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern int
            librazerblade_UsbInterface_getUsbResponse(IntPtr handle, IntPtr RazerPacket_request,
                IntPtr RazerPacket_responseBuffer,
                ulong maxWait, IntPtr aux);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern void librazerblade_UsbInterface_closeHandle(IntPtr handle, ref UsbDevice device);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern void librazerblade_UsbInterface_closeHandle(IntPtr handle, IntPtr UsbDevice_device);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern IntPtr librazerblade_UsbInterface_openDevice(ref UsbDevice device);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern IntPtr librazerblade_UsbInterface_openDevice(IntPtr UsbDevice_device);

        //UsbDeviceList*
        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern IntPtr librazerblade_UsbInterface_listDevices();

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern void librazerblade_UsbInterface_freeDeviceList(IntPtr UsbDeviceList_list);

        #endregion

        #region Packet Factory

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern byte librazerblade_PacketFactory_getDataSize(BladePacketType type);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern RazerPacket.Proxy librazerblade_PacketFactory_empty();

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern RazerPacket.Proxy
            librazerblade_PacketFactory_createRazerPacket(byte commandClass, BladePacketType type,
                BladePacketDirection direction = BladePacketDirection.Set,
                byte size = 0);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern RazerPacket.Proxy
            librazerblade_PacketFactory_fan(byte fanSpeedDiv100,
                BladePacketDirection direction = BladePacketDirection.Set);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern RazerPacket.Proxy
            librazerblade_PacketFactory_power(byte powerMode, int autoFanSpeed,
                BladePacketDirection direction = BladePacketDirection.Set);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern RazerPacket.Proxy librazerblade_PacketFactory_row(KeyboardRow row,
            BladePacketDirection direction = BladePacketDirection.Set);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern RazerPacket.Proxy librazerblade_PacketFactory_applyChroma();

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern RazerPacket.Proxy
            librazerblade_PacketFactory_brightness(byte brightness,
                BladePacketDirection direction = BladePacketDirection.Set);

        #endregion

        #region Packet Util

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern byte librazerblade_PacketUtil_getFanValueRaw(ref RazerPacket pkt);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern int librazerblade_PacketUtil_getFanValue(ref RazerPacket pkt);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern byte librazerblade_PacketUtil_getBrightness(ref RazerPacket pkt);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern KeyboardRow librazerblade_PacketUtil_getRow(ref RazerPacket pkt);

        #endregion

        #region Laptop

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern LaptopPtr librazerblade_Laptop_new(LaptopDescription description, UsbHandle usbHandle,
            UsbDevice device);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern void librazerblade_Laptop_delete(LaptopPtr self);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern UsbDevice librazerblade_Laptop_getDevice(LaptopPtr self);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern UsbHandle librazerblade_Laptop_getUsbHandle(LaptopPtr self);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern void librazerblade_Laptop_setUsbHandle(LaptopPtr self, UsbHandle v);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern byte librazerblade_Laptop_clampFan(LaptopPtr self, int v);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern BladeCapabilities librazerblade_Laptop_getCapabilities(LaptopPtr self);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern LaptopDescription.Proxy librazerblade_Laptop_getDescription(LaptopPtr self);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern LaptopState.Proxy librazerblade_Laptop_getState(LaptopPtr self);

        //Unsafe.
        //LaptopState*
        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern IntPtr librazerblade_Laptop_getStatePtrUnsafe(LaptopPtr self);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern UsbPacketResult librazerblade_Laptop_queryFanSpeed(LaptopPtr self, int numRetries = 0);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern UsbPacketResult librazerblade_Laptop_queryPowerMode(LaptopPtr self, int numRetries = 0);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern UsbPacketResult librazerblade_Laptop_queryBrightness(LaptopPtr self, int numRetries = 0);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern UsbPacketResult
            librazerblade_Laptop_queryChromaRow(LaptopPtr self, int rowId, int numRetries = 0);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern LaptopQueryResult
            librazerblade_Laptop_query(LaptopPtr self, BladeQuery query, int numRetries = 0);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern LaptopQueryResult librazerblade_Laptop_queryRows(LaptopPtr self, BladeQueryRows rows,
            int numRetries = 0);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern UsbPacketResult librazerblade_Laptop_setFanSpeed(LaptopPtr self, int speed,
            int numRetries = 0);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern UsbPacketResult
            librazerblade_Laptop_setPowerMode(LaptopPtr self, byte powerMode, int autoFanSpeed, int numRetries = 0);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern UsbPacketResult librazerblade_Laptop_setBrightness(LaptopPtr self, byte brightness,
            int numRetries = 0);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern UsbPacketResult librazerblade_Laptop_applyChroma(LaptopPtr self, int numRetries = 0);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern UsbPacketResult librazerblade_Laptop_sendKeyboardRow(LaptopPtr self, KeyboardRow row,
            int numRetries = 0);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern UsbPacketResult
            librazerblade_Laptop_sendPacketWithRetry(LaptopPtr self, ref RazerPacket packet, ref RazerPacket output,
                int numRetries = 0,
                int retryIntervalMs = 250);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern UsbPacketResult
            librazerblade_Laptop_sendPacketWithRetry(LaptopPtr self, IntPtr RazerPacket_packet, IntPtr RazerPacket_output,
                int numRetries = 0,
                int retryIntervalMs = 250);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern int librazerblade_Laptop_getArrayIndex(int q);

        #endregion

        #region Storage

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern LaptopDescription.Proxy librazerblade_DescriptionStorage_get(ushort vendor, ushort product,
            ref int idx);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern void librazerblade_DescriptionStorage_put(LaptopDescription description);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern IntPtr librazerblade_DescriptionStorage_getAll(ref int size);

        [DllImport(LibraryPath, CallingConvention = CallType, CharSet = CharSet.Ansi)]
        public static extern void librazerblade_DescriptionStorage_set(int idx, LaptopDescription description);

        #endregion
    }
}