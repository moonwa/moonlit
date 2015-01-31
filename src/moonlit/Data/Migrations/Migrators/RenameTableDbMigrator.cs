using System.Data.Common;

namespace Moonlit.Data.Migrations.Migrators
{
    internal class RenameTableDbMigrator : DbMigrator
    {
        private readonly string _tableName;
        private readonly string _newTableName;

        public RenameTableDbMigrator(string tableName, string newTableName)
        {
            _tableName = tableName;
            _newTableName = newTableName;
        }

        public string TableName
        {
            get { return _tableName; }
        }

        public string NewTableName
        {
            get { return _newTableName; }
        }

        protected override MigrationStatement DoCore()
        {
            return new MigrationStatement(string.Format("sp_rename '{0}', '{1}'", Quete(_tableName), _newTableName));
        }
    }
}