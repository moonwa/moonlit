using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Moonlit.Security.Cryptography
{
    /// <summary>
    /// 字符移位加密算法
    /// </summary>
    public class MoveChar : SymmetricAlgorithm
    {
        public MoveChar()
        {
        }
        private int key = 0;
        public new int Key
        {
            set
            {
                if (value > 25 || value < -25)
                    throw new ArgumentOutOfRangeException("移位加密密钥只能在 -25 ~ 25之间");
                this.key = value;
            }
            get
            {
                return this.key;
            }
        }
        public new static MoveChar Create()
        {
            MoveChar mc = new MoveChar();
            mc.GenerateKey();
            return mc;
        }
        public override ICryptoTransform CreateDecryptor()
        {
            if (this.key == 0)
                throw new ArgumentNullException("移位加密的密钥未初始化");
            else
            {
                int k = -this.key;
                while (k > 26) k -= 26;
                while (k < -26) k += 26;
                return new MoveCharCryptoTransform(k);
            }
        }
        public override ICryptoTransform CreateEncryptor()
        {
            if (this.key == 0)
                throw new ArgumentNullException("移位加密的密钥未初始化");
            else
            {
                return new MoveCharCryptoTransform(this.key);
            }
        }
        public override System.Security.Cryptography.ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override System.Security.Cryptography.ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override void GenerateIV()
        {
            throw new ArgumentException("移位加密暂不支持IV");
        }
        public override void GenerateKey()
        {
            Random rand = new Random();
            this.Key = rand.Next(-25, 25);
            if (this.key == 0)
                this.key++;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class MoveCharCryptoTransform : ICryptoTransform
    {
        /// <summary>
        /// 
        /// </summary>
        public const int blockSize = 1024;
        protected int moveCharCount = 0;
        /// <summary>
        /// Initializes a new instance of the <see cref="MoveCharCryptoTransform"/> class.
        /// </summary>
        /// <param name="movecount">The movecount.</param>
        public MoveCharCryptoTransform(int movecount)
        {
            this.moveCharCount = movecount;
        }

        #region ICryptoTransform Members
        /// <summary>
        /// Gets a value indicating whether the current transform can be reused.
        /// </summary>
        /// <value></value>
        /// <returns>true if the current transform can be reused; otherwise, false.
        /// </returns>
        public bool CanReuseTransform
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether multiple blocks can be transformed.
        /// </summary>
        /// <value></value>
        /// <returns>true if multiple blocks can be transformed; otherwise, false.
        /// </returns>
        public bool CanTransformMultipleBlocks
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the input block size.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The size of the input data blocks in bytes.
        /// </returns>
        public int InputBlockSize
        {
            get { return MoveCharCryptoTransform.blockSize; }
        }

        /// <summary>
        /// Gets the output block size.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The size of the output data blocks in bytes.
        /// </returns>
        public int OutputBlockSize
        {
            get { return MoveCharCryptoTransform.blockSize; }
        }

        /// <summary>
        /// Transforms the specified region of the input byte array and copies the resulting transform to the specified region of the output byte array.
        /// </summary>
        /// <param name="inputBuffer">The input for which to compute the transform.</param>
        /// <param name="inputOffset">The offset into the input byte array from which to begin using data.</param>
        /// <param name="inputCount">The number of bytes in the input byte array to use as data.</param>
        /// <param name="outputBuffer">The output to which to write the transform.</param>
        /// <param name="outputOffset">The offset into the output byte array from which to begin writing data.</param>
        /// <returns>The number of bytes written.</returns>
        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {

            // 将值改变为如 key = 1, 则将 'c' 变为 'D', 空格不变

            int retcount = 0;
            for (int i = 0; i < inputCount; i++)
            {
                char c = (char)inputBuffer[inputOffset + i];
                if (!char.IsLetter(c))
                    continue;
                this.TransformChar(ref c);
                outputBuffer[i] = (byte)c;
                retcount++;
            }
            return inputCount;
        }
        private void TransformChar(ref char c)
        {
            int intvalue = (int)c + this.moveCharCount;
            c = (char)intvalue;
            if (!char.IsLetter(c))
            {
                if (this.moveCharCount > 0)
                {
                    intvalue -= 26;
                }
                else
                {
                    intvalue += 26;
                }
                c = (char)intvalue;
            }
            if (char.IsLower(c))
            {
                c = char.ToUpper(c);
            }
            else if (char.IsUpper(c))
            {
                c = char.ToLower(c);
            }
        }
        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            byte[] outputBuffer = new byte[inputCount];
            this.TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, 0);
            return outputBuffer;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }

}
