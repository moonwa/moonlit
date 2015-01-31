namespace Moonlit.Data.Migrations.Migrators
{
    internal class MigrationStatement
    {
        public MigrationStatement(string sql) : this(sql, false) { }

        public MigrationStatement(string sql, bool suppressTransaction)
        {
            Sql = sql;
            SuppressTransaction = suppressTransaction;
        }

        public string Sql { get; set; }
        public bool SuppressTransaction { get; set; }
    }
}