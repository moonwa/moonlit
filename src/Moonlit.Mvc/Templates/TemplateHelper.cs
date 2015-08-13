using System;
using System.Collections.Generic;
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

    public class FieldsBuilder
    {
        List<Field> _fields = new List<Field>();
        public FieldsBuilder()
        {

        }

        public FieldsBuilder ReadOnly(string names)
        {
            var nameArray = names.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            return ReadOnly(nameArray);
        }

        public FieldsBuilder ReadOnly(string[] names)
        {
            foreach (var field in _fields)
            {
                if (names.Any(x => string.Equals(field.FieldName, x, StringComparison.OrdinalIgnoreCase)))
                {
                    var enabledControl = field.Control as IEnabledControl;
                    if (enabledControl != null)
                    {
                        enabledControl.Enabled = false;
                    }
                }
            }
            return this;
        }

        public FieldsBuilder ForEntity<T>(T model, ControllerContext controllerContext)
        {
            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForProperties(model, typeof(T));
            var props = modelMetadata.Where(x => ModelMetadataHelper.GetField(x) != null && ModelMetadataHelper.GetControlBuilder(x) != null).ToList();
            props = props.OrderBy(x => x.GetSort()).ToList();
            var fields = props.Select(x =>
            {
                var field = x.GetField();
                field.Control = x.GetControlBuilder().CreateControl(x, x.Model, controllerContext);
                field.Name = x.PropertyName;
                return field;
            }).ToArray();
            _fields.AddRange(fields);
            return this;
        }

        public Field[] Build()
        {
            return _fields.ToArray();
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

        public TableBuilder<T> Add(Func<RowBoundItem<T>, Control> cellTemplate, string header)
        {
            _table.Columns.Add(new TableColumn()
            {
                Header = header,
                CellTemplate = (x) => cellTemplate(new RowBoundItem<T>
                {
                    Target = (T)x.Target,
                }),
            });
            return this;
        }
    }
}