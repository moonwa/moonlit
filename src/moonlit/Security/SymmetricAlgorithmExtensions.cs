using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Moonlit.Security
{
    /// <summary>
    /// 
    /// </summary>
    public static class SymmetricAlgorithmExtensions
    {
        /// <summary>
        /// Encryptors the specified algorithm.
        /// </summary>
        /// <param name="algorithm">The algorithm.</param>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static byte[] Encryptor(this SymmetricAlgorithm algorithm, byte[] data, int offset, int count)
        {
            MemoryStream ms = new MemoryStream();
            System.Security.Cryptography.CryptoStream cryptoStream = new CryptoStream(ms, algorithm.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(data, offset, count);
            cryptoStream.FlushFinalBlock();
            return ms.ToArray();
        }
        /// <summary>
        /// Dencryptors the specified algorithm.
        /// </summary>
        /// <param name="algorithm">The algorithm.</param>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static byte[] Dencryptor(this SymmetricAlgorithm algorithm, byte[] data, int offset, int count)
        {
            MemoryStream tmp = new MemoryStream(data);
            tmp.Position = 0;
            System.Security.Cryptography.CryptoStream cryptoStream = new CryptoStream(tmp, algorithm.CreateDecryptor(), CryptoStreamMode.Read);
            MemoryStream ms = new MemoryStream();

            byte[] buffer = new byte[1024];
            int readCount = -1;
            do
            {
                readCount = cryptoStream.Read(buffer, 0, 1024);
                ms.Write(buffer, 0, readCount);
            }
            while (readCount != 0);
            return ms.ToArray();
        }
    }
}
