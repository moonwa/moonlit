using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moonlit
{
    public static class RandomHelper
    {
        public static string GenerateNumber(int length)
        {
            string buffer = "";

            while (buffer.Length < length)
            {
                var code = Guid.NewGuid().ToString().GetHashCode();
                code = Math.Abs(code);
                var s = code.ToString();
                buffer += s;
            }
            if (buffer.Length - length < 0)
                Console.WriteLine(1);
            return buffer.Substring(buffer.Length - length);
        }
    }
}
