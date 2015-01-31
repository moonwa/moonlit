using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Moonlit.Data.Design.Entity
{
    internal class ModelMetadataParser
    {
        internal static List<ModelMetadata> GetModelMetadatas(Type contextType)
        {
            var dbsetProperties = contextType.GetProperties()
                .Where(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(System.Data.Entity.DbSet<>)).ToList();

            List<ModelMetadata> newModelMetadatas = new List<ModelMetadata>();
            foreach (var dbsetProperty in dbsetProperties)
            {
                var entityType = dbsetProperty.PropertyType.GetGenericArguments()[0];
                var modelMetadata = GetModelMetadata(dbsetProperty.Name, entityType);
                newModelMetadatas.Add(modelMetadata);
            }
            return newModelMetadatas;
        }

        internal static ModelMetadata GetModelMetadata(string name, Type entityType)
        {
            var modelMetadata = new ModelMetadata();
            modelMetadata.Name = name;
            modelMetadata.TypeName = entityType.Name;
            foreach (var propertyInfo in entityType.GetProperties())
            {
                PropertyMetadata propertyMetadata = new PropertyMetadata
                                                        {
                                                            Name = propertyInfo.Name,
                                                            Type = propertyInfo.PropertyType,
                                                            IsNullable = IsNullable(propertyInfo),
                                                            IsKey = IsKey(propertyInfo),
                                                            MaxLength = MaxLength(propertyInfo),
                                                        };
                modelMetadata.Properties.Add(propertyMetadata);
            }
            return modelMetadata;
        }

        private static int MaxLength(PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType != typeof(string))
                return 0;

            var stringLength = propertyInfo.GetCustomAttributes(false).OfType<StringLengthAttribute>().FirstOrDefault();
            if (stringLength == null)
                return 50;
            return stringLength.MaximumLength;
        }

        private static bool IsKey(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes(false).OfType<KeyAttribute>().Any();
        }

        private static bool IsNullable(PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType == typeof(int) ||
                propertyInfo.PropertyType == typeof(long) ||
                propertyInfo.PropertyType == typeof(DateTime) ||
                propertyInfo.PropertyType == typeof(decimal) ||
                propertyInfo.PropertyType == typeof(double) ||
                propertyInfo.PropertyType == typeof(float) ||
                propertyInfo.PropertyType == typeof(bool))
                return false;
            if (propertyInfo.GetCustomAttributes(false).Any(x => typeof(RequiredAttribute).IsAssignableFrom(x.GetType())))
                return false;
            return true;
        }
    }
}