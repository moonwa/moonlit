using System.Collections.Generic;
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.Data.Migrations;
using Moonlit.Data.Migrations.Migrators;

namespace Moonlit.Data.Fixtures.Design.Entity
{
    [TestClass]
    public class DbMigratorTestFixture
    {
        [TestMethod]
        public void Test_CreateTableDbMigrator()
        {
            ColumnBuilder builder = new ColumnBuilder();
            CreateTableDbMigrator migrator = new CreateTableDbMigrator("dbo.Persons", new List<ColumnModel>
                {
                    builder.Int(identity:true, name:"PersonId "),
                    builder.String(nullable:false, name:"FirstName ", maxLength:3),
                    builder.String(nullable:true, name:"Remark ", maxLength:300, isMaxLength:true),
                });
            AssetStatement(migrator.CreateStatement(),
                           "create table [dbo].[persons] ([PersonId] int identity(1, 1) NOT NULL, [FirstName] nvarchar(3) NOT NULL, [Remark] nvarchar(max) NULL)");
        }
        [TestMethod]
        public void Test_DropTableDbMigrator()
        {
            ColumnBuilder builder = new ColumnBuilder();
            var migrator = new DropTableDbMigrator("Persons");
            AssetStatement(migrator.CreateStatement(), "Drop TABLE [Persons]");
        }
        [TestMethod]
        public void Test_RenameTableDbMigrator()
        {
            RenameTableDbMigrator migrator = new RenameTableDbMigrator("Persons", "Users");
            AssetStatement(migrator.CreateStatement(), "SP_RENAME '[Persons]', 'Users'");
        }

        [TestMethod]
        public void Test_CreateIndexDbMigrator()
        {
            ColumnBuilder builder = new ColumnBuilder();
            var migrator = new CreateIndexDbMigrator("Persons", new[] { "FirstName", "LastName" }, true);
            AssetStatement(migrator.CreateStatement(), "CREATE UNIQUE INDEX IX_Persons_Firstname_Lastname ON [Persons]([Firstname],[Lastname])");

            migrator = new CreateIndexDbMigrator("Persons", new[] { "FirstName" }, true);
            AssetStatement(migrator.CreateStatement(), "CREATE UNIQUE INDEX IX_Persons_Firstname ON [Persons]([Firstname])");
        }

        [TestMethod]
        public void Test_DropIndexDbMigrator()
        {
            ColumnBuilder builder = new ColumnBuilder();
            var migrator = new DropIndexDbMigrator("Persons", new[] { "FirstName", "LastName" });
            AssetStatement(migrator.CreateStatement(), "Drop INDEX IX_Persons_Firstname_Lastname ON [Persons]");

            migrator = new DropIndexDbMigrator("Persons", new[] { "FirstName" });
            AssetStatement(migrator.CreateStatement(), "Drop INDEX IX_Persons_Firstname ON [Persons]");
        }
        [TestMethod]
        public void Test_AddColumnDbMigrator()
        {
            ColumnBuilder builder = new ColumnBuilder();
            AddColumnDbMigrator migrator = new AddColumnDbMigrator("Persons", "Remark", builder.String(nullable: false, maxLength: 12));
            AssetStatement(migrator.CreateStatement(), "ALTER TABLE [Persons] ADD [Remark] nvarchar(12) NOT NULL");
        }
        [TestMethod]
        public void Test_AlterColumnDbMigrator()
        {
            ColumnBuilder builder = new ColumnBuilder();
            var migrator = new AlterColumnDbMigrator("Persons", "Remark", builder.String(nullable: false, maxLength: 12));
            AssetStatement(migrator.CreateStatement(), "ALTER TABLE [Persons] ALTER Column [Remark] nvarchar(12) NOT NULL");
        }
        [TestMethod]
        public void Test_RenameColumnDbMigrator()
        {
            RenameColumnDbMigrator migrator = new RenameColumnDbMigrator("Persons", "PersonId", "UserId");
            AssetStatement(migrator.CreateStatement(), "SP_RENAME '[Persons].[PersonId]', 'UserId', 'COLUMN'");
        }
        [TestMethod]
        public void Test_DropColumnDbMigrator()
        {
            ColumnBuilder builder = new ColumnBuilder();
            var migrator = new DropColumnDbMigrator("Persons", "Remark");
            AssetStatement(migrator.CreateStatement(), "ALTER TABLE [Persons] DROP COLUMN [Remark]");
        }
        [TestMethod]
        public void Test_AddPrimaryKeyDbMigrator()
        {
            ColumnBuilder builder = new ColumnBuilder();
            AddPrimaryKeyDbMigrator migrator = new AddPrimaryKeyDbMigrator("Persons", "PersonId");
            AssetStatement(migrator.CreateStatement(), "ALTER TABLE [persons] ADD CONSTRAINT PK_PERSONS PRIMARY KEY ([PersonId])");
        }
        [TestMethod]
        public void Test_DropPrimaryKeyDbMigrator()
        {
            ColumnBuilder builder = new ColumnBuilder();
            var migrator = new DropPrimaryKeyDbMigrator("Persons");
            AssetStatement(migrator.CreateStatement(), "ALTER TABLE [Persons] DROP CONSTRAINT PK_PERSONS");
        }


        private void AssetStatement(MigrationStatement statement, string sql, bool suppressTransaction = false)
        {
            Assert.AreEqual(Trim(sql), Trim(statement.Sql));
            Assert.AreEqual(suppressTransaction, statement.SuppressTransaction);
        }

        private string Trim(string sql)
        {
            if (string.IsNullOrEmpty(sql)) return sql;
            sql = sql.Trim().ToLower();
            return sql;
        }
    }
}
