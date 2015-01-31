using System;
using System.Collections;
using System.DirectoryServices;

namespace Moonlit.DirectoryServices.IIS
{
    public class WebSite
    {
        private readonly DirectoryEntry _entry;
        private readonly IISRoot _root;
        private int _siteId;
        public WebSite(DirectoryEntry entry, IISRoot root)
        {
            _entry = entry;
            _root = root;

            this._siteId = Convert.ToInt32(entry.Name);
        }

        public WebVirtualDir CreateVirtualDir(string name, string physicalPath)
        {
            DirectoryEntry newRoot = _entry.Children.Add(name, "IIsWebVirtualDir");
            WebVirtualDir virtualDir = new WebVirtualDir(newRoot);
            virtualDir.PhysicalPath = physicalPath;
            newRoot.CommitChanges();
            return virtualDir;
        }
        public void CommitChanges()
        {
            _entry.CommitChanges();
        }

        public string Name
        {
            get
            {
                return (string)_entry.Properties["ServerComment"][0];
            }
            set
            {
                _entry.Properties["ServerComment"][0] = value;
            }
        }
        //public string AppPoolId
        //{
        //    get
        //    {
        //        return (string)_entry.Properties["AppPoolId"][0];
        //    }
        //    set
        //    {
        //        _entry.Properties["AppPoolId"][0] = value;
        //    }
        //}

        private ServerBindings _serverBindings;
        public ServerBindings ServerBindings
        {
            get
            {
                EnsureServerBindings();
                return _serverBindings;
            }
        }

        public int SiteId
        {
            get { return _siteId; }
        }

        private void EnsureServerBindings()
        {
            if (_serverBindings== null)
                _serverBindings = new ServerBindings(_entry.Properties["ServerBindings"]);
        } 
    }
}