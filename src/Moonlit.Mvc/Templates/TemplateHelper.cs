using System.Linq;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;

namespace Moonlit.Mvc.Templates
{
    public static class TemplateHelper
    {
        public static Field[] MakeFields(object model, ControllerContext controllerContext)
        {
            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, model.GetType());
            var props = modelMetadata.Properties.Where(x => ModelMetadataHelper.GetField(x) != null && ModelMetadataHelper.GetControlBuilder(x) != null).OrderBy(x => x.GetSort()).ToList();
            return props.Select(x =>
            {
                var field = x.GetField();
                field.Control = x.GetControlBuilder().CreateControl(x, x.Model, controllerContext);
                field.Name = x.PropertyName;
                return field;
            }).ToArray();
        }
    }

    public class TableBuilder<T>
    {
        private Table _table;
        public TableBuilder()
        {
            _table = new Table();
        }

        public TableBuilder<T> ForEntity(ControllerContext controllerContext)
        {
            var metadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(T));
            foreach (var propertyMetadata in metadata.Properties.Where(x => x.GetCellTemplateBuilder() != null))
            {
                _table.Columns.Add(new TableColumn()
                {
                    Sort = propertyMetadata.GetSort(),
                    Header = propertyMetadata.GetDisplayName(),
                    CellTemplate = propertyMetadata.GetCellTemplateBuilder().CreateCellTemplate(propertyMetadata, controllerContext),
                });
            }
            return this;
        }

        public Table Build()
        {
            return _table;
        }
    }
}