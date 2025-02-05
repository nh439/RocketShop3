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
    }
}
