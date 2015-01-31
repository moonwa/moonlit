using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Moonlit.Data.Migrations.Migrators
{
    public class TableBuilder<TColumns>
    {
        private readonly DbMigration _dbMigration;
        private readonly string _tableName;
        private readonly TColumns _obj;

        internal TableBuilder(DbMigration dbMigration, string tableName, Func<ColumnBuilder, TColumns> columnsAction)
        {
            _dbMigration = dbMigration;
            _tableName = tableName;
            var builder = new ColumnBuilder();
            _obj = columnsAction(builder);
            var properties = typeof(TColumns).GetProperties();

            List<ColumnModel> columnModels = new List<ColumnModel>();
            foreach (var propertyInfo in properties)
            {
                var columnModel = (ColumnModel)propertyInfo.GetValue(_obj);
                columnModel.Name = columnModel.Name ?? propertyInfo.Name;

                columnModels.Add(columnModel);
            }
            _dbMigration.AddMigrators(new CreateTableDbMigrator(tableName, columnModels));
        }

        public void PrimaryKey(Expression<Func<TColumns, object>> keyExpression)
        {
            var column = (ColumnModel)keyExpression.Compile()(_obj);

            _dbMigration.AddPrimaryKey(_tableName, column.Name);
        }
    }
}