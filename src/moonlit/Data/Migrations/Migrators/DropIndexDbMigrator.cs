using System.Data.Common;

namespace Moonlit.Data.Migrations.Migrators
{
    internal class DropIndexDbMigrator : DbMigrator
    {
        private readonly string _table;
        private readonly string[] _columns;

        public DropIndexDbMigrator(string table, string[] columns)
        {
            _table = table;
            _columns = columns;
        }

        public string TableName
        {
            get { return _table; }
        }

        public string[] Columns
        {
            get { return _columns; }
        }


        protected override MigrationStatement DoCore()
        {
            return new MigrationStatement(string.Format("DROP INDEX IX_{0}_{1} ON {2}", Name(_table), Name(_columns), Quete(_table)));
        }
    }
}