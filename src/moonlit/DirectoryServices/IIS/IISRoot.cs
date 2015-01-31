using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;

namespace Moonlit.DirectoryServices.IIS
{
    public class IISRoot
    {
        private List<ApplicationPool> _applicationPools;
        private List<WebSite> _webSites;
        public WebServerTypes ServiceType { get; private set; }
        public IISRoot(string domainName)
        {
            DomainName = domainName;
            if (string.IsNullOrEmpty(domainName))
            {
                DomainName = "localhost";
            }
            var entry = new DirectoryEntry("IIS://" + domainName + "/W3SVC/INFO");
            ServiceType = (WebServerTypes)(int)entry.Properties["MajorIISVersionNumber"].Value;
        }

        public string DomainName { get; private set; }

        /// <summary>
        /// Returns a list of all the Application Pools configured
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ApplicationPool> ApplicationPools
        {
            get
            {
                return EnsureApplicationPools().AsReadOnly();
                //if (ServerType != WebServerTypes.IIS6 &&
                //    ServerType != WebServerTypes.IIS7)
                //    return null;
            }
        }

        public IEnumerable<WebSite> WebSites
        {
            get { return EnsureWebSites().AsReadOnly(); }
        }

        private List<WebSite> EnsureWebSites()
        {
            var list = _webSites;
            if (list != null)
                return list;

            var webSites = new DirectoryEntry("IIS://" + this.DomainName + "/W3SVC");
            list = new List<WebSite>();
            foreach (DirectoryEntry entry in webSites.Children)
            {
                if (entry.SchemaClassName.EndsWith("Server"))
                {
                    WebSite webSite = new WebSite(entry, this);
                    list.Add(webSite);
                }
            }
            _webSites = list;
            return list;
        }
        private List<ApplicationPool> EnsureApplicationPools()
        {
            var list = _applicationPools;
            if (list != null)
                return list;

            var appPools = new DirectoryEntry("IIS://" + this.DomainName + "/W3SVC/AppPools");
            list = new List<ApplicationPool>();
            foreach (DirectoryEntry entry in appPools.Children)
            {
                var pool = new ApplicationPool(entry, this);

                list.Add(pool);
            }
            _applicationPools = list;
            return list;
        }

        /// <summary>
        /// Create a new Application Pool and return an instance of the entry
        /// </summary>
        /// <param name="appPoolName"></param>
        /// <returns></returns>
        public ApplicationPool CreateApplicationPool(string appPoolName)
        {
            var applicationPools = EnsureApplicationPools();

            var root = new DirectoryEntry("IIS://" + this.DomainName + "/W3SVC/AppPools");
            var appPool = root.Invoke("Create", "IIsApplicationPool", appPoolName) as DirectoryEntry;
            appPool.CommitChanges();

            var newAppPool = new ApplicationPool(appPool, this);
            applicationPools.Add(newAppPool);

            return newAppPool;
        }

        internal void DeleteApplicationPool(ApplicationPool applicationPool)
        {
            var applicationPools = EnsureApplicationPools();

            var root = new DirectoryEntry("IIS://" + this.DomainName + "/W3SVC/AppPools");
            root.Invoke("Delete", "IIsApplicationPool", applicationPool.Name);

            var appPool =
                applicationPools.FirstOrDefault(
                    x => string.Equals(x.Name, applicationPool.Name, StringComparison.OrdinalIgnoreCase));
            if (appPool != null) applicationPools.Add(appPool);
        }

        public WebSite CreateSite(string siteId, string siteName, string physicalRootPath)
        {
            DirectoryEntry service = new DirectoryEntry("IIS://" + this.DomainName + "/W3SVC");
            string className = service.SchemaClassName;
            //if (className.EndsWith("Service"))
            {
                DirectoryEntries sites = service.Children;
                DirectoryEntry newSite = sites.Add(siteId, (className.Replace("Service", "Server")));
                var site = new WebSite(newSite, this);
                site.Name = siteName;
                site.CommitChanges();

                var virtualDir = site.CreateVirtualDir("root", physicalRootPath);
                virtualDir.CommitChanges();
                return site;
            }

        }
    }
}