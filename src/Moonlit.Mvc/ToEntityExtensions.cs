using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public static class ToEntityExtensions
    {
        public static void FromEntity<TModel, TEntity>(this TModel model, TEntity entity, FromEntityContext context)
        {
            var modelAccessor = EntityAccessor.GetAccessor(typeof(TModel));
            var entityAccessor = EntityAccessor.GetAccessor(typeof(TEntity));

            var properties = ModelMetadataProviders.Current.GetMetadataForProperties(model, typeof(TModel));
            foreach (var propertyMatadata in properties)
            {
                var mappingTo = propertyMatadata.GetMapping();
                if (mappingTo != null)
                {
                    if (!mappingTo.OnlyNotPostback || !context.IsPostback || propertyMatadata.IsReadOnly)
                    {
                        if (entityAccessor.HasPropertyGetter(mappingTo.To) && modelAccessor.HasPropertySetter(propertyMatadata.PropertyName))
                        {
                            var value = entityAccessor.GetProperty(entity, mappingTo.To ?? propertyMatadata.PropertyName);
                            modelAccessor.SetProperty(model, propertyMatadata.PropertyName, value);
                        }
                    }
                }
            }
            IFromEntity<TEntity> to = model as IFromEntity<TEntity>;
            if (to != null)
            {
                to.OnFromEntity(entity, context);
            }
        }
        public static void ToEntity<TModel, TEntity>(this TModel model, TEntity entity, ToEntityContext context)
        {
            var modelAccessor = EntityAccessor.GetAccessor(typeof(TModel));
            var entityAccessor = EntityAccessor.GetAccessor(typeof(TEntity));

            var properties = ModelMetadataProviders.Current.GetMetadataForProperties(model, typeof(TModel));
            foreach (var propertyMatadata in properties)
            {
                if (propertyMatadata.IsReadOnly)
                {
                    continue;
                }
                var mappingTo = propertyMatadata.GetMapping();
                if (mappingTo != null)
                {
                    var propertyName = mappingTo.To ?? propertyMatadata.PropertyName;
                    if (entityAccessor.HasPropertySetter(propertyName))
                    {
                        entityAccessor.SetProperty(entity, propertyName, propertyMatadata.Model);
                    }
                }
            }
            IToEntity<TEntity> to = model as IToEntity<TEntity>;
            if (to != null)
            {
                to.OnToEntity(entity, context);
            }
        }
    }
}