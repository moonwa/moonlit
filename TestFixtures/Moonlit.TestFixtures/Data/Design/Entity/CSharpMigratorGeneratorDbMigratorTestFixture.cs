using System;
using System.CodeDom.Compiler;
using System.Data.Common;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.Data.Design.Entity;
using Moonlit.Data.Migrations;
using Moonlit.Data.Migrations.Migrators;

namespace Moonlit.Data.Fixtures.Design.Entity
{
    [TestClass]
    public class CSharpMigratorsCodeGeneratorFixture
    {
        class CreateTableMigration : DbMigration
        {
            public override void Up()
            {
                CreateTable("Persons", x => new
                                                {
                                                    PersonId = x.Int(nullable: false, identity: true),
                                                    FirstName = x.String(nullable: false, maxLength: 30)
                                                });
            }

            public override Version Version
            {
                get { return new Version("0.0.1.0"); }
            }

            public override string Target
            {
                get { return "123"; }
            }
        }
        class RenameTableMigration : DbMigration
        {
            public override void Up()
            {
                RenameTable("Persons", "People");
            }

            public override Version Version
            {
                get { return new Version("0.0.2.0"); }
            }

            public override string Target
            {
                get { return "333"; }
            }
        }
        class DropTableMigration : DbMigration
        {
            public override void Up()
            {
                DropTable("Persons");
            }

            public override Version Version
            {
                get { return new Version("0.0.1.0"); }
            }

            public override string Target
            {
                get { return "123"; }
            }
        }
        class CreateIndexMigration : DbMigration
        {
            public override void Up()
            {
                CreateIndex("Persons", "LastName", unique: true);
            }

            public override Version Version
            {
                get { return new Version("0.0.1.0"); }
            }

            public override string Target
            {
                get { return "123"; }
            }
        }
        class DropIndexMigration : DbMigration
        {
            public override void Up()
            {
                DropIndex("Persons", "LastName");
            }

            public override Version Version
            {
                get { return new Version("0.0.1.0"); }
            }

            public override string Target
            {
                get { return "123"; }
            }
        }
        class AddPrimaryKeyMigration : DbMigration
        {
            public override void Up()
            {
                AddPrimaryKey("Persons", "PersonId");
            }

            public override Version Version
            {
                get { return new Version("0.0.2.0"); }
            }

            public override string Target
            {
                get { return "333"; }
            }
        }
        class DropPrimaryKeyMigration : DbMigration
        {
            public override void Up()
            {
                DropPrimaryKey("Persons");
            }

            public override Version Version
            {
                get { return new Version("0.0.2.0"); }
            }

            public override string Target
            {
                get { return "333"; }
            }
        }
        class AddColumnMigration : DbMigration
        {
            public override void Up()
            {
                AddColumn("Persons", "IsDeleted", x => x.Boolean(nullable: false));
                AddColumn("Persons", "CreationTime", x => x.DateTime(nullable: false));
                AddColumn("Persons", "Amount", x => x.Decimal(nullable: false));
            }

            public override Version Version
            {
                get { return new Version("0.0.2.0"); }
            }

            public override string Target
            {
                get { return "333"; }
            }
        }
        class AlterColumnMigration : DbMigration
        {
            public override void Up()
            {
                AlterColumn("Persons", "IsDeleted", x => x.Boolean(nullable: false));
                AlterColumn("Persons", "CreationTime", x => x.DateTime(nullable: false));
                AlterColumn("Persons", "Amount", x => x.Decimal(nullable: false));
            }

            public override Version Version
            {
                get { return new Version("0.0.2.0"); }
            }

            public override string Target
            {
                get { return "333"; }
            }
        }
        class RenameColumnMigration : DbMigration
        {
            public override void Up()
            {
                RenameColumn("Persons", "Name", "FirstName");
            }

            public override Version Version
            {
                get { return new Version("0.0.2.0"); }
            }

            public override string Target
            {
                get { return "333"; }
            }
        }
        class DropColumnMigration : DbMigration
        {
            public override void Up()
            {
                DropColumn("Persons", "PersonId2");
            }

            public override Version Version
            {
                get { return new Version("0.0.2.0"); }
            }

            public override string Target
            {
                get { return "333"; }
            }
        }
        [TestMethod]
        public void Test_CreateTable()
        {
            AssetDbMigration(new CreateTableMigration());
        }
        [TestMethod]
        public void Test_RenameTable()
        {
            AssetDbMigration(new RenameTableMigration());
        }
        [TestMethod]
        public void Test_DropTable()
        {
            AssetDbMigration(new DropTableMigration());
        }

        [TestMethod]
        public void Test_AddPrimaryKey()
        {
            AssetDbMigration(new AddPrimaryKeyMigration());
        }
        [TestMethod]
        public void Test_DropPrimaryKey()
        {
            AssetDbMigration(new DropPrimaryKeyMigration());
        }
        [TestMethod]
        public void Test_AddColumn()
        {
            AssetDbMigration(new AddColumnMigration());
        }
        [TestMethod]
        public void Test_DropColumn()
        {
            AssetDbMigration(new DropColumnMigration());
        }
        [TestMethod]
        public void Test_AlterColumn()
        {
            AssetDbMigration(new AlterColumnMigration());
        }
        [TestMethod]
        public void Test_RenameColumn()
        {
            AssetDbMigration(new RenameColumnMigration());
        }
        [TestMethod]
        public void Test_CreateIndex()
        {
            AssetDbMigration(new CreateIndexMigration());
        }
        [TestMethod]
        public void Test_DropIndex()
        {
            AssetDbMigration(new DropIndexMigration());
        }

        private void AssetDbMigration(DbMigration migration)
        {
            migration.Up();
            CSharpMigratorsCodeGenerator generator = new CSharpMigratorsCodeGenerator(migration.Migrators, migration.Target);
            var text = generator.Generate("P1", "PN1", migration.Version);
            Microsoft.CSharp.CSharpCodeProvider provider = new CSharpCodeProvider();
            var tmpFile = Path.GetTempFileName();
            File.WriteAllText(tmpFile, text);
            var compilerParameters = new CompilerParameters()
                                         {
                                             GenerateInMemory = true,
                                             ReferencedAssemblies =
                                                 {
                                                     GetLocation(typeof (DbCommand).Assembly),
                                                     GetLocation(typeof (DbContext).Assembly),
                                                 }
                                         };
            var r = provider.CompileAssemblyFromFile(compilerParameters, tmpFile);
            var errors = r.Errors;
            Console.WriteLine(text);
            Assert.IsFalse(errors.HasErrors, "Build Errors");
            var type = r.CompiledAssembly.GetTypes().FirstOrDefault(x => x.Name == "P1");
            DbMigration dbMigration = (DbMigration)Activator.CreateInstance(type);
            dbMigration.Up();
            AssertMigration(migration, dbMigration);
        }

        private static string GetLocation(Assembly assembly)
        {
            return new Uri(assembly.CodeBase).LocalPath;
        }

        private void AssertMigration(DbMigration expect, DbMigration actual)
        {
            Assert.AreEqual(expect.Version, actual.Version, "Check Version Error");
            Assert.AreEqual(expect.Target, actual.Target, "Check Target Error");
            Assert.AreEqual(expect.Migrators.Count(), actual.Migrators.Count(), "Check Migrators Count");
            for (int i = 0; i < expect.Migrators.Count(); i++)
            {
                var expectMigrator = expect.Migrators.Skip(i).First();
                var actualMigrator = actual.Migrators.Skip(i).First();
                Assert.AreEqual(expectMigrator.GetType(), actualMigrator.GetType());

                AssertMigrator(expectMigrator, actualMigrator);
            }
        }

        private void AssertMigrator(DbMigrator expect, DbMigrator actual)
        {
            var migratorType = expect.GetType();
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod;
            var assetMethod = this.GetType().GetMethod("AssertSpecialMigrator", bindingFlags, null, new[] { migratorType, migratorType }, null);
            Assert.IsNotNull(assetMethod, "AssertSpecialMigrator Not Exist for type " + migratorType.FullName);
            assetMethod.Invoke(this, new[] { expect, actual });
        }
        private void AssertSpecialMigrator(CreateTableDbMigrator expect, CreateTableDbMigrator actual)
        {
            Assert.AreEqual(expect.TableName, actual.TableName);
            Assert.AreEqual(expect.ColumnModels.Count(), actual.ColumnModels.Count());
            for (int i = 0; i < expect.ColumnModels.Count(); i++)
            {
                var expectColumn = expect.ColumnModels.Skip(i).First();
                var actualColumn = actual.ColumnModels.Skip(i).First();

                AssertColumn(expectColumn, actualColumn);
            }
        }
        private void AssertSpecialMigrator(RenameTableDbMigrator expect, RenameTableDbMigrator actual)
        {
            Assert.AreEqual(expect.TableName, actual.TableName);
            Assert.AreEqual(expect.NewTableName, actual.NewTableName);
        }
        private void AssertSpecialMigrator(DropTableDbMigrator expect, DropTableDbMigrator actual)
        {
            Assert.AreEqual(expect.TableName, actual.TableName);
        }
        private void AssertSpecialMigrator(AddPrimaryKeyDbMigrator expect, AddPrimaryKeyDbMigrator actual)
        {
            Assert.AreEqual(expect.TableName, actual.TableName);
            Assert.AreEqual(expect.ColumnName, actual.ColumnName);
        }
        private void AssertSpecialMigrator(DropPrimaryKeyDbMigrator expect, DropPrimaryKeyDbMigrator actual)
        {
            Assert.AreEqual(expect.TableName, actual.TableName);
        }
        private void AssertSpecialMigrator(AddColumnDbMigrator expect, AddColumnDbMigrator actual)
        {
            Assert.AreEqual(expect.TableName, actual.TableName);
            Assert.AreEqual(expect.ColumnName, actual.ColumnName);
            AssertColumn(expect.Column, actual.Column);
        }
        private void AssertSpecialMigrator(AlterColumnDbMigrator expect, AlterColumnDbMigrator actual)
        {
            Assert.AreEqual(expect.TableName, actual.TableName);
            Assert.AreEqual(expect.ColumnName, actual.ColumnName);
            AssertColumn(expect.Column, actual.Column);
        }
        private void AssertSpecialMigrator(RenameColumnDbMigrator expect, RenameColumnDbMigrator actual)
        {
            Assert.AreEqual(expect.TableName, actual.TableName);
            Assert.AreEqual(expect.ColumnName, actual.ColumnName);
            Assert.AreEqual(expect.NewColumnName, actual.NewColumnName);
        }
        private void AssertSpecialMigrator(DropColumnDbMigrator expect, DropColumnDbMigrator actual)
        {
            Assert.AreEqual(expect.TableName, actual.TableName);
            Assert.AreEqual(expect.ColumnName, actual.ColumnName);
        }

        private void AssertSpecialMigrator(CreateIndexDbMigrator expect, CreateIndexDbMigrator actual)
        {
            Assert.AreEqual(expect.TableName, actual.TableName);
            Assert.AreEqual(expect.Unique, actual.Unique);
            CollectionAssert.AreEqual(expect.Columns, actual.Columns);
        }

        private void AssertSpecialMigrator(DropIndexDbMigrator expect, DropIndexDbMigrator actual)
        {
            Assert.AreEqual(expect.TableName, actual.TableName);
            CollectionAssert.AreEqual(expect.Columns, actual.Columns);
        }

        private static void AssertColumn(ColumnModel expectColumn, ColumnModel actualColumn)
        {
            Assert.AreEqual(expectColumn.FixedLength, actualColumn.FixedLength);
            Assert.AreEqual(expectColumn.Identity, actualColumn.Identity);
            Assert.AreEqual(expectColumn.IsMaxLength, actualColumn.IsMaxLength);
            Assert.AreEqual(expectColumn.MaxLength, actualColumn.MaxLength);
            Assert.AreEqual(expectColumn.Name, actualColumn.Name);
            Assert.AreEqual(expectColumn.Nullable, actualColumn.Nullable);
            Assert.AreEqual(expectColumn.Precision, actualColumn.Precision);
            Assert.AreEqual(expectColumn.StoreType, actualColumn.StoreType);
        }
    }
}