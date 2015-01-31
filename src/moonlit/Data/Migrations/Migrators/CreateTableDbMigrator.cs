using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Moonlit.Data.Migrations.Migrators
{
    internal class CreateTableDbMigrator : DbMigrator
    {
        private readonly string _tableName;
        private readonly List<ColumnModel> _columnModels;

        public CreateTableDbMigrator(string tableName, List<ColumnModel> columnModels)
        {
            _tableName = tableName;
            _columnModels = columnModels;
        }

        internal IEnumerable<ColumnModel> ColumnModels
        {
            get { return _columnModels; }
        }

        public string TableName
        {
            get { return _tableName; }
        }


        protected override MigrationStatement DoCore()
        {
            var fields = from x in ColumnModels
                         select Quete(x.Name) + " " + base.ToString(x);
            return new MigrationStatement(string.Format("Create Table {0} ({1})", Quete(_tableName), string.Join(", ", fields.ToArray())));
        }
    }
}