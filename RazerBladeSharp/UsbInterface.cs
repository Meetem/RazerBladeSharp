using System;

namespace librazerblade
{
    public static class UsbInterface
    {
        public static int Initialize()
        {
            var t = LibRazerBladeNative.librazerblade_initialize();
            if (t != 0)
            {
                throw new Exception($"Initialization failed with code {t}");
            }

            return t;
        }

        public static void Deinitialize()
        {
            LibRazerBladeNative.librazerblade_deinitialize();
        }

        public static void SetImplmenetation(UsbInterfaceImplementation implementation)
        {
            LibRazerBladeNative.librazerblade_UsbInterface_setImplementation(implementation);
        }

        public static UsbInterfaceImplementation GetImplemnetation()
        {
            return LibRazerBladeNative.librazerblade_UsbInterface_getImplementation();
        }

        public static int ControlTransfer(IntPtr handle, uint requestType, uint request, uint value,
            uint reportIndex,
            IntPtr buffer, ushort bufferSize, uint timeout, IntPtr aux)
        {
            return LibRazerBladeNative.librazerblade_UsbInterface_controlTransfer(handle, requestType, request, value,
                reportIndex, buffer, bufferSize, timeout, aux);
        }

        public static RazerPacket SendPayload(IntPtr handle, ref RazerPacket request, uint delayAfter,
            ref UsbPacketResult result, IntPtr aux)
        {
            return LibRazerBladeNative.librazerblade_UsbInterface_sendPayload(handle, ref request, delayAfter,
                ref result, aux);
        }

        public static RazerPacket SendPayload(IntPtr handle, IntPtr RazerPacket_request, uint delayAfter,
            IntPtr UsbPacketResult_result, IntPtr aux)
        {
            return LibRazerBladeNative.librazerblade_UsbInterface_sendPayload(handle, RazerPacket_request, delayAfter,
                UsbPacketResult_result, aux);
        }

        public static int GetUsbResponse(IntPtr handle, ref RazerPacket request,
            ref RazerPacket responseBuffer,
            ulong maxWait, IntPtr aux)
        {
            return LibRazerBladeNative.librazerblade_UsbInterface_getUsbResponse(handle, ref request,
                ref responseBuffer, maxWait, aux);
        }

        public static int GetUsbResponse(IntPtr handle, IntPtr RazerPacket_request,
            IntPtr RazerPacket_responseBuffer,
            ulong maxWait, IntPtr aux)
        {
            return LibRazerBladeNative.librazerblade_UsbInterface_getUsbResponse(handle, RazerPacket_request,
                RazerPacket_responseBuffer, maxWait, aux);
        }

        public static void CloseHandle(IntPtr handle, ref UsbDevice device)
        {
            LibRazerBladeNative.librazerblade_UsbInterface_closeHandle(handle, ref device);
        }

        public static void CloseHandle(IntPtr handle, IntPtr UsbDevice_device)
        {
            LibRazerBladeNative.librazerblade_UsbInterface_closeHandle(handle, UsbDevice_device);
        }

        public static IntPtr OpenDevice(ref UsbDevice device)
        {
            return LibRazerBladeNative.librazerblade_UsbInterface_openDevice(ref device);
        }

        public static IntPtr OpenDevice(IntPtr UsbDevice_device)
        {
            return LibRazerBladeNative.librazerblade_UsbInterface_openDevice(UsbDevice_device);
        }

        //UsbDeviceList*
        public static UsbDeviceListPtr ListDevices()
        {
            var ptr = LibRazerBladeNative.librazerblade_UsbInterface_listDevices();
            return new UsbDeviceListPtr(ptr);
        }

        public static void FreeDeviceList(UsbDeviceListPtr ptr)
        {
            LibRazerBladeNative.librazerblade_UsbInterface_freeDeviceList(ptr.ptr);
        }
    }
}