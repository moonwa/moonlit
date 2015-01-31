using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.Data.Design.Entity;
using Moonlit.Data.Migrations;
using Moonlit.Data.Migrations.Migrators;

namespace Moonlit.Data.Fixtures.Design.Entity
{
    [TestClass]
    public class MigrationFacadeFixture
    { 
        [TestMethod]
        public void TestCreateTable()
        {
            var migrationFacade = new MigrationFacade();
            MigratorsCodeGenerator codeGenerator = migrationFacade.GetMigratorsCodeGenerator(
                new List<ModelMetadata>(),
                new List<ModelMetadata>
                    {
                        new ModelMetadata
                            {
                                Name = "Persons",
                                Properties =
                                    {
                                        new PropertyMetadata
                                            {
                                                IsKey = true,
                                                Name = "UserId",
                                                Type  = typeof (int),
                                            },
                                        new PropertyMetadata
                                            {
                                                IsKey = false,
                                                Name = "FirstName",
                                                MaxLength = 12,
                                                IsNullable = false,
                                                Type  = typeof (string) ,
                                            },
                                        new PropertyMetadata
                                            {
                                                IsKey = false,
                                                Name = "Remark",
                                                MaxLength = int.MaxValue,
                                                IsNullable = true,
                                                Type = typeof (string),
                                            }
                                    }
                            },
                    });
            Assert.AreEqual(2, codeGenerator.Migrators.Count());
            var createTableDbMigrator = codeGenerator.Migrators.First() as CreateTableDbMigrator;
            Assert.IsNotNull(createTableDbMigrator);
            Assert.AreEqual(3, createTableDbMigrator.ColumnModels.Count());
            AssertColumn(new ColumnModel
                {
                    Identity = true,
                    Name = "UserId",
                    Nullable = false,
                    StoreType = PrimitiveTypeName.TypeInt,
                },
                createTableDbMigrator.ColumnModels.First(), "UserId");
            AssertColumn(new ColumnModel
                {
                    Identity = false,
                    Name = "FirstName",
                    Nullable = false,
                    IsMaxLength = false,
                    MaxLength = 12,
                    StoreType = PrimitiveTypeName.TypeString,
                },
                createTableDbMigrator.ColumnModels.Skip(1).First(), "FirstName");
            AssertColumn(new ColumnModel
                {
                    Identity = false,
                    Name = "Remark",
                    Nullable = true,
                    IsMaxLength = true,
                    MaxLength = null,
                    StoreType = PrimitiveTypeName.TypeString,
                },
                createTableDbMigrator.ColumnModels.Skip(2).First(), "Last");

            // key 
            var addPrimaryKeyDbMigrator = codeGenerator.Migrators.Skip(1).First() as AddPrimaryKeyDbMigrator;
            Assert.IsNotNull(addPrimaryKeyDbMigrator);
            Assert.AreEqual("UserId", addPrimaryKeyDbMigrator.ColumnName);
            Assert.AreEqual("Persons", addPrimaryKeyDbMigrator.TableName);
        }
        [TestMethod]
        public void TestDropTable()
        {
            var migrationFacade = new MigrationFacade();
            MigratorsCodeGenerator codeGenerator = migrationFacade.GetMigratorsCodeGenerator(
                new List<ModelMetadata>
                    {
                        new ModelMetadata
                            {
                                Name = "Persons",
                                Properties =
                                    {
                                        new PropertyMetadata
                                            {
                                                IsKey = true,
                                                Name = "UserId",
                                                TypeName = "Int32"
                                            }, 
                                    }
                            },
                    },
                 new List<ModelMetadata>());
            Assert.AreEqual(1, codeGenerator.Migrators.Count());
            var createTableDbMigrator = codeGenerator.Migrators.First() as DropTableDbMigrator;
            Assert.IsNotNull(createTableDbMigrator);
            Assert.AreEqual("Persons", createTableDbMigrator.TableName);
        }

        [TestMethod]
        public void TestAddColumn()
        {
            var migrationFacade = new MigrationFacade();
            MigratorsCodeGenerator codeGenerator = migrationFacade.GetMigratorsCodeGenerator(
                new List<ModelMetadata>
                    {
                        new ModelMetadata
                            {
                                Name = "Persons",
                                Properties =
                                    {
                                        new PropertyMetadata
                                            {
                                                IsKey = true,
                                                Name = "UserId",
                                                Type = typeof (int)
                                            }, 
                                    }
                            },
                    },
                 new List<ModelMetadata>{
                        new ModelMetadata
                            {
                                Name = "Persons",
                                Properties =
                                    {
                                        new PropertyMetadata
                                            {
                                                IsKey = true,
                                                Name = "UserId",
                                                Type = typeof (int)
                                            }, 
                                        new PropertyMetadata
                                            {
                                                IsKey = false,
                                                Name = "FirstName",
                                                MaxLength = 12,
                                                Type = typeof (string)
                                            }, 
                                    }
                            },
                    });
            Assert.AreEqual(1, codeGenerator.Migrators.Count());
            var addColumnDbMigrator = codeGenerator.Migrators.First() as AddColumnDbMigrator;
            Assert.IsNotNull(addColumnDbMigrator);
            Assert.AreEqual("FirstName", addColumnDbMigrator.ColumnName);
            Assert.AreEqual("Persons", addColumnDbMigrator.TableName);
        }
        [TestMethod]
        public void TestDropColumn()
        {
            var migrationFacade = new MigrationFacade();
            MigratorsCodeGenerator codeGenerator = migrationFacade.GetMigratorsCodeGenerator(
                new List<ModelMetadata>{
                        new ModelMetadata
                            {
                                Name = "Persons",
                                Properties =
                                    {
                                        new PropertyMetadata
                                            {
                                                IsKey = true,
                                                Name = "UserId",
                                                Type = typeof (int)
                                            }, 
                                        new PropertyMetadata
                                            {
                                                IsKey = false,
                                                Name = "FirstName",
                                                MaxLength = 12,
                                                Type = typeof (string)
                                            }, 
                                    }
                            },
                    },
                new List<ModelMetadata>
                    {
                        new ModelMetadata
                            {
                                Name = "Persons",
                                Properties =
                                    {
                                        new PropertyMetadata
                                            {
                                                IsKey = true,
                                                Name = "UserId",
                                                Type = typeof (int)
                                            }, 
                                    }
                            },
                    }
                 );
            Assert.AreEqual(1, codeGenerator.Migrators.Count());
            var addColumnDbMigrator = codeGenerator.Migrators.First() as DropColumnDbMigrator;
            Assert.IsNotNull(addColumnDbMigrator);
            Assert.AreEqual("FirstName", addColumnDbMigrator.ColumnName);
            Assert.AreEqual("Persons", addColumnDbMigrator.TableName);
        }

        [TestMethod]
        public void TestChangePrimaryKeyTable()
        {
            var migrationFacade = new MigrationFacade();
            MigratorsCodeGenerator codeGenerator = migrationFacade.GetMigratorsCodeGenerator(
                new List<ModelMetadata>{
                        new ModelMetadata
                            {
                                Name = "Persons",
                                Properties =
                                    {
                                        new PropertyMetadata
                                            {
                                                IsKey = true,
                                                Name = "UserId",
                                                Type = typeof (int)
                                            },  
                                    }
                            },
                    },
                new List<ModelMetadata>
                    {
                        new ModelMetadata
                            {
                                Name = "Persons",
                                Properties =
                                    {
                                        new PropertyMetadata
                                            {
                                                IsKey = true,
                                                Name = "PersonId",
                                                Type = typeof (int)
                                            }, 
                                    }
                            },
                    }
                 );
            var dbMigrators = codeGenerator.Migrators.ToArray();
            Assert.AreEqual(4, dbMigrators.Count());
            var dropPrimaryKeyDbMigrator = dbMigrators[0] as DropPrimaryKeyDbMigrator;
            Assert.IsNotNull(dropPrimaryKeyDbMigrator);
            Assert.AreEqual("Persons", dropPrimaryKeyDbMigrator.TableName);

            var dropColumnDbMigrator = dbMigrators[1] as DropColumnDbMigrator;
            Assert.IsNotNull(dropColumnDbMigrator);
            Assert.AreEqual("Persons", dropColumnDbMigrator.TableName);
            Assert.AreEqual("UserId", dropColumnDbMigrator.ColumnName);

            var addColumnDbMigrator = dbMigrators[2] as AddColumnDbMigrator;
            Assert.IsNotNull(addColumnDbMigrator);
            Assert.AreEqual("Persons", addColumnDbMigrator.TableName);
            Assert.AreEqual("PersonId", addColumnDbMigrator.ColumnName);

            var addPrimaryKeyDbMigrator = dbMigrators[3] as AddPrimaryKeyDbMigrator;
            Assert.IsNotNull(addPrimaryKeyDbMigrator);
            Assert.AreEqual("Persons", addPrimaryKeyDbMigrator.TableName);
            Assert.AreEqual("PersonId", addPrimaryKeyDbMigrator.ColumnName);
        }

        [TestMethod]
        public void Test_AlterColumn()
        {
            var migrationFacade = new MigrationFacade();
            MigratorsCodeGenerator codeGenerator = migrationFacade.GetMigratorsCodeGenerator(
                new List<ModelMetadata>{
                        new ModelMetadata
                            {
                                Name = "Persons",
                                Properties =
                                    {
                                        new PropertyMetadata
                                            {
                                                IsKey = true,
                                                Name = "UserId",
                                                Type = typeof (int)
                                            },  
                                    }
                            },
                    },
                new List<ModelMetadata>
                    {
                        new ModelMetadata
                            {
                                Name = "Persons",
                                Properties =
                                    {
                                        new PropertyMetadata
                                            {
                                                IsKey = false,
                                                Name = "UserId",
                                                Type = typeof (int?)
                                            }, 
                                    }
                            },
                    }
                 );
            var dbMigrators = codeGenerator.Migrators.ToArray();
            Assert.AreEqual(2, dbMigrators.Count());
            var alterColumnDbMigrator = dbMigrators[0] as AlterColumnDbMigrator;
            Assert.IsNotNull(alterColumnDbMigrator);
            Assert.AreEqual("UserId", alterColumnDbMigrator.ColumnName);
            AssertColumn(new ColumnModel
            {
                Identity = false,
                Name = "UserId",
                Nullable = false,
                StoreType = PrimitiveTypeName.TypeInt
            }, alterColumnDbMigrator.Column, "UserId");

            var dropPrimaryKeyDbMigrator = dbMigrators[1] as DropPrimaryKeyDbMigrator;
            Assert.IsNotNull(dropPrimaryKeyDbMigrator);
            Assert.AreEqual("Persons", dropPrimaryKeyDbMigrator.TableName);
        }

        private void AssertColumn(ColumnModel expected, ColumnModel actual, string name)
        {
            Assert.AreEqual(expected.FixedLength, actual.FixedLength, name + "'s FixedLength Error");
            Assert.AreEqual(expected.Identity, actual.Identity, name + "'s Identity Error");
            Assert.AreEqual(expected.IsMaxLength, actual.IsMaxLength, name + "'s IsMaxLength Error");
            Assert.AreEqual(expected.MaxLength, actual.MaxLength, name + "'s MaxLength Error");
            Assert.AreEqual(expected.Name, actual.Name, name + "'s Name Error");
            Assert.AreEqual(expected.Nullable, actual.Nullable, name + "'s Nullable Error");
            Assert.AreEqual(expected.Precision, actual.Precision, name + "'s Precision Error");
            Assert.AreEqual(expected.StoreType, actual.StoreType, name + "'s StoreType Error");
        }

    }
}