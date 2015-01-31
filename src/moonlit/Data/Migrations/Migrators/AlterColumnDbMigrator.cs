using System;
using System.Data.Common;

namespace Moonlit.Data.Migrations.Migrators
{
    internal class AlterColumnDbMigrator : DbMigrator
    {
        private readonly string _table;
        private readonly string _name;
        private readonly ColumnModel _column;

        public AlterColumnDbMigrator(string table, string name, ColumnModel column)
        {
            _table = table;
            _name = name;
            _column = column;
        }

        public string TableName { get { return _table; } }
        public string ColumnName
        {
            get { return _name; }
        }

        public ColumnModel Column
        {
            get { return _column; }
        }

        protected override MigrationStatement DoCore()
        {
            return new MigrationStatement(string.Format("ALTER TABLE {0} ALTER COLUMN {1} {2}", Quete(_table), Quete(_name), ToString(_column)));
        }
    }
}