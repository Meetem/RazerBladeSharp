using System;
using System.Collections.Generic;

namespace librazerblade
{
    public class Laptop : IDisposable
    {
        public Laptop(LaptopDescription description, UsbHandle usbHandle, UsbDevice device)
        {
            ptr = LibRazerBladeNative.librazerblade_Laptop_new(description, usbHandle, device);
            if (ptr.Null)
                throw new NullReferenceException("Can't create laptop");
        }

        public Laptop(LaptopPtr ptr)
        {
            if (ptr.Null)
                throw new NullReferenceException("Can't create laptop from null");

            this.ptr = ptr;
        }

        public UsbDevice GetUsbDevice()
        {
            return LibRazerBladeNative.librazerblade_Laptop_getDevice(ptr);
        }

        public UsbHandle GetUsbHandle()
        {
            return LibRazerBladeNative.librazerblade_Laptop_getUsbHandle(ptr);
        }

        public void SetUsbHandle(UsbHandle usbHandle)
        {
            LibRazerBladeNative.librazerblade_Laptop_setUsbHandle(ptr, usbHandle);
        }

        public byte ClampFan(int fanSpeed)
        {
            return LibRazerBladeNative.librazerblade_Laptop_clampFan(ptr, fanSpeed);
        }

        public BladeCapabilities GetCapabilities()
        {
            return LibRazerBladeNative.librazerblade_Laptop_getCapabilities(ptr);
        }

        public LaptopDescription GetDescription()
        {
            return LibRazerBladeNative.librazerblade_Laptop_getDescription(ptr).Struct;
        }

        public LaptopState GetState()
        {
            return LibRazerBladeNative.librazerblade_Laptop_getState(ptr).Struct;
        }

        public IntPtr GetStateUnsafe()
        {
            return LibRazerBladeNative.librazerblade_Laptop_getStatePtrUnsafe(ptr);
        }

        public LaptopQueryResult Query(BladeQuery query, int numRetries = 0)
        {
            return LibRazerBladeNative.librazerblade_Laptop_query(ptr, query, numRetries);
        }

        public Dictionary<BladeQuery, UsbPacketResult> QueryWithResults(BladeQuery query, int numRetries = 0)
        {
            var r = LibRazerBladeNative.librazerblade_Laptop_query(ptr, query, numRetries);
            return r.GetResults(query);
        }

        /// <summary>
        /// This method can fail on some laptops
        /// Confirmed failing on Razer Blade 15" 2019 Advanced
        /// </summary>
        /// <param name="query"></param>
        /// <param name="numRetries"></param>
        /// <returns></returns>
        public LaptopQueryResult QueryRows(BladeQueryRows query, int numRetries = 0)
        {
            return LibRazerBladeNative.librazerblade_Laptop_queryRows(ptr, query, numRetries);
        }

        public UsbPacketResult SetFanSpeed(int speed, int numRetries = 0, int fanId = 1, bool clampSpeed = true)
        {
            return LibRazerBladeNative.librazerblade_Laptop_setFanSpeed(ptr, speed, numRetries, fanId,
                clampSpeed ? 1 : 0);
        }

        public UsbPacketResult SetBoost(BladeBoostId boostId, byte value, int numRetries = 0)
        {
            return LibRazerBladeNative.librazerblade_Laptop_setBoost(ptr, boostId, value, numRetries);
        }

        public UsbPacketResult SetPowerMode(byte powerMode, bool autoFanSpeed, int numRetries = 0)
        {
            return LibRazerBladeNative.librazerblade_Laptop_setPowerMode(ptr, powerMode, autoFanSpeed ? 1 : 0,
                numRetries);
        }

        public UsbPacketResult SetBrightness(byte brightness, int numRetries = 0)
        {
            return LibRazerBladeNative.librazerblade_Laptop_setBrightness(ptr, brightness, numRetries);
        }

        public UsbPacketResult ApplyChroma(int numRetries = 0)
        {
            return LibRazerBladeNative.librazerblade_Laptop_applyChroma(ptr, numRetries);
        }

        public UsbPacketResult SendKeyboardRow(KeyboardRow row, int numRetries = 0)
        {
            return LibRazerBladeNative.librazerblade_Laptop_sendKeyboardRow(ptr, row, numRetries);
        }

        public UsbPacketResult SendPacketWithRetry(ref RazerPacket packet, ref RazerPacket output,
            int numRetries = 0,
            int retryIntervalMs = 250)
        {
            return LibRazerBladeNative.librazerblade_Laptop_sendPacketWithRetry(ptr, ref packet, ref output, numRetries,
                retryIntervalMs);
        }

        public UsbPacketResult SendPacketWithRetry(IntPtr RazerPacket_packet, IntPtr RazerPacket_output,
            int numRetries = 0,
            int retryIntervalMs = 250)
        {
            return LibRazerBladeNative.librazerblade_Laptop_sendPacketWithRetry(ptr, RazerPacket_packet,
                RazerPacket_output, numRetries,
                retryIntervalMs);
        }

        public static int GetArrayIndex(int flag)
        {
            return LibRazerBladeNative.librazerblade_Laptop_getArrayIndex(flag);
        }

        protected LaptopPtr ptr;

        public void Dispose()
        {
            if (ptr.ptr != IntPtr.Zero)
            {
                LibRazerBladeNative.librazerblade_Laptop_delete(ptr);
                ptr.ptr = IntPtr.Zero;
            }
        }

        ~Laptop()
        {
            Dispose();
        }
    }
}