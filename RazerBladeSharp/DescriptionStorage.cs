using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using librazerblade.Json;

namespace librazerblade
{
    public static class DescriptionStorage
    {
        public static LaptopDescription? Get(ushort vendor, ushort product)
        {
            int idx = -1;
            
            var d = LibRazerBladeNative.librazerblade_DescriptionStorage_get(vendor, product, ref idx);
            if (idx < 0)
                return null;

            return d.Struct;
        }
        
        public static LaptopDescription? Get(UsbId id)
        {
            return Get(id.vendorId, id.productId);
        }
        
        public static LaptopDescription[] GetAll()
        {
            int count = -1;
            var ptr = LibRazerBladeNative.librazerblade_DescriptionStorage_getAll(ref count);

            if (ptr == IntPtr.Zero || count <= 0)
                return Array.Empty<LaptopDescription>();

            return ptr.PtrToStructureArray<LaptopDescription>(count);
        }

        public static void Set(int idx, LaptopDescription description)
        {
            LibRazerBladeNative.librazerblade_DescriptionStorage_set(idx, description);
        }

        public static void Put(LaptopDescription description)
        {
            LibRazerBladeNative.librazerblade_DescriptionStorage_put(description);
        }

        public static IntPtr GetAllUnsafe(ref int count)
        {
            return LibRazerBladeNative.librazerblade_DescriptionStorage_getAll(ref count);
        }

        public static AggregateException Put(IEnumerable<LaptopCustomDescription> descriptions, Encoding userDataEncoding = null)
        {
            List<Exception> handled = new List<Exception>();
            foreach (var description in descriptions)
            {
                try
                {
                    Put(description.GetStruct(userDataEncoding));
                }
                catch (Exception e)
                {
                    handled.Add(e);
                }
            }

            if (handled.Count == 0)
                return null;

            return new AggregateException(handled);
        }
        
        public static void Clear()
        {
            LibRazerBladeNative.librazerblade_DescriptionStorage_clear();
        }
    }
}