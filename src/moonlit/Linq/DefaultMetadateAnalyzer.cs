using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Moonlit.Linq.Expressions
{
    public class DefaultMetadateAnalyzer : IMetadateAnalyzer
    {
        public IList<ColumnMetadate> GetColumns(Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => x.Name != "Item").Select(x => new ColumnMetadate() { ColumnName = x.Name, PropertyName = x.Name }).ToList();
        }
    }
}