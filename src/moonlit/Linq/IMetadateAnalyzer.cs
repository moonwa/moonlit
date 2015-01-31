using System;
using System.Collections.Generic;

namespace Moonlit.Linq.Expressions
{
    public interface IMetadateAnalyzer
    {
        IList<ColumnMetadate> GetColumns(Type type);
    }
}