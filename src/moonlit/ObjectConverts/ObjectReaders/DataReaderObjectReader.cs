using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Moonlit.ObjectConverts.ObjectReaders
{
    internal class DataReaderObjectReaderFactory : IObjectReaderFactory
    {

        public IObjectReader CreateReader(object obj)
        {
            DbDataReader reader = obj as DbDataReader;
            if (reader == null)
            {
                return null;
            }
            return new DataReaderObjectReader(reader);
        }

    }
    internal class DataReaderObjectReader : IObjectReader
    {
        private readonly DbDataReader _dataReader;

        public DataReaderObjectReader(DbDataReader dataReader)
        {
            _dataReader = dataReader;

        }

        public object GetValue(string propertyName)
        {
            try
            {
                return _dataReader[propertyName];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public object Value { get { return _dataReader; } }
        public IEnumerable<string> Properties
        {
            get
            {
                foreach (DataRow row in this._dataReader.GetSchemaTable().Rows)
                {
                    yield return row[0].ToString();
                }
            }
        }

        public bool TryGetValue(string propertyName, out object obj)
        {
            obj = null;
            try
            {
                obj = _dataReader[propertyName];
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
