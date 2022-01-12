using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace librazerblade
{
    public static class Ext
    {
        public static bool IsAllSucceded<T>(this Dictionary<T, UsbPacketResult> results)
        {
            foreach (var kv in results)
            {
                if (!kv.Value.IsSuccess)
                    return false;
            }

            return true;
        }

        public static bool IsAllSucceded(this LaptopQueryResult result, BladeQuery query)
        {
            return result.GetResults(query).IsAllSucceded();
        }

        public static Dictionary<T, UsbPacketResult> GetNotSucceded<T>(this Dictionary<T, UsbPacketResult> results)
        {
            Dictionary<T, UsbPacketResult> nd = new Dictionary<T, UsbPacketResult>();
            foreach (var usbPacketResult in results)
            {
                if (!usbPacketResult.Value.IsSuccess)
                    nd[usbPacketResult.Key] = usbPacketResult.Value;
            }

            return nd;
        }

        public static TElement[] StructToArray<TElement, TStruct>(this TStruct s)
            where TStruct : struct
        {
            var sz = Marshal.SizeOf<TStruct>();
            var szElement = Marshal.SizeOf<TElement>();

            if (sz % szElement != 0)
                throw new InvalidOperationException(
                    $"Can't copy {typeof(TStruct).Name} to {typeof(TElement).Name}[], {sz} indivisible by {szElement}");

            var numElements = sz / szElement;
            var array = new TElement[numElements];
            Marshal.StructureToPtr(s, Marshal.UnsafeAddrOfPinnedArrayElement(array, 0), false);
            return array;
        }

        public static TStruct PtrToStructure<TStruct>(this IntPtr ptr)
            where TStruct : struct
        {
            return Marshal.PtrToStructure<TStruct>(ptr);
        }

        public static TStruct ArrayToStructure<TStruct, TElement>(this TElement[] array)
            where TStruct : struct
        {
            var arraySize = Marshal.SizeOf<TElement>() * array.Length;
            var structSize = Marshal.SizeOf<TStruct>();
            if (structSize != arraySize)
                throw new InvalidOperationException($"Array size {arraySize} bytes, but structure is {structSize}");

            var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(array, 0);
            return Marshal.PtrToStructure<TStruct>(ptr);
        }

        public static TStruct[] PtrToStructureArray<TStruct>(this IntPtr ptr, int count)
        {
            if (count <= 0)
                throw new ArgumentException("Count must be >= 0", nameof(count));

            var array = new TStruct[count];
            var sz = Marshal.SizeOf<TStruct>();

            for (int i = 0; i < count; i++)
                array[i] = Marshal.PtrToStructure<TStruct>(IntPtr.Add(ptr, sz * i));

            return array;
        }

        public static TStruct ToStruct<TStruct, TProxy>(this TProxy proxy)
            where TStruct : struct
            where TProxy : struct
        {
            var szp = Marshal.SizeOf<TProxy>();
            var szt = Marshal.SizeOf<TStruct>();

            if (szp != szt)
                throw new InvalidOperationException(
                    $"Proxy and struct size are different for {typeof(TStruct).Name} Proxy: {szp}, Struct: {szt}");

            var p = Marshal.AllocHGlobal(szp);
            Marshal.StructureToPtr(proxy, p, false);
            var r = Marshal.PtrToStructure<TStruct>(p);
            Marshal.FreeHGlobal(p);
            return r;
        }
    }
}