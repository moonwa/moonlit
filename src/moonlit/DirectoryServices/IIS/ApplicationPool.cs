using System;
using System.Collections.Generic;
using System.DirectoryServices;

namespace Moonlit.DirectoryServices.IIS
{
    public class ApplicationPool
    {
        private readonly DirectoryEntry _entry;
        private readonly IISRoot _root;

        internal ApplicationPool(DirectoryEntry entry, IISRoot root)
        {
            _entry = entry;
            _root = root;
            this.Name = entry.Name;
        }

        public string Name { get; internal set; }

        public void Recycle()
        {
            _entry.Invoke("Recycle", null);
        }
        public void Delete()
        {
            _root.DeleteApplicationPool(this);
        }
        public AppPoolIdentityTypes AppPoolIdentityType
        {
            get
            {
                return (AppPoolIdentityTypes)_entry.InvokeGet("AppPoolIdentityType");
            }
            set
            {
                _entry.InvokeSet("AppPoolIdentityType", new Object[] { (int)value });
            }
        }
        public string WAMUserName
        {
            get
            {
                return (string)_entry.InvokeGet("WAMUserName");
            }
            set
            {
                _entry.InvokeSet("WAMUserName", new Object[] { value });
            }
        }
        public string WAMUserPass
        {
            get
            {
                return (string)_entry.InvokeGet("WAMUserPass");
            }
            set
            {
                _entry.InvokeSet("WAMUserPass", new Object[] { value });
            }
        }

        public void CommitChanges()
        {
            _entry.CommitChanges();
        }
    }
}