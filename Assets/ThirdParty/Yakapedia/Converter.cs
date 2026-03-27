using System;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Yakapedia
{
    public static class Converter
    {
        private static readonly StringBuilder _stringBuilder = new();
        
        /// <summary>
        /// Inverts the input color
        /// </summary>
        /// <param name="inputColor">Color to invert</param>
        /// <returns>The inverse of the input color</returns>
        public static Color Invert(this ref Color inputColor)
        {
            var inverseColor = new Color(1 - inputColor.r, 1 - inputColor.g, 1 - inputColor.b, inputColor.a);
            return inverseColor;
        }
        
        /// <summary>
        /// Converts an integer value to a Roman numeral string representation. (0 - 3999)
        /// </summary>
        /// <param name="value">The integer value to convert.</param>
        /// <returns>The Roman numeral string representation of the integer value.</returns>
        public static string ConvertToRomanNumeral(this int value)
        {
            string[] romanNumeralSymbols = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
            int[] romanNumeralValues = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };

            _stringBuilder.Length = 0;

            for (int i = 0; i < romanNumeralSymbols.Length; i++)
            {
                while (value >= romanNumeralValues[i])
                {
                    _stringBuilder.Append(romanNumeralSymbols[i]);
                    value -= romanNumeralValues[i];
                }
            }

            return _stringBuilder.ToString();
        }
        
        /// <summary>
        /// Converts string to byte[].
        /// </summary>
        /// <param name="str">string to convert.</param>
        /// <returns>Converted byte[] from string.</returns>
        public static byte[] StringToByte(this string str)
        {
            var chars = str.ToArray();
            var bytes = new byte[chars.Length * 2];

            for (int i = 0; i < chars.Length; i++)
            {
                Array.Copy(BitConverter.GetBytes(chars[i]), 0, bytes, i * 2, 2);
            }

            return bytes;
        }

        /// <summary>
        /// Converts byte[] to string.
        /// </summary>
        /// <param name="bytes">byte[] to convert.</param>
        /// <returns>Converted string from byte[].</returns>
        public static string ByteToString(this byte[] bytes)
        {
            var chars = new char[bytes.Length / 2];

            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = BitConverter.ToChar(bytes, i * 2);
            }

            return new string(chars);
        }
    }
}