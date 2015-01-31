using System.Data.Common;

namespace Moonlit.Data.Migrations.Migrators
{
    internal class DropPrimaryKeyDbMigrator : DbMigrator
    {
        private readonly string _table;

        internal DropPrimaryKeyDbMigrator(string table)
        {
            _table = table;
        }

        internal string TableName
        {
            get { return _table; }
        }
        
        protected override MigrationStatement DoCore()
        {
            return new MigrationStatement(string.Format("ALTER TABLE {0} DROP CONSTRAINT PK_{1}", Quete(_table), Name(_table)));
        }
    }
}