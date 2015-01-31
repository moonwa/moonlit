using System;
using System.Text;

namespace Moonlit.Collections
{
    /// <summary>
    /// ��Ϣ��
    /// </summary>
    public class MessageBlock
    {
        private int _index;
        private byte[] _data;

        #region Ctor
        /// <summary>
        /// ����һ��MessageBlock
        /// </summary>
        /// <param name="data">����</param>
        public MessageBlock(byte[] data)
        {
            this.Write(data);
        }
        /// <summary>
        ///  ����һ����MessageBlock
        /// </summary>
        public MessageBlock()
        {
            this.Write(new byte[0] { });
        }
        #endregion
        #region Length
        /// <summary>
        /// ���ݻ��泤��
        /// </summary>
        public int Length
        {
            get
            {
                return this._data.Length;
            }
        }
        #endregion

        #region IsEmpty
        /// <summary>
        /// ��Ϣ���Ƿ��Ѿ�����
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return this._data.Length == this.Index;
            }
        }
        #endregion

        #region Remain Len

        /// <summary>
        /// ��ʣ�¶�����Ϣû����
        /// </summary>
        public int RemainLength
        {
            get
            {
                return this._data.Length - this.Index;
            }
        }
        #endregion

        #region Data
        /// <summary>
        /// ��Ϣ������
        /// </summary>
        public byte[] Data
        {
            get
            {
                return this._data;
            }
        }
        #endregion

        #region Index
        /// <summary>
        /// ��ǰ����
        /// </summary>
        public int Index
        {
            get
            {
                return this._index;
            }
        }
        #endregion

        #region Read
        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="length">Ҫ��ȡ�ĳ��ȣ������ȡ�ĳ��ȴ���ʣ�೤�ȣ���ֻ��ȡʣ�೤��</param>
        /// <returns></returns>
        public byte[] Read(int length)
        {
            int remain_size = this._data.Length - this._index;
            length = (length > remain_size ? remain_size : length);

            byte[] bytes_ret = new byte[length];
            Buffer.BlockCopy(this._data, this._index, bytes_ret, 0, length);
            this._index += length;
            return bytes_ret;

        }
        #endregion

        //public byte[] ReadNoReturn(int length)
        //{
        //    int remain_size = this._data.Length - this._index;
        //    length = (length > remain_size ? remain_size : length);

        //    byte[] bytes_ret = new byte[length];
        //    Buffer.BlockCopy(this._data, this._index, bytes_ret, 0, length);
        //    this._index += length;
        //    return bytes_ret;
        //}

        #region Write
        /// <summary>
        /// д���ݣ�һ����д���ݣ�������������0
        /// </summary>
        /// <param name="data">Ҫд������</param>
        public void Write(byte[] data)
        {
            this._data = data;
            this._index = 0;
        }
        #endregion

        #region Rewrite
        /// <summary>
        /// ��������λ�������ƶ�length
        /// </summary>
        /// <param name="length">Ҫ�ƶ���ƫ����</param>
        public void Rewrite(int length)
        {
            if (this._index > length)
            {
                this._index -= length;
            }
            else
            {
                this._index = 0;
            }
        }
        #endregion

    }
}
