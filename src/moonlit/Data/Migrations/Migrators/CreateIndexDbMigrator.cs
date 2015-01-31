using System.Data.Common;
using System.Linq;

namespace Moonlit.Data.Migrations.Migrators
{
    internal class CreateIndexDbMigrator : DbMigrator
    {
        private readonly string _table;
        private readonly string[] _columns;
        private readonly bool _unique;

        public CreateIndexDbMigrator(string table, string[] columns, bool unique)
        {
            _table = table;
            _columns = columns;
            _unique = unique;
        }

        public string TableName
        {
            get { return _table; }
        }

        public string[] Columns
        {
            get { return _columns; }
        }

        public bool Unique
        {
            get { return _unique; }
        }


        protected override MigrationStatement DoCore()
        {
            var columns = string.Join(",", Columns.Select(x => Quete(x)).ToArray());
            return new MigrationStatement(string.Format("CREATE {4} INDEX IX_{0}_{1} on {2}({3})", Name(_table), Name(Columns), Quete(_table), columns, Unique ? "UNIQUE" : ""));
        }
    }
}