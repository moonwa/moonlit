using System.Collections.Generic;
using System.Linq;
using AssemblyV1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.Data.Design.Entity;
using Moonlit.Data.Migrations;

namespace Moonlit.Data.Fixtures.Design.Entity
{
    [TestClass]
    public class ModelMetadataParserFixture
    {
        [TestMethod]
        public void TestMethod1()
        {
            var modelMetadatas = ModelMetadataParser.GetModelMetadatas(typeof(DbContextV1));
            Assert.AreEqual(3, modelMetadatas.Count);

            var personMetadata = modelMetadatas.FirstOrDefault(x => x.Name == "Persons");
            Assert.IsNotNull(personMetadata);
            Assert.AreEqual(9, personMetadata.Properties.Count);
            AssetProperty(personMetadata.Properties, "Person", "PersonId", new PropertyMetadata
                {
                    MaxLength = 0,
                    Name = "PersonId",
                    IsKey = true,
                    IsNullable = false,
                    TypeName = PrimitiveTypeName.TypeInt,
                });
            AssetProperty(personMetadata.Properties, "Person", "StringNullableField", new PropertyMetadata
                {
                    MaxLength = 50,
                    Name = "StringNullableField",
                    IsKey = false,
                    IsNullable = true,
                    TypeName = PrimitiveTypeName.TypeString,
                });

            AssetProperty(personMetadata.Properties, "Person", "StringNotNullableField", new PropertyMetadata
                {
                    MaxLength = 30,
                    Name = "StringNotNullableField",
                    IsKey = false,
                    IsNullable = false,
                    TypeName = PrimitiveTypeName.TypeString,
                });
            AssetProperty(personMetadata.Properties, "Person", "IntNullableField", new PropertyMetadata
                {
                    Name = "IntNullableField",
                    IsKey = false,
                    IsNullable = true,
                    TypeName = PrimitiveTypeName.TypeInt,
                });

            AssetProperty(personMetadata.Properties, "Person", "IntNotNullableField", new PropertyMetadata
                {
                    Name = "IntNotNullableField",
                    IsKey = false,
                    IsNullable = false,
                    TypeName = PrimitiveTypeName.TypeInt,
                });
            AssetProperty(personMetadata.Properties, "Person", "DecimalNullableField", new PropertyMetadata
                {
                    Name = "DecimalNullableField",
                    IsKey = false,
                    IsNullable = true,
                    TypeName = PrimitiveTypeName.TypeDecimal,
                });

            AssetProperty(personMetadata.Properties, "Person", "DecimalNotNullableField", new PropertyMetadata
                {
                    Name = "DecimalNotNullableField",
                    IsKey = false,
                    IsNullable = false,
                    TypeName = PrimitiveTypeName.TypeDecimal,
                });
            AssetProperty(personMetadata.Properties, "Person", "BooleanNullableField", new PropertyMetadata
                {
                    Name = "BooleanNullableField",
                    IsKey = false,
                    IsNullable = true,
                    TypeName = PrimitiveTypeName.TypeBoolean,
                });

            AssetProperty(personMetadata.Properties, "Person", "BooleanNotNullableField", new PropertyMetadata
                {
                    Name = "BooleanNotNullableField",
                    IsKey = false,
                    IsNullable = false,
                    TypeName = PrimitiveTypeName.TypeBoolean,
                });


            var catMetadata = modelMetadatas.FirstOrDefault(x => x.Name == "Cats");
            Assert.IsNotNull(catMetadata);
            Assert.AreEqual(2, catMetadata.Properties.Count);
            AssetProperty(catMetadata.Properties, "Cat", "CatId", new PropertyMetadata
                {
                    MaxLength = 0,
                    Name = "CatId",
                    IsKey = true,
                    IsNullable = false,
                    Type = typeof(int),
                });
            AssetProperty(catMetadata.Properties, "Cat", "Name", new PropertyMetadata
                {
                    MaxLength = 50,
                    Name = "Name",
                    IsKey = false,
                    IsNullable = true,
                    Type = typeof(string),
                });
        }
        [TestMethod]
        public void EnumTest()
        {
            var modelMetadatas = ModelMetadataParser.GetModelMetadatas(typeof(DbContextV1));
            Assert.AreEqual(3, modelMetadatas.Count);

            var personMetadata = modelMetadatas.FirstOrDefault(x => x.Name == "Persons");
            Assert.IsNotNull(personMetadata);
            Assert.AreEqual(9, personMetadata.Properties.Count);
            AssetProperty(personMetadata.Properties, "Person", "PersonId", new PropertyMetadata
                {
                    MaxLength = 0,
                    Name = "PersonId",
                    IsKey = true,
                    IsNullable = false,
                    TypeName = PrimitiveTypeName.TypeInt,
                });
            AssetProperty(personMetadata.Properties, "Person", "StringNullableField", new PropertyMetadata
                {
                    MaxLength = 50,
                    Name = "StringNullableField",
                    IsKey = false,
                    IsNullable = true,
                    TypeName = PrimitiveTypeName.TypeString,
                });

            AssetProperty(personMetadata.Properties, "Person", "StringNotNullableField", new PropertyMetadata
                {
                    MaxLength = 30,
                    Name = "StringNotNullableField",
                    IsKey = false,
                    IsNullable = false,
                    TypeName = PrimitiveTypeName.TypeString,
                });
            AssetProperty(personMetadata.Properties, "Person", "IntNullableField", new PropertyMetadata
                {
                    Name = "IntNullableField",
                    IsKey = false,
                    IsNullable = true,
                    TypeName = PrimitiveTypeName.TypeInt,
                });

            AssetProperty(personMetadata.Properties, "Person", "IntNotNullableField", new PropertyMetadata
                {
                    Name = "IntNotNullableField",
                    IsKey = false,
                    IsNullable = false,
                    TypeName = PrimitiveTypeName.TypeInt,
                });
            AssetProperty(personMetadata.Properties, "Person", "DecimalNullableField", new PropertyMetadata
                {
                    Name = "DecimalNullableField",
                    IsKey = false,
                    IsNullable = true,
                    TypeName = PrimitiveTypeName.TypeDecimal,
                });

            AssetProperty(personMetadata.Properties, "Person", "DecimalNotNullableField", new PropertyMetadata
                {
                    Name = "DecimalNotNullableField",
                    IsKey = false,
                    IsNullable = false,
                    TypeName = PrimitiveTypeName.TypeDecimal,
                });
            AssetProperty(personMetadata.Properties, "Person", "BooleanNullableField", new PropertyMetadata
                {
                    Name = "BooleanNullableField",
                    IsKey = false,
                    IsNullable = true,
                    TypeName = PrimitiveTypeName.TypeBoolean,
                });

            AssetProperty(personMetadata.Properties, "Person", "BooleanNotNullableField", new PropertyMetadata
                {
                    Name = "BooleanNotNullableField",
                    IsKey = false,
                    IsNullable = false,
                    TypeName = PrimitiveTypeName.TypeBoolean,
                });


            var catMetadata = modelMetadatas.FirstOrDefault(x => x.Name == "Cats");
            Assert.IsNotNull(catMetadata);
            Assert.AreEqual(2, catMetadata.Properties.Count);
            AssetProperty(catMetadata.Properties, "Cat", "CatId", new PropertyMetadata
                {
                    MaxLength = 0,
                    Name = "CatId",
                    IsKey = true,
                    IsNullable = false,
                    Type = typeof(int),
                });
            AssetProperty(catMetadata.Properties, "Cat", "Name", new PropertyMetadata
                {
                    MaxLength = 50,
                    Name = "Name",
                    IsKey = false,
                    IsNullable = true,
                    Type = typeof(string),
                });
        }


        private void AssetProperty(List<PropertyMetadata> properties, string modelName, string name, PropertyMetadata columnModel)
        {
            PropertyMetadata property = properties.FirstOrDefault(x => x.Name.EqualsIgnoreCase(name));
            Assert.IsNotNull(property, name + " of " + modelName + "'s Property is null");

            Assert.AreEqual(columnModel.IsKey, property.IsKey, name + " of " + modelName + "'s Property IsKey Error");
            Assert.AreEqual(columnModel.IsNullable, property.IsNullable, name + " of " + modelName + "'s Property IsNullable Error");
            Assert.AreEqual(columnModel.MaxLength, property.MaxLength, name + " of " + modelName + "'s Property MaxLength Error");
            Assert.AreEqual(columnModel.Name, property.Name, name + " of " + modelName + "'s Property Name Error");
            Assert.AreEqual(property.TypeName, columnModel.TypeName, name + " of " + modelName + "'s Property TypeName Error");
        }
    }
}
