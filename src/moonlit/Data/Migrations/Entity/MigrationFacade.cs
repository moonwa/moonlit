using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using Moonlit.Data.Migrations;
using Moonlit.Data.Migrations.Migrators;
using Moonlit.Runtime.Serialization;

namespace Moonlit.Data.Design.Entity
{
    public class MigrationFacade
    {
        public MigratorsCodeGenerator GetMigratorsCodeGenerator(string target, Type contextType)
        {
            List<ModelMetadata> modelMetadatas = StringSerializer.DeserializeAsJson<List<ModelMetadata>>(target);
            var newModelMetadatas = ModelMetadataParser.GetModelMetadatas(contextType);
            return GetMigratorsCodeGenerator(modelMetadatas, newModelMetadatas);
        }

        public MigratorsCodeGenerator GetMigratorsCodeGenerator(Assembly assembly)
        {
            var types = assembly.GetTypes();
            var contextType = types.FirstOrDefault(x => typeof(DbContext).IsAssignableFrom(x));
            if (contextType == null)
            {
                return new EmptyCodeGenerator();
            }

            var migration = types.Where(x => typeof(DbMigration).IsAssignableFrom(x))
                .Select(x => (DbMigration)Activator.CreateInstance(x)).OrderBy(x => x.Version)
                .LastOrDefault();

            if (migration == null)
            {
                return GetMigratorsCodeGenerator("{}", contextType);
            }
            else
            {
                return GetMigratorsCodeGenerator(migration.Target, contextType);
            }
        }
        internal MigratorsCodeGenerator GetMigratorsCodeGenerator(List<ModelMetadata> modelMetadatas, List<ModelMetadata> newModelMetadatas)
        {
            List<DbMigrator> migrators = new List<DbMigrator>();
            foreach (var newModelMetadata in newModelMetadatas)
            {
                var modelMetadata = modelMetadatas.FirstOrDefault(x => x.Name.EqualsIgnoreCase(newModelMetadata.Name));
                if (modelMetadata == null)
                {
                    migrators.AddRange(NewTable(newModelMetadata));
                }
                else
                {
                    migrators.AddRange(MergeTable(newModelMetadata, modelMetadata));
                    modelMetadatas.Remove(modelMetadata);
                }
            }
            foreach (var modelMetadata in modelMetadatas)
            {
                migrators.Add(new DropTableDbMigrator(modelMetadata.Name));
            }
            if (migrators.Count == 0)
                return new EmptyCodeGenerator();
            return new CSharpMigratorsCodeGenerator(migrators, newModelMetadatas.SerializeAsJson());
        }
        private class ModelMetadataNameEqualityComparer : IEqualityComparer<PropertyMetadata>
        {
            public bool Equals(PropertyMetadata x, PropertyMetadata y)
            {
                return x.Name.EqualsIgnoreCase(y.Name);
            }

            public int GetHashCode(PropertyMetadata obj)
            {
                return obj.Name.GetHashCode();
            }
        }
        private IEnumerable<DbMigrator> MergeTable(ModelMetadata newModelMetadata, ModelMetadata modelMetadata)
        {
            foreach (var property in modelMetadata.Properties.Except(newModelMetadata.Properties, new ModelMetadataNameEqualityComparer()).ToList())
            {
                if (property.IsKey)
                {
                    yield return new DropPrimaryKeyDbMigrator(modelMetadata.Name);
                }
                yield return new DropColumnDbMigrator(modelMetadata.Name, property.Name);

                modelMetadata.Properties.Remove(property);
            }
            foreach (var newProperty in newModelMetadata.Properties)
            {
                var property = modelMetadata.Properties.FirstOrDefault(x => x.Name.EqualsIgnoreCase(newProperty.Name));
                if (property == null)
                {
                    yield return new AddColumnDbMigrator(modelMetadata.Name, newProperty.Name, CreateColumnModel(newProperty));
                    if (newProperty.IsKey)
                        yield return new AddPrimaryKeyDbMigrator(modelMetadata.Name, newProperty.Name);
                }
                else
                {
                    if (!Equals(property, newProperty))
                    {
                        yield return new AlterColumnDbMigrator(modelMetadata.Name, property.Name, CreateColumnModel(newProperty));
                    }
                    if (property.IsKey != newProperty.IsKey)
                    {
                        yield return new DropPrimaryKeyDbMigrator(modelMetadata.Name);
                        if (newProperty.IsKey)
                        {
                            yield return new AddColumnDbMigrator(modelMetadata.Name, newProperty.Name, CreateColumnModel(newProperty));
                        }
                    }
                    modelMetadata.Properties.Remove(property);
                }
            }
        }

        private IEnumerable<DbMigrator> NewTable(ModelMetadata newModelMetadata)
        {
            var properties = from x in newModelMetadata.Properties
                             select CreateColumnModel(x);
            yield return new CreateTableDbMigrator(newModelMetadata.Name, properties.ToList());
            var keyProperty = newModelMetadata.Properties.FirstOrDefault(x => x.IsKey);
            if (keyProperty != null) yield return new AddPrimaryKeyDbMigrator(newModelMetadata.Name, keyProperty.Name);
        }

        private ColumnModel CreateColumnModel(PropertyMetadata x)
        {
            return new ColumnModel
                {
                    Identity = x.IsKey,
                    Nullable = x.IsNullable,
                    Name = x.Name,
                    IsMaxLength = x.TypeName == PrimitiveTypeName.TypeString ? x.MaxLength == int.MaxValue : (bool?)null,
                    MaxLength = x.TypeName == PrimitiveTypeName.TypeString ? (x.MaxLength == int.MaxValue ? (int?)null : x.MaxLength) : null,
                    StoreType = x.TypeName,
                    Precision = GetPrecision(x)
                };
        }

        private byte? GetPrecision(PropertyMetadata propertyMetadata)
        {
            var storeType = propertyMetadata.TypeName;
            if (storeType == "decimal")
                return 2;
            return null;
        }
    }
}
