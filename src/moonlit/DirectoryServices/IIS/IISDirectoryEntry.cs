using System;
using System.Collections.Generic;
using System.Text;
using System.DirectoryServices;
using Moonlit.DirectoryServices.IIS;

namespace Moonlit.DirectoryServices
{
    public partial class IisDirectoryEntry : IDisposable
    {
        private DirectoryEntry _baseDirectoryEntry;
        protected DirectoryEntry BaseDirectoryEntry
        {
            get{return this._baseDirectoryEntry;}
            set{this._baseDirectoryEntry = value;}
        }
        protected IisDirectoryEntry(DirectoryEntry entry) { this._baseDirectoryEntry = entry; }
        protected IisDirectoryEntry() { this._baseDirectoryEntry = new DirectoryEntry();}
        protected IisDirectoryEntry(string path) { this._baseDirectoryEntry = new DirectoryEntry(path); }
        protected IisDirectoryEntry(object adsObject)  {this._baseDirectoryEntry = new DirectoryEntry(adsObject); }
        protected IisDirectoryEntry(string path, string username, string password) { this._baseDirectoryEntry = new DirectoryEntry(path, username, password); }
        protected IisDirectoryEntry(string path, string username, string password, AuthenticationTypes authenticationTypes) { this._baseDirectoryEntry = new DirectoryEntry(path, username, password, authenticationTypes); }
        protected object GetBaseEntryPropertyValue(string propertyKey)
        {
            return this.BaseDirectoryEntry.Properties[propertyKey][0]; 
        }
        protected void SetBaseEntryPropertyValue(string propertyKey, object obj)
        {
            if (this.BaseDirectoryEntry.Properties[propertyKey].Value == null)
                this.BaseDirectoryEntry.Properties[propertyKey].Value = obj;
            this.BaseDirectoryEntry.Properties[propertyKey].Insert(0,obj); 
        }
        public string Name { get { return this.BaseDirectoryEntry.Name; } }
        public static IisDirectoryEntry Create (WebSiteServiceType websiteServiceType)
        {
            return new IisDirectoryEntry("IIS://localhost/W3SVC/1/root");
        }
        public string DefaultDoc { get { return this.GetBaseEntryPropertyValue("DefaultDoc").ToString(); } set { this.SetBaseEntryPropertyValue("DefaultDoc", value); } }

        public string AppFriendlyName
        {
            get { return this.GetBaseEntryPropertyValue("AppFriendlyName").ToString(); }
        }
        public IISAuthFlag AuthFlag
        {
            get { return (IISAuthFlag)this.GetBaseEntryPropertyValue("AuthFlags"); }
            set { this.SetBaseEntryPropertyValue("AuthFlags", value); }
        }
        public string AppRoot
        {
            get { return this.GetBaseEntryPropertyValue("AppRoot").ToString(); }
        }
        public string PhysicalDirectoryPath
        {
            get { return this.GetBaseEntryPropertyValue("Path").ToString(); }
            set { this.SetBaseEntryPropertyValue("Path", value); }
        }
        private Dictionary<string, IisDirectoryEntry.ScriptMap> m_scriptMaps = null;
        public Dictionary<string, IisDirectoryEntry.ScriptMap> ScriptMaps 
        {
            get 
            {
                if (this.m_scriptMaps == null)
                {
                    this.m_scriptMaps = new Dictionary<string, IisDirectoryEntry.ScriptMap>();
                    PropertyValueCollection pvs = this._baseDirectoryEntry.Properties["ScriptMaps"];
                    for (int i = 0; i < pvs.Count; i++)
                    {
                        // .asp,C:\WINDOWS\system32\inetsrv\asp.dll,5,GET,HEAD,POST,TRACE
                        //
                        string s = (string)pvs[i];
                        string[] fileds = s.Split(',');
                        ScriptMap map = new ScriptMap();
                        map.FileExName = fileds[0];
                        map.AssemblyName = fileds[1];

                        for (int j = 3; j < fileds.Length; j++)
                        {
                            HttpAction action = (HttpAction)Enum.Parse(typeof(HttpAction), fileds[j], true);
                            map.Actions |= action;
                        }
                        if(!this.m_scriptMaps.ContainsKey(map.FileExName))
                            this.m_scriptMaps.Add(map.FileExName, map);
                    }
                }
                return this.m_scriptMaps;
            }
        }
        public void AddScriptMap(IisDirectoryEntry.ScriptMap map)
        {
            if (this.ScriptMaps.ContainsKey(map.FileExName))
            {
                return;
            }
            this.ScriptMaps.Add(map.FileExName, map);
            this.BaseDirectoryEntry.Properties["ScriptMaps"].Add(map.ToString());
        }
        public void CommitChanges()
        {
            this.BaseDirectoryEntry.CommitChanges();
        }

        public IisDirectoryEntry CreateVirtualDir(string virtualDirName, string physicalDir, WebSiteServiceType serviceType)
        {
            IisDirectoryEntry virDir = new IisDirectoryEntry(this.BaseDirectoryEntry.Children.Add (virtualDirName, "IIsWebVirtualDir"));
            virDir.BaseDirectoryEntry.Properties["Path"].Insert(0, physicalDir);
            virDir.BaseDirectoryEntry.Invoke("AppCreate", true);            
            return virDir;
        }
        public KeyType AppKeyType
        {
            get
            {
                return (KeyType)Enum.Parse(typeof(KeyType), this.GetBaseEntryPropertyValue("KeyType").ToString(), true);
            }
        }
        public void RefreshCache()
        {
            this.BaseDirectoryEntry.RefreshCache();
        }
        public void Close() 
        {
            this.BaseDirectoryEntry.Close();
        }
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this.BaseDirectoryEntry.Children.GetEnumerator());
        }

        #region IDisposable 成员

        public void Dispose()
        {
            this.Close();
        }

        #endregion
    }
}
