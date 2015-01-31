using System;
using System.Security.Cryptography;

namespace Moonlit.Security
{
    /// <summary>
    /// 
    /// </summary>
    public static class RsaExtensions
    {

        public static byte[] BlockEncrypt(this RSACryptoServiceProvider rsa, byte[] pbBuffer)
        {
            // Setup the return buffer
            System.IO.MemoryStream stream = new System.IO.MemoryStream();

            // The maximum block size is the length of the modulus in bytes
            // minus 11 bytes for padding.
            int nMaxBlockSize = rsa.ExportParameters(false).Modulus.Length - 11;
            int nLength = pbBuffer.Length;
            int nBlocks = ((nLength % nMaxBlockSize) == 0) ? nLength / nMaxBlockSize : nLength / nMaxBlockSize + 1;
            int nTotalBytes = 0;

            for (int i = 0; i < nBlocks; i++)
            {
                // Calculate the block length
                int nBlockLength = (i == (nBlocks - 1) ? nLength - (i * nMaxBlockSize) : nMaxBlockSize);

                // Allocate a new block and copy data from the main buffer
                byte[] pbBlock = new byte[nBlockLength];
                Array.Copy(pbBuffer, i * nMaxBlockSize, pbBlock, 0, nBlockLength);

                // Encrypt the block
                byte[] pbOut = rsa.Encrypt(pbBlock, false);

                // Copy the block to the output stream
                stream.Write(pbOut, 0, pbOut.Length);

                // Keep a count of the encrypted bytes
                nTotalBytes += pbOut.Length;
            }

            // Create an output buffer
            byte[] pbReturn = new byte[nTotalBytes];
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            stream.Read(pbReturn, 0, nTotalBytes);

            // Return the data
            return pbReturn;
        }

        public static byte[] BlockDecrypt(this RSACryptoServiceProvider rsa, byte[] pbBuffer)
        {
            // Setup the return buffer
            System.IO.MemoryStream stream = new System.IO.MemoryStream();

            // The maximum block size is the length of the modulus in bytes
            int nMaxBlockSize = rsa.ExportParameters(false).Modulus.Length;
            int nLength = pbBuffer.Length;
            int nBlocks = ((nLength % nMaxBlockSize) == 0) ? nLength / nMaxBlockSize : nLength / nMaxBlockSize + 1;
            int nTotalBytes = 0;

            for (int i = 0; i < nBlocks; i++)
            {
                // Calculate the block length
                int nBlockLength = (i == (nBlocks - 1) ? nLength - (i * nMaxBlockSize) : nMaxBlockSize);

                // Allocate a new block and copy data from the main buffer
                byte[] pbBlock = new byte[nBlockLength];
                Array.Copy(pbBuffer, i * nMaxBlockSize, pbBlock, 0, nBlockLength);

                // Encrypt the block
                byte[] pbOut = rsa.Decrypt(pbBlock, false);

                // Copy the block to the output stream
                stream.Write(pbOut, 0, pbOut.Length);

                // Keep a count of the encrypted bytes
                nTotalBytes += pbOut.Length;
            }

            // Create an output buffer
            byte[] pbReturn = new byte[nTotalBytes];
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            stream.Read(pbReturn, 0, nTotalBytes);

            // Return the data
            return pbReturn;
        }
    }
}