using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace librazerblade
{
    public delegate int ControlTransferFunction(IntPtr handle, uint requestType, uint request, uint value,
        uint reportIndex, IntPtr buffer, ushort bufferSize, uint timeout,
        IntPtr aux);

    public delegate int SendControlMessageFunction(IntPtr handle, IntPtr buffer, ushort size, uint timeout,
        uint delayAfter, IntPtr aux);

    public delegate void CloseHandleFunction(IntPtr handle, ref UsbDevice device);

    public delegate IntPtr OpenDeviceFunction(ref UsbDevice device);

    //UsbDeviceList*
    public delegate IntPtr ListDevicesFunction();

    public delegate void FreeDeviceListFunction(ref UsbDeviceList list);

    [StructLayout(LayoutKind.Sequential)]
    public struct UsbId
    {
        public ushort vendorId;
        public ushort productId;

        public int Id => (int)((uint)vendorId | ((uint)productId << 16));

        public override string ToString()
        {
            return $"{vendorId:X4}:{productId:X4}";
        }

        public static implicit operator int(UsbId id)
        {
            return id.Id;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct UsbDevice
    {
        public IntPtr device;
        public IntPtr aux;
        public UsbId usbId;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct UsbDeviceList
    {
        public int count;
        public int error;

        //UsbDevice*
        public IntPtr devices;
        public IntPtr userData;

        public UsbDevice[] GetDevices()
        {
            if (devices == IntPtr.Zero)
                return null;

            var array = new UsbDevice[count];
            var sz = Marshal.SizeOf<UsbDevice>();

            for (int i = 0; i < count; i++)
            {
                var p = IntPtr.Add(devices, sz * i);
                array[i] = Marshal.PtrToStructure<UsbDevice>(p);
            }

            return array;
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct UsbPacketResult
    {
        public int sendResult;

        [MarshalAs(UnmanagedType.I4)]
        public BladeResponse packetResult;

        public int isSuccess;
        public bool IsSuccess => isSuccess == 1;

        public override string ToString()
        {
            if (IsSuccess)
                return "(Success)";

            return $"(SendResult: {sendResult}, PacketResult: {packetResult})";
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct LaptopQueryResult
    {
        public UsbPacketResult result0;
        public UsbPacketResult result1;
        public UsbPacketResult result2;
        public UsbPacketResult result3;
        public UsbPacketResult result4;
        public UsbPacketResult result5;
        public UsbPacketResult result6;
        public UsbPacketResult result7;

        public UsbPacketResult[] GetResults()
        {
            var results = new UsbPacketResult[8];
            for (int i = 0; i < results.Length; i++)
                results[i] = GetById(i);

            return results;
        }

        public bool IsAllSucceded()
        {
            for (int i = 0; i < 8; i++)
            {
                var r = GetById(i);
                if (r.packetResult == BladeResponse.RazerResponseNone && r.sendResult == 0)
                    continue;

                if (!r.IsSuccess)
                    return false;
            }

            return true;
        }

        public Dictionary<BladeQuery, UsbPacketResult> GetResults(BladeQuery flags)
        {
            var f = (uint)flags;
            Dictionary<BladeQuery, UsbPacketResult> results = new Dictionary<BladeQuery, UsbPacketResult>();

            for (int i = 0; i < (int)BladeQuery.QueryCount; i++)
            {
                var t = (1 << i);
                if ((f & t) == t)
                    results[(BladeQuery)t] = GetResult((BladeQuery)t);
            }

            return results;
        }

        public Dictionary<BladeQueryRows, UsbPacketResult> GetResults(BladeQueryRows flags)
        {
            var f = (uint)flags;
            Dictionary<BladeQueryRows, UsbPacketResult> results = new Dictionary<BladeQueryRows, UsbPacketResult>();

            for (int i = 0; i < (int)BladeQueryRows.QueryCount; i++)
            {
                var t = (1 << i);
                if ((f & t) == t)
                    results[(BladeQueryRows)t] = GetResult((BladeQueryRows)t);
            }

            return results;
        }

        public UsbPacketResult GetResult(BladeQuery query)
        {
            return GetById(Laptop.GetArrayIndex((int)query));
        }

        public UsbPacketResult GetResult(BladeQueryRows query)
        {
            return GetById(Laptop.GetArrayIndex((int)query));
        }

        public UsbPacketResult GetById(int idx)
        {
            switch (idx)
            {
                case 0:
                    return result0;
                case 1:
                    return result1;
                case 2:
                    return result2;
                case 3:
                    return result3;
                case 4:
                    return result4;
                case 5:
                    return result5;
                case 6:
                    return result6;
                case 7:
                    return result7;
                default:
                    throw new IndexOutOfRangeException($"Index {idx} is invalid, allowed [0;7]");
            }
        }
    };

    public struct UsbInterfaceImplementation
    {
        public ControlTransferFunction controlTransfer;
        public SendControlMessageFunction sendControlMessage;
        public CloseHandleFunction closeHandle;
        public OpenDeviceFunction openDevice;
        public ListDevicesFunction listDevices;
        public FreeDeviceListFunction freeDeviceList;
    };
}