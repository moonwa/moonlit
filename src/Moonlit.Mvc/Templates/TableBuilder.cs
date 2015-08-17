using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;

namespace Moonlit.Mvc.Templates
{
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
            foreach (var propertyMetadata in metadata.Properties.Where(x => ModelMetadataHelper.GetCellTemplateBuilder(x) != null))
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

        public TableBuilder<T> Add(TableColumn column)
        {
            _table.Columns.Add(column);
            return this;
        }
  
        public TableBuilder<T> Add(Func<RowBoundItem<T>, Control> cellTemplate, string header, string sort = null)
        {
            var tableColumn = new TableColumn()
            {
                Header = header,
                Sort = sort,
                CellTemplate = (x) => cellTemplate(new RowBoundItem<T>
                {
                    Target = (T)x.Target,
                }),
            };

            return this.Add(tableColumn);
        }

        public Func<RowBoundItem<T>, Control> CheckBox<TResult>(Func<T, TResult> func, ControllerContext controllerContext, string name = null)
        {
            return x =>
            { 
                return new CheckBox
                {
                    Value = func(x.Target).IfNotNull(a => a.ToString()),
                    Name = name ?? ""
                };
            };
        }
 
        public Func<RowBoundItem<T>, Control> Literal<TResult>(Func<T, TResult> func, ControllerContext controllerContext)
        {
            return x =>
            {
                return new Literal()
                {
                    Text = func(x.Target).IfNotNull(a => a.ToString())
                };
            };
        }
    }
}