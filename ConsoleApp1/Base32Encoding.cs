using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class Base32Encoding
    {
        public static string ToBase32String(byte[] data)
        {
            const string base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            int i = 0, index = 0, digit = 0;
            int currentByte, nextByte;
            StringBuilder result = new StringBuilder((data.Length + 7) * 8 / 5);

            while (i < data.Length)
            {
                currentByte = (data[i] >= 0) ? data[i] : (data[i] + 256); // Convert to unsigned

                // Is the current digit going to span a byte boundary?
                if (index > 3)
                {
                    if ((i + 1) < data.Length)
                    {
                        nextByte = (data[i + 1] >= 0) ? data[i + 1] : (data[i + 1] + 256);
                    }
                    else
                    {
                        nextByte = 0;
                    }

                    digit = currentByte & (0xFF >> index);
                    index = (index + 5) % 8;
                    digit <<= index;
                    digit |= nextByte >> (8 - index);
                    i++;
                }
                else
                {
                    digit = (currentByte >> (8 - (index + 5))) & 0x1F;
                    index = (index + 5) % 8;
                    if (index == 0)
                        i++;
                }
                result.Append(base32Chars[digit]);
            }

            return result.ToString();
        }
    }
}
