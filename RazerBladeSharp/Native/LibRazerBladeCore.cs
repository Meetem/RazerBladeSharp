using System;
using System.Runtime.InteropServices;

namespace librazerblade
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LaptopPtr
    {
        public LaptopPtr(IntPtr p)
        {
            ptr = p;
        }

        public IntPtr ptr;
        public bool Null => ptr == IntPtr.Zero;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct UsbHandle
    {
        public int autoRelease;
        public IntPtr handle;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct UsbDeviceListPtr : IDisposable
    {
        public UsbDeviceListPtr(IntPtr p)
        {
            ptr = p;

            if (ptr != IntPtr.Zero)
                list = Marshal.PtrToStructure<UsbDeviceList>(ptr);
            else
                list = new UsbDeviceList();
        }

        public IntPtr ptr;
        public UsbDeviceList list;

        public void Dispose()
        {
            UsbInterface.FreeDeviceList(this);
        }

        public UsbDevice[] GetDevices()
        {
            return list.GetDevices();
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 3)]
    public readonly struct Rgb24
    {
        public bool Equals(Rgb24 other)
        {
            return r == other.r && g == other.g && b == other.b;
        }

        public override bool Equals(object obj)
        {
            return obj is Rgb24 other && Equals(other);
        }

        public override int GetHashCode()
        {
            return r | (g << 8) | (b << 16);
        }

        public readonly byte r, g, b;

        public Rgb24(byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardRow
    {
        public bool Equals(KeyboardRow other)
        {
            return rowid == other.rowid && IsKeysEquals(other);
        }

        public override bool Equals(object obj)
        {
            return obj is KeyboardRow other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (rowid.GetHashCode() * 397) ^ (keys != null ? keys.GetHashCode() : 0);
            }
        }

        public byte rowid; // Row ID (0-5) used by the EC to update each row

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public Rgb24[] keys; // 15 keys per row (regardless if the LEDs are present or not)

        public KeyboardRow(byte rowId)
        {
            this.rowid = rowId;
            keys = new Rgb24[15];
        }

        public bool IsKeysEquals(KeyboardRow keyboardRow)
        {
            if (keys == keyboardRow.keys)
                return true;

            if (keys.Length != keyboardRow.keys.Length)
                return false;

            for (int i = 0; i < keys.Length; i++)
            {
                if (!keys[i].Equals(keyboardRow.keys[i]))
                    return false;
            }

            return true;
        }
    };

    // Keyboard structs
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardInfo
    {
        public bool Equals(KeyboardInfo other)
        {
            return brightness == other.brightness && Equals(rows, other.rows);
        }

        public override bool Equals(object obj)
        {
            return obj is KeyboardInfo other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (brightness.GetHashCode() * 397) ^ (rows != null ? rows.GetHashCode() : 0);
            }
        }

        public byte brightness; // 0-255 brightness

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public KeyboardRow[] rows; // 6 rows
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
    public struct TransactionId
    {
        public byte id;

        public byte Id
        {
            get => (byte)(id & 0b00011111);
            set
            {
                id &= 0b11100000;
                id = (byte)(id | (value & 0b00011111));
            }
        }

        public byte Device
        {
            get => (byte)((id & 0b11100000) >> 5);
            set
            {
                var v = value & 0b00000111;
                id &= 0b11111000;
                id = (byte)(id | (v << 5));
            }
        }

        public TransactionId(byte device, byte id)
        {
            device &= 0b00000111;
            device <<= 5;

            id &= 0b00011111;
            this.id = (byte)(device | id);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
    public struct CommandId
    {
        public byte id;

        public byte Id
        {
            get => (byte)(id & 0b01111111);
            set
            {
                id &= 0b10000000;
                id = (byte)(id | (value & 0b01111111));
            }
        }

        public byte Direction
        {
            get => (byte)((id & 0b10000000) >> 7);
            set
            {
                var dir = (value & 0x01) << 7;
                id &= 0b01111111;
                id = (byte)(id | (dir & 0b10000000));
            }
        }

        public CommandId(byte direction, byte id)
        {
            direction &= 0b00000001;
            direction <<= 7;

            id &= 0b01111111;
            this.id = (byte)(direction | id);
        }
    }

    /* Status:
    * 0x00 New Command
    * 0x01 Command Busy
    * 0x02 Command Successful
    * 0x03 Command Failure
    * 0x04 Command No Response / Command Timeout
    * 0x05 Command Not Support
    *
    * Transaction ID used to group request-response, device useful when multiple devices are on one usb
    * Remaining Packets is the number of remaining packets in the sequence
    * Protocol Type is always 0x00
    * Data Size is the size of payload, cannot be greater than 80. 90 = header (8B) + data + CRC (1B) + Reserved (1B)
    * Command Class is the type of command being issued
    * Command ID is the type of command being send. Direction 0 is Host->Device, Direction 1 is Device->Host. AKA Get LED 0x80, Set LED 0x00
    *
    * */
    [StructLayout(LayoutKind.Sequential)]
    public struct RazerPacket
    {
        [StructLayout(LayoutKind.Explicit, Size = 90)]
        public struct Proxy
        {
            public RazerPacket Struct => this.ToStruct<RazerPacket, Proxy>();
        }

        public byte status;
        public TransactionId transaction_id;
        public ushort remaining_packets;
        public byte protocol_type; // 0x00
        public byte data_size;
        public byte command_class;
        public CommandId command_id;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
        public byte[] args;

        public byte crc;
        public byte reserved; // 0x00
    };
}