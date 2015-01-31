using System.Data.Common;

namespace Moonlit.Data.Migrations.Migrators
{
    internal class DropColumnDbMigrator : DbMigrator
    {
        private readonly string _table;
        private readonly string _name;

        public DropColumnDbMigrator(string table, string name)
        {
            _table = table;
            _name = name;
        }

        internal string ColumnName
        {
            get { return _name; }
        }

        internal string TableName
        {
            get { return _table; }
        }
        
        protected override MigrationStatement DoCore()
        {
            return new MigrationStatement(string.Format("ALTER TABLE {0} DROP COLUMN {1}", Quete(_table), Quete(_name)));
        }
    }
}