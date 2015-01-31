using System.Data.Common;

namespace Moonlit.Data.Migrations.Migrators
{
    internal class DropTableDbMigrator : DbMigrator
    {
        private readonly string _name;

        public DropTableDbMigrator(string name)
        {
            _name = name;
        }

        internal string TableName
        {
            get { return _name; }
        }
        
        protected override MigrationStatement DoCore()
        {
            return new MigrationStatement(string.Format("DROP TABLE {0}", Quete(_name)));
        }
    }
}