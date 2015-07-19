using System.Diagnostics;

namespace Moonlit.Mvc.Maintenance.Domains
{
    [DebuggerDisplay("#{CultureTextId} {Name}:{Text}")]
    public class CultureText
    {
        public int CultureTextId { get; set; }
        public string Name { get; set; }
        public int CultureId { get; set; }
        public string Text { get; set; }
        public bool IsEdited { get; set; }
    }
}