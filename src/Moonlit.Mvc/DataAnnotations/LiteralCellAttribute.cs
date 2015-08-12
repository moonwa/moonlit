using System;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;

namespace Moonlit.Mvc
{
    public class LiteralCellAttribute : CellTemplateBuilderAttribute
    {
        #region Overrides of CellTemplateBuilderAttribute

        public override Func<RowBoundItem, Control> CreateCellTemplate(ModelMetadata propertyMetadata, ControllerContext controllerContext)
        {
            return item =>
            {
                var propertyValue = EntityAccessor.GetAccessor(propertyMetadata.ContainerType).GetProperty(item.Target, propertyMetadata.PropertyName);
                var text = propertyValue == null ? "" : propertyValue.ToString();
                return new Literal
                {
                    Text = text,
                };
            };
        }

        #endregion
    }
}