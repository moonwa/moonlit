using System.Data.Common;

namespace Moonlit.Data.Migrations.Migrators
{
    internal class AddPrimaryKeyDbMigrator : DbMigrator
    {
        private readonly string _table;
        private readonly string _name;

        public AddPrimaryKeyDbMigrator(string table, string name)
        {
            _table = table;
            _name = name;
        }

        public string TableName
        {
            get { return _table; }
        }
        public string ColumnName
        {
            get { return _name; }
        }

        protected override MigrationStatement DoCore()
        {
            return new MigrationStatement(string.Format("ALTER TABLE {0} add CONSTRAINT PK_{1} PRIMARY KEY ({2})", Quete(_table), Name(_table), Quete(_name)));
        }
    }
}