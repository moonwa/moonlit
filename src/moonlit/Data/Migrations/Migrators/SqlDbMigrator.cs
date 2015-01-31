using System.Data.Common;

namespace Moonlit.Data.Migrations.Migrators
{
    internal class SqlDbMigrator : DbMigrator
    {
        private readonly string _sql;

        public SqlDbMigrator(string sql)
        {
            _sql = sql;
        }

        public string Sql
        {
            get { return _sql; }
        }

        protected override MigrationStatement DoCore()
        {
            return new MigrationStatement(_sql);
        }
    }
}