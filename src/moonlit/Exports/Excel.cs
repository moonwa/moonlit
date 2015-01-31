using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Moonlit.Exports
{
    public class Font
    {
        public string FontName { get; set; }
        public string Family { get; set; }
        public int? Size { get; set; }
        public Color? Color { get; set; }
        public bool? Bold { get; set; }
    }

    public class Borders
    {
        public Border Top { get; set; }
        public Border Left { get; set; }
        public Border Right { get; set; }
        public Border Bottom { get; set; }
    }

    public class Border
    {
        public LineStyle LineStyle { get; set; }
        public int Weight { get; set; }
    }

    public enum LineStyle
    {
        Solid
    }

    public class GridCellCollection : List<GridCell>
    {
    }

    public class Style
    {
        public Font Font { get; set; }
        public NumberFormat NumberFormat { get; set; }
        public HorizontalAlignment? HorizontalAlignment { get; set; }
        public VerticalAlignment? VerticalAlignment { get; set; }
        public Color? BackgroundColor { get; set; }
        public bool? WrapText { get; set; }
        public Borders Borders { get; set; }
    }

    public enum GridFormat
    {
        Number,
        String,
        Html
    }

    public class GridCell
    {
        private object _text;

        internal GridCell()
            : this("")
        {
        }

        internal GridCell(string text)
        {
            _text = text;
        }

        public Style Style { get; set; }

        public object Text
        {
            get { return _text; }
            set
            {
                _text = value;
                if (value is decimal || value is double || value is int || value is long || value is short
                    || value is decimal? || value is double? || value is int? || value is long? || value is short?
                    )
                    Format = GridFormat.Number;
            }
        }


        public GridFormat Format { get; set; }

        public int? ColSpan { get; set; }
    }

    public class GridRow
    {
        internal GridRow()
        {
            Cells = new GridCellCollection();
        }

        public GridCellCollection Cells { get; private set; }
        public int? Height { get; set; }

        public GridCell this[GridColumn col]
        {
            get { return Cells[col.DisplayIndex]; }
        }

        public GridCell this[int index]
        {
            get { return Cells[index]; }
        }
    }

    public class GridColumn
    {
        public GridColumn(string title, int width)
        {
            Title = title;
            Width = width;
            Format = GridFormat.String;
            Header = new GridHeader(title);
        }

        public GridHeader Header { get; private set; }
        public int Width { get; set; }
        public string Title { get; set; }
        public int DisplayIndex { get; internal set; }
        protected GridFormat Format { get; set; }

        internal GridCell CreateCell()
        {
            var cell = new GridCell();
            cell.Format = Format;
            return cell;
        }
    }

    public enum HorizontalAlignment
    {
        Left = 0,
        Center = 1,
        Right = 2
    };

    public enum VerticalAlignment
    {
        Top = 0,
        Middle = 1,
        Bottom = 2
    }

    public class GridColumnCollection : Collection<GridColumn>
    {
        protected override void InsertItem(int index, GridColumn item)
        {
            base.InsertItem(index, item);

            int displayIndex = 0;

            foreach (GridColumn gridColumn in Items)
            {
                gridColumn.DisplayIndex = displayIndex++;
            }
        }
    }


    public class GridHeader : GridCell
    {
        public GridHeader(string text)
            : base(text)
        {
            Format = GridFormat.String;
        }
    }

    public class GridRowCollection : List<GridRow>
    {
    }

    public class Grid
    {
        private Style _headerStyle;

        public Grid()
        {
            Columns = new GridColumnCollection();

            Rows = new GridRowCollection();

            DisplayHeader = true;
        }

        public GridOption Options { get; set; }
        public bool DisplayHeader { get; set; }
        public string Title { get; set; }
        public GridColumnCollection Columns { get; private set; }
        public GridRowCollection Rows { get; private set; }

        public Style HeaderStyle
        {
            get { return _headerStyle; }

            set
            {
                foreach (GridColumn column in Columns)
                {
                    if (column.Header.Style == null)
                        column.Header.Style = value;
                }
                _headerStyle = value;
            }
        }

        public GridRow NewRow()
        {
            var row = new GridRow();

            foreach (GridColumn column in Columns)
            {
                row.Cells.Add(column.CreateCell());
            }

            return row;
        }
    }


    public class ExcelExport
    {
        private static readonly XNamespace NsWb = "urn:schemas-microsoft-com:office:spreadsheet";
        private static readonly XNamespace NsOffice = "urn:schemas-microsoft-com:office:office";
        private static readonly XNamespace NsExcel = "urn:schemas-microsoft-com:office:excel";
        private static readonly XNamespace NsSpreadsheet = "urn:schemas-microsoft-com:office:spreadsheet";
        private static readonly XNamespace NsHtml = "http://www.w3.org/TR/REC-html40";

        public Byte[] Export(params Grid[] grids)
        {
            XDocument doc = CreateXDocument();
            XElement root = doc.Root;
            Dictionary<Style, string> styleMaps = GetStyles(grids);
            root.Add(GetStyles(styleMaps));

            int index = 1;
            foreach (Grid grid in grids)
            {
                root.Add(RenderSheet(grid, index++, styleMaps));
            }

            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                doc.Declaration = new XDeclaration("1.0", "utf-8", null);
                string s = doc.ToString();
                s = s.Replace("<![CDATA[", "").Replace("]]>", "");
                writer.Write(doc.Declaration.ToString());
                writer.Write(s);
                writer.Flush();
                return stream.ToArray();
            }
        }


        private XElement RenderSheet(Grid grid, int index, Dictionary<Style, string> styleMaps)
        {
            string sheetName = string.IsNullOrEmpty(grid.Title) ? "sheet " + index : grid.Title;
            var worksheet = new XElement(NsWb + "Worksheet", new XAttribute(NsSpreadsheet + "Name", sheetName));

            var table = new XElement(NsWb + "Table"
                                     , new XAttribute(NsSpreadsheet + "ExpandedColumnCount", grid.Columns.Count)
                                     ,
                                     new XAttribute(NsSpreadsheet + "ExpandedRowCount",
                                                    grid.DisplayHeader ? grid.Rows.Count + 1 : grid.Rows.Count)
                                     , new XAttribute(NsExcel + "FullColumns", GetAttributeValue(true))
                                     , new XAttribute(NsExcel + "FullRows", GetAttributeValue(true))
                );

            table.Add(RenderColumns(grid));

            if (grid.DisplayHeader)
                table.Add(RenderHeaderRows(grid, null, styleMaps));

            table.Add(RenderRows(grid, styleMaps));
            worksheet.Add(table);


            if (grid.Options != null)
            {
                worksheet.Add(RenderOptions(grid));
            }

            return worksheet;
        }

        private XElement RenderOptions(Grid grid)
        {
            var element = new XElement(NsExcel + "WorksheetOptions");

            GridOption options = grid.Options;
            if (options != null)
            {
                if (options.PageSetup != null)
                {
                    element.Add(RenderPageSetup(options.PageSetup));
                }
            }

            return element;
        }

        private XElement RenderPageSetup(PageSetup pageSetup)
        {
            var element = new XElement(NsExcel + "PageSetup");
            PageHeader pageHeader = pageSetup.Header;
            var xlayout = new XElement(NsExcel + "Layout");
            xlayout.SetAttributeValue(NsExcel + "Orientation", pageSetup.Layout.ToString());
            element.Add(xlayout);

            if (pageHeader != null)
            {
                var headerElement = new XElement(NsExcel + "Header");

                if (pageHeader.Margin != null)
                    headerElement.SetAttributeValue(NsExcel + "Margin", Format(pageHeader.Margin));

                if (!string.IsNullOrEmpty(pageHeader.Text))
                    headerElement.SetAttributeValue(NsExcel + "Data",
                                                    "&" + pageHeader.HorizontalAlignment.ToString()[0] +
                                                    Trim(ParseString(pageHeader.Text)));

                element.Add(headerElement);
            }

            PageFooter pageFooter = pageSetup.Footer;

            if (pageFooter != null)
            {
                var footerElement = new XElement(NsExcel + "Footer");

                if (pageFooter.Margin != null)
                    footerElement.SetAttributeValue(NsExcel + "Margin", Format(pageFooter.Margin));

                if (!string.IsNullOrEmpty(pageFooter.Text))
                    footerElement.SetAttributeValue(NsExcel + "Data",
                                                    "&" + pageFooter.HorizontalAlignment.ToString()[0] +
                                                    Trim(ParseString(pageFooter.Text)));

                element.Add(footerElement);
            }

            return element;
        }

        private string Trim(string s)
        {
            return s.Replace("\r\n", "\n");
        }


        private string Format(Margin margin)
        {
            if (margin.Left == margin.Top && margin.Top == margin.Right && margin.Right == margin.Bottom)

                return string.Format("{0}", margin.Left);

            return string.Format("{0},{1},{2},{3}", margin.Left, margin.Top, margin.Right, margin.Bottom);
        }

        private XElement GetStyles(Dictionary<Style, string> styleMaps)
        {
            var styles = new XElement(NsWb + "Styles");

            foreach (var styleMap in styleMaps)
            {
                styles.Add(RenderStyle(styleMap.Value, styleMap.Key));
            }

            var defaultStyle = new Style();
            defaultStyle.WrapText = true;
            defaultStyle.VerticalAlignment = VerticalAlignment.Bottom;
            defaultStyle.HorizontalAlignment = HorizontalAlignment.Left;
            defaultStyle.Font = new Font

                                    {
                                        FontName = "Calibri",
                                        Family = "Swiss",
                                        Size = 11,
                                        Color = Color.Black,
                                    };

            styles.Add(RenderStyle("Default", defaultStyle));

            return styles;
        }

        private IEnumerable<XElement> RenderRows(Grid grid, Dictionary<Style, string> styles)
        {
            var rows = new List<XElement>();

            foreach (GridRow row in grid.Rows)
            {
                rows.Add(RenderRow(row, row.Height, styles));
            }

            return rows;
        }

        private XElement RenderRow(GridRow row, int? height, Dictionary<Style, string> styles)
        {
            var rowNode = new XElement(NsWb + "Row");

            if (height == null)

                rowNode.SetAttributeValue(NsSpreadsheet + "AutoFitHeight", GetAttributeValue(true));

            else

                rowNode.SetAttributeValue(NsSpreadsheet + "Height", height.Value);

            int colSpan = 0;
            foreach (GridCell cell in row.Cells)
            {
                if (colSpan-- > 0)
                {
                    continue; // jump merge
                }
                if (cell.ColSpan.HasValue)
                    colSpan = cell.ColSpan.Value - 1;
                rowNode.Add(RenderCell(cell, styles));
            }

            return rowNode;
        }

        private XElement RenderCell(GridCell cell, Dictionary<Style, string> styles)
        {
            var cellNode = new XElement(NsWb + "Cell");

            if (cell.Style != null)
                cellNode.SetAttributeValue(NsSpreadsheet + "StyleID", styles[cell.Style]);

            var data = new XElement(NsSpreadsheet + "Data");

            switch (cell.Format)
            {
                case GridFormat.Number:
                case GridFormat.String:
                    data.SetAttributeValue(NsSpreadsheet + "Type", cell.Format);
                    data.Add(ParseString(cell.Text));
                    break;
                case GridFormat.Html:
                    data.SetAttributeValue(NsSpreadsheet + "Type", "String");
                    data.SetAttributeValue("xmlns", NsHtml);
                    data.Add(new XCData(ParseString(cell.Text)));
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            cellNode.Add(data);
            if (cell.ColSpan.HasValue && cell.ColSpan.Value > 1)
                cellNode.SetAttributeValue(NsSpreadsheet + "MergeAcross", cell.ColSpan.Value - 1);
            return cellNode;
        }

        private XElement RenderHeaderRows(Grid grid, int? height, Dictionary<Style, string> styles)
        {
            var row = new XElement(NsWb + "Row");

            if (height == null)

                row.SetAttributeValue(NsSpreadsheet + "AutoFitHeight", GetAttributeValue(true));

            foreach (GridHeader header in grid.Columns.Select(x => x.Header))
            {
                row.Add(RenderCell(header, styles));
            }

            return row;
        }

        private IEnumerable<XElement> RenderColumns(Grid grid)
        {
            var columns = new List<XElement>();

            int excelColIndex = 1;

            foreach (GridColumn col in grid.Columns)
            {
                var column = new XElement(NsSpreadsheet + "Column"
                                          , new XAttribute(NsSpreadsheet + "Index", excelColIndex++)
                    );

                column.SetAttributeValue(NsSpreadsheet + "Width", col.Width);

                columns.Add(column);
            }

            return columns;
        }

        private Dictionary<Style, string> GetStyles(IEnumerable<Grid> grids)
        {
            IEnumerable<IGrouping<Style, Style>> rowsStyleQuery = from grid in grids
                                                                  from row in grid.Rows
                                                                  from cell in row.Cells
                                                                  where cell.Style != null
                                                                  group cell.Style by cell.Style
                                                                  into style
                                                                  select style;

            IEnumerable<IGrouping<Style, Style>> headersStyleQuery = from grid in grids
                                                                     from row in grid.Columns
                                                                     let cell = row.Header
                                                                     where cell.Style != null
                                                                     group cell.Style by cell.Style
                                                                     into style
                                                                     select style;

            IEnumerable<IGrouping<Style, Style>> styleQuery = rowsStyleQuery.Union(headersStyleQuery);


            int styleKey = 1;

            var styles = new Dictionary<Style, string>();

            foreach (var grouping in styleQuery)
            {
                string key = styleKey.ToString().PadLeft(3, '0');
                Style style = grouping.Key;
                styles.Add(style, key);
                styleKey++;
            }

            return styles;
        }

        private XElement RenderStyle(string styleId, Style style)
        {
            var xstyle = new XElement(NsWb + "Style",
                                      new XAttribute(NsSpreadsheet + "ID", styleId));


            var attrs = new Dictionary<XName, object>
                            {
                                {NsSpreadsheet + "Horizontal", style.HorizontalAlignment},
                                {NsSpreadsheet + "Vertical", style.VerticalAlignment},
                                {NsSpreadsheet + "WrapText", style.WrapText},
                            };

            AddElementWithAttribute(NsWb + "Alignment", attrs, xstyle);


            attrs = new Dictionary<XName, object>
                        {
                            {NsSpreadsheet + "Color", style.BackgroundColor},
                            {NsSpreadsheet + "Pattern", style.BackgroundColor.HasValue ? "Solid" : null},
                        };

            AddElementWithAttribute(NsWb + "Interior", attrs, xstyle);

            if (style.Font != null)
            {
                attrs = new Dictionary<XName, object>
                            {
                                {NsSpreadsheet + "Bold", style.Font.Bold},
                                {NsSpreadsheet + "Color", style.Font.Color},
                                {NsExcel + "Family", style.Font.Family},
                                {NsSpreadsheet + "FontName", style.Font.FontName},
                                {NsSpreadsheet + "Size", style.Font.Size},
                            };

                AddElementWithAttribute(NsSpreadsheet + "Font", attrs, xstyle);
            }
            if (style.NumberFormat != null)
            {
                attrs = new Dictionary<XName, object>
                            {
                                {NsSpreadsheet + "Format", style.NumberFormat.Format},
                            };
                AddElementWithAttribute(NsSpreadsheet + "NumberFormat", attrs, xstyle);
            }
            if (style.Borders != null)
            {
                var xborders = new XElement(NsWb + "Borders");
                xstyle.Add(xborders);
                AddBorderStyle(style.Borders.Left, xborders, "Left");
                AddBorderStyle(style.Borders.Right, xborders, "Right");
                AddBorderStyle(style.Borders.Bottom, xborders, "Bottom");
                AddBorderStyle(style.Borders.Top, xborders, "Top");
            }
            return xstyle;
        }

        private void AddBorderStyle(Border border, XElement xborders, string position)
        {
            Dictionary<XName, object> attrs;
            if (border != null)
            {
                attrs = new Dictionary<XName, object>
                            {
                                {NsSpreadsheet + "Weight", border.Weight},
                                {NsSpreadsheet + "Position", position},
                                {NsSpreadsheet + "LineStyle", "Continuous"},
                            };
                AddElementWithAttribute(NsSpreadsheet + "Border", attrs, xborders);
            }
        }


        private void AddElementWithAttribute(XName elementName, Dictionary<XName, object> attrs, XElement styleElement)
        {
            if (attrs.Any(x => x.Value != null))
            {
                var ele = new XElement(elementName);

                foreach (var attr in attrs.Where(x => x.Value != null))
                {
                    ele.SetAttributeValue(attr.Key, GetAttributeValue(attr.Value));
                }

                styleElement.Add(ele);
            }
        }


        private string GetAttributeValue(object value)
        {
            var boolvalue = value as bool?;

            if (boolvalue != null)

                return boolvalue.Value ? "1" : "0";

            var color = value as Color?;

            if (color != null)
            {
                string s = color.Value.ToArgb().ToString("x");
                if (s.Length > 6)
                    s = s.Substring(s.Length - 6, 6);
                if (s.Length < 6)
                    s = s.PadLeft(6, '0');

                return "#" + s;
            }

            return value.ToString();
        }


        private string ParseString(object value)
        {
            if (value == null) return "";
            return value.ToString();
        }

        private XDocument CreateXDocument()
        {
            var doc = new XDocument();

            doc.Add(new XProcessingInstruction("mso-application", "progid=\"Excel.Sheet\""));

            var workbook = new XElement(NsSpreadsheet + "Workbook"
                                        , new XAttribute("xmlns", NsSpreadsheet)
                                        , new XAttribute(XNamespace.Xmlns + "o", NsOffice)
                                        , new XAttribute(XNamespace.Xmlns + "x", NsExcel)
                                        , new XAttribute(XNamespace.Xmlns + "ss", NsSpreadsheet)
                                        , new XAttribute(XNamespace.Xmlns + "html", NsHtml)
                );

            var docProp = new XElement(NsOffice + "DocumentProperties"
                                       , new XElement(NsOffice + "Author", "WNB")
                                       , new XElement(NsOffice + "LastAuthor", "WNB")
                                       , new XElement(NsOffice + "Created", DateTime.Now.ToString())
                                       , new XElement(NsOffice + "Version", "12.00")
                );

            var excelWorkbook = new XElement(NsExcel + "ExcelWorkbook"
                                             , new XElement(NsExcel + "WindowHeight", "8385")
                                             , new XElement(NsExcel + "WindowWidth", "11295")
                                             , new XElement(NsExcel + "WindowTopX", 360)
                                             , new XElement(NsExcel + "WindowTopY", 15)
                );


            workbook.Add(docProp);
            workbook.Add(excelWorkbook);
            doc.Add(workbook);
            doc.Add();
            return doc;
        }
    }

    public class GridOption
    {
        public GridOption()
        {
            PageSetup = new PageSetup();
        }

        public PageSetup PageSetup { get; set; }
    }

    public class PageSetup
    {
        public PageSetup()
        {
            Header = new PageHeader();

            Footer = new PageFooter();
        }


        public Margin PageMargin { get; set; }
        public PageHeader Header { get; set; }
        public PageFooter Footer { get; set; }
        public PageSetupLayout Layout { get; set; }
    }


    public enum PageSetupLayout
    {
        Portrait,
        Landscape,
    }


    public class PageFooter
    {
        public Margin Margin { get; set; }
        public string Text { get; set; }
        public HorizontalAlignment HorizontalAlignment { get; set; }
    }

    public class PageHeader
    {
        public Margin Margin { get; set; }
        public string Text { get; set; }
        public HorizontalAlignment HorizontalAlignment { get; set; }
    }

    public class NumberFormat
    {
        public string Format { get; set; }
    }

    public class Margin
    {
        public Margin(decimal width)
        {
            Left = width;
            Right = width;
            Top = width;
            Bottom = width;
        }

        public decimal Left { get; set; }
        public decimal Right { get; set; }
        public decimal Top { get; set; }
        public decimal Bottom { get; set; }
    }
}