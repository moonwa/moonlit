using System.Data.Common;

namespace Moonlit.Data
{
    public interface IDatabase
    {
        DbParameter CreateParameter(string paramName, object value);
        DbDataAdapter CreateDataAdapter();
        DbConnection Connection { get; }
    }
}