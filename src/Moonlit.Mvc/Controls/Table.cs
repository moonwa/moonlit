using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Moonlit.Mvc.Controls
{
    public class Table : Control
    {
        public Table()
        {
            Columns = new List<TableColumn>();
        }
        public List<TableColumn> Columns { get; set; }
        public IEnumerable DataSource { get; set; }
    }
}