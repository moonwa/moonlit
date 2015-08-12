using System.Collections;
using System.Web.Mvc;

namespace Moonlit.Mvc.Controls
{
    public class Table : Control
    {
        public TableColumn[] Columns { get; set; }
        public IEnumerable DataSource { get; set; }
    }
}