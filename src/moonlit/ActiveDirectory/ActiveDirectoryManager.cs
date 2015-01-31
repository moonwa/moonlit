using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using Moonlit.Caching;

namespace Moonlit.ActiveDirectory
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActiveDirectoryManager
    {
        private List<ActiveDirectoryEntry> GetActivyDirectoryEntriesCore()
        {
            var entries = new List<ActiveDirectoryEntry>();

            ActiveDirectorySection section = System.Configuration.ConfigurationManager.GetSection("activeDirectory") as ActiveDirectorySection;
            if (section == null)
            {
                return null;
            }

            if (section.Directorys != null)
            {
                foreach (ActiveDirectoryElement activeDirectoryElement in section.Directorys)
                {
                    ActiveDirectoryEntry activeDirectoryEntry = new ActiveDirectoryEntry
                        {
                            Password = activeDirectoryElement.Password,
                            UserName = activeDirectoryElement.User,
                            BaseUrl = activeDirectoryElement.BaseUrl,
                            IsEnabled = activeDirectoryElement.IsEnabled
                        };

                    entries.Add(activeDirectoryEntry);
                }
            }
            return entries;
        }

         

        /// <summary>
        /// Get all activeActiveDirectories
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<ActiveDirectoryEntry> GetActiveDirectoryEntries()
        {
            return GetActivyDirectoryEntriesCore();
        } 
    }
}