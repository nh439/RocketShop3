using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.NumberCalculation
{
    public struct Hexadecimal
    {
        public string Value { get; init; }
        public Hexadecimal(string value)
        {
            HashSet<char> allowedChars = new HashSet<char>("1234567890ABCDEF");
            bool isValid = true;
            if(value.StartsWith("0x"))
            {
                value = value.Substring(2);
            }
            foreach (char c in value)
            {
                if (!allowedChars.Contains(c))
                {
                    isValid = false;
                    break;
                }
            }
            if (isValid)
            {
                Value = value.ToUpper();
            }
            else
            {
                throw new ArgumentException("Invalid hexadecimal value");
            }
        }
        public Hexadecimal(byte[] data)
        {
            string hex = BitConverter.ToString(data).Replace("-", "");
            Value = hex;
        }
        public byte[] ToByteArray()
        {
            byte[] data = new byte[Value.Length / 2];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Convert.ToByte(Value.Substring(i * 2, 2), 16);
            }
            return data;
        }
        public string ToBase64String() => Convert.ToBase64String(ToByteArray());


        public static implicit operator Hexadecimal(byte value) => new Hexadecimal(value.ToString("X"));
        public static implicit operator Hexadecimal(short value) => new Hexadecimal(value.ToString("X"));
        public static implicit operator Hexadecimal(int value) => new Hexadecimal(value.ToString("X"));
        public static implicit operator Hexadecimal(long value) => new Hexadecimal(value.ToString("X"));
        public static implicit operator Hexadecimal(Int128 value) => new Hexadecimal(value.ToString("X"));

        public override string ToString() => Value;

        public byte ToByte() => Convert.ToByte(Value, 16);
        public short ToShort() => Convert.ToInt16(Value, 16);
        public int ToInt() => Convert.ToInt32(Value, 16);
        public long ToLong() => Convert.ToInt64(Value, 16);
        public Int128 ToInt128() => Int128.Parse(Value, System.Globalization.NumberStyles.HexNumber);

        byte ConvertCharecterToByte(char c)
        {
           switch(c)
            {
                case '0': return 0;
                case '1': return 1;
                case '2': return 2;
                case '3': return 3;
                case '4': return 4;
                case '5': return 5;
                case '6': return 6;
                case '7': return 7;
                case '8': return 8;
                case '9': return 9;
                case 'A': return 10;
                case 'B': return 11;
                case 'C': return 12;
                case 'D': return 13;
                case 'E': return 14;
                case 'F': return 15;
                default: throw new ArgumentException("Invalid hexadecimal value");
            }
        }
    }
}
