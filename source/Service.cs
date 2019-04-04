using System;
using System.Reflection;

namespace gmp
{
    public static class CRC16
    {
        const ushort polynomial = 0xA001;

        static readonly ushort[] table = new ushort[256];

        public static ushort Compute(byte[] bytes)
        {
            return Compute(bytes, 0, bytes.Length);
        }
        public static ushort Compute(byte[] bytes, int Offset, int Size)
        {
            ushort crc = 0xFFFF;

            for (int i = Offset; i < Offset + Size; ++i)
            {
                byte index = (byte)(crc ^ bytes[i]);
                crc = (ushort)((crc >> 8) ^ table[index]);
            }

            return crc;
        }

        static CRC16()
        {
            ushort value;
            ushort temp;

            for (ushort i = 0; i < table.Length; ++i)
            {
                value = 0;
                temp = i;

                for (byte j = 0; j < 8; ++j)
                {
                    if (((value ^ temp) & 0x0001) != 0)
                    {
                        value = (ushort)((value >> 1) ^ polynomial);
                    }
                    else
                    {
                        value >>= 1;
                    }
                    temp >>= 1;
                }

                table[i] = value;
            }
        }
    }

    public static class ExceptionHelper
    {
        public static Exception SetCode(this Exception e, int value)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            FieldInfo fieldInfo = typeof(Exception).GetField("_HResult", flags);

            fieldInfo.SetValue(e, value);

            return e;
        }
    }

    public static class Converter
    {
        public static DateTime ToDateTime32(byte[] array, int startIndex)
        {
            int minutes = array[startIndex] + array[startIndex + 1] * 256 + array[startIndex + 2] * 256 * 256;
            int seconds = array[startIndex + 3];

            return new DateTime(2000, 1, 1).AddMinutes(minutes).AddSeconds(seconds);
        }
        public static byte[] FromDateTime32(DateTime date)
        {
            long minutes = (long)(date.AddSeconds(-date.Second) - new DateTime(2000, 1, 1)).TotalMinutes;
            
            byte[] array;

            array = System.BitConverter.GetBytes(minutes);
            array[3] = (byte)date.Second;

            return array;
        }
        public static byte[] StringToByteArray(string hex)
        {
            if (hex.Length % 2 == 1) throw new Exception("Error in hex value");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }
        public static int GetHexVal(char hex)
        {
            int val = (int)hex;

            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }
    }
}
