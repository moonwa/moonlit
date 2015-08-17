using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;

namespace Moonlit.Mvc.Templates
{
    public class FieldsBuilder
    {
        static List<ControlBuilderCriteria> _controlBuilderCriterias = new List<ControlBuilderCriteria>();
        List<Field> _fields = new List<Field>();
        public FieldsBuilder()
        {
            _controlBuilderCriterias.Add(new ControlBuilderCriteria(x => x.ModelType == typeof(bool) || x.ModelType == typeof(bool?), new CheckBoxAttribute()));
            _controlBuilderCriterias.Add(new ControlBuilderCriteria(x => x.ModelType.ToWithoutNullableType().IsEnum, new SelectListAttribute(typeof(EnumSelectListProvider))));
            _controlBuilderCriterias.Add(new ControlBuilderCriteria(x => (x.ModelType == typeof(DateTime?) || x.ModelType == typeof(DateTime)), new DatePickerAttribute()));
            _controlBuilderCriterias.Add(new ControlBuilderCriteria(x => true, new TextBoxAttribute()));
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
            var props = from x in modelMetadata
                        let field = ModelMetadataHelper.GetField(x)
                        let controlBuilder = GetControlBuilder(x)
                        where field != null && controlBuilder != null
                        orderby x.GetSort()
                        select new
                        {
                            ModelMetadata = x,
                            Field = field,
                            ControlBuilder = controlBuilder,
                        };
            var fields = props.Select(x =>
            {
                x.Field.Control = x.ControlBuilder.CreateControl(x.ModelMetadata, x.ModelMetadata.Model, controllerContext);
                x.Field.Name = x.ModelMetadata.PropertyName;
                return x.Field;
            }).ToArray();
            _fields.AddRange(fields);
            return this;
        }

        public static IControllBuilder GetControlBuilder(ModelMetadata modelMetadata)
        {
            return ModelMetadataHelper.GetControlBuilder(modelMetadata) ?? GetDefaultControlBuilder(modelMetadata);
        }

        private static IControllBuilder GetDefaultControlBuilder(ModelMetadata modelMetadata)
        {
            foreach (var controlBuilderCriteria in _controlBuilderCriterias)
            {
                if (controlBuilderCriteria.IsSupport(modelMetadata))
                {
                    return controlBuilderCriteria.ControllBuilder;
                }
            }
            return null;
        }

        public Field[] Build()
        {
            return _fields.ToArray();
        }
    }
}