using System;
using System.Collections.Generic;
using Moonlit.Data.Migrations.Migrators;

namespace Moonlit.Data.Migrations
{
    public abstract class DbMigration
    {
        public DbMigration()
        {
            _migrators = new List<DbMigrator>();
        }
        private List<DbMigrator> _migrators;
        public abstract void Up();

        internal IEnumerable<DbMigrator> Migrators
        {
            get { return _migrators; }
        }

        public abstract Version Version { get; }
        public void Clean()
        {
            _migrators.Clear();
        }
        protected internal TableBuilder<TColumns> CreateTable<TColumns>(string name, Func<ColumnBuilder, TColumns> columnsAction)
        {
            return new TableBuilder<TColumns>(this, name, columnsAction);
        }
        protected internal void DropTable(string name)
        {
            this._migrators.Add(new DropTableDbMigrator(name));
        }
        protected internal void RenameTable(string name, string newName)
        {
            this._migrators.Add(new RenameTableDbMigrator(name, newName));
        }
        protected internal void RenameColumn(string table, string name, string newName)
        {
            this._migrators.Add(new RenameColumnDbMigrator(table, name, newName));
        }
        protected internal void AddColumn(string table, string name, Func<ColumnBuilder, ColumnModel> columnAction)
        {
            this._migrators.Add(new AddColumnDbMigrator(table, name, columnAction(new ColumnBuilder())));
        }
        protected internal void DropColumn(string table, string name)
        {
            this._migrators.Add(new DropColumnDbMigrator(table, name));
        }

        public abstract string Target { get; }
        protected internal void AlterColumn(string table, string name, Func<ColumnBuilder, ColumnModel> columnAction)
        {
            var column = columnAction(new ColumnBuilder());
            this._migrators.Add(new AlterColumnDbMigrator(table, name, column));
        }
        protected internal void AddPrimaryKey(string table, string column)
        {
            this._migrators.Add(new AddPrimaryKeyDbMigrator(table, column));
        }
        protected internal void DropPrimaryKey(string table)
        {
            this._migrators.Add(new DropPrimaryKeyDbMigrator(table));
        }
        protected internal void CreateIndex(string table, string column, bool unique = false)
        {
            CreateIndex(table, new[] { column }, unique);
        }
        protected internal void CreateIndex(string table, string[] columns, bool unique = false)
        {
            this._migrators.Add(new CreateIndexDbMigrator(table, columns, unique));

        }
        protected internal void DropIndex(string table, string name)
        {
            DropIndex(table, new[] { name });
        }
        protected internal void DropIndex(string table, string[] names)
        {
            this._migrators.Add(new DropIndexDbMigrator(table, names));
        }

        internal void AddMigrators(DbMigrator dbMigrator)
        {
            _migrators.Add(dbMigrator);
        }
        protected internal void Sql(string sql)
        {
            this._migrators.Add(new SqlDbMigrator(sql));
        }
    }
}