using System.DirectoryServices;

namespace Moonlit.DirectoryServices.IIS
{
    public class WebVirtualDir
    {
        private readonly DirectoryEntry _entry;

        public WebVirtualDir(DirectoryEntry entry)
        {
            _entry = entry;
        }

        public string PhysicalPath
        {
            get
            {
                return (string)_entry.Properties["Path"][0];
            }
            set
            {
                _entry.Properties["Path"][0] = value;
            }
        }

        public bool IsAccessScript
        {
            get
            {
                return (bool)_entry.Properties["AccessScript"][0];
            }
            set
            {
                _entry.Properties["AccessScript"][0] = true;
            }
        }

        public void CommitChanges()
        {
            _entry.CommitChanges();
        }
    }
}