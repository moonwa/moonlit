using System.Data.Common;

namespace Moonlit.Data.Migrations.Migrators
{
    internal class RenameColumnDbMigrator : DbMigrator
    {
        private readonly string _table;
        private readonly string _name;
        private readonly string _newName;

        public RenameColumnDbMigrator(string table, string name, string newName)
        {
            _table = table;
            _name = name;
            _newName = newName;
        }

        public string NewColumnName
        {
            get { return _newName; }
        }

        public string ColumnName
        {
            get { return _name; }
        }

        public string TableName
        {
            get { return _table; }
        }


        protected override MigrationStatement DoCore()
        {
            return new MigrationStatement(string.Format("sp_rename '{0}.{1}', '{2}', 'COLUMN'",  Quete(_table), Quete(_name), _newName));
        }
    }
}