using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;

namespace Moonlit.ActiveDirectory
{
    /// <summary>
    /// Define AD entities
    /// </summary>
    [Serializable()]
    public class ActiveDirectoryEntry
    {
        /// <summary>
        /// LDAP URL
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// user name to connect ldap
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// user password to connect ldap
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// active or not
        /// </summary>
        public bool IsEnabled { get; set; }

        public IEnumerable<string> GetRolesOfUser(string userName,
           IEnumerable<string> exceptsRoles)
        {
            using (DirectoryEntry entry = CreateDirectoryEntry())
            {
                ActiveDirectoryUser user = GetActiveDirectoryUser(userName);

                foreach (string role in exceptsRoles)
                {
                    if (IsUserInRole(entry, user, role))
                    {
                        yield return role.ToLower();
                    }
                }
            }
        }

        static Regex RegexRoleOfUser = new Regex("CN=(?<value>.+?),", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private IEnumerable<string> GetRolesOfUser(DirectoryEntry de, ActiveDirectoryUser user)
        {
            DirectorySearcher search = new DirectorySearcher(de);
            search.Filter = "(&(objectCategory=person)(samAccountName=" + user.UserName + "))";
            search.PropertiesToLoad.Add("memberOf");

            SearchResult result = search.FindOne();
            if (result == null)
            {
                yield break;
            }
            int propertyCount = result.Properties["memberOf"].Count;


            for (int propertyCounter = 0; propertyCounter < propertyCount; propertyCounter++)
            {
                string dn = (String)result.Properties["memberOf"][propertyCounter];


                var match = RegexRoleOfUser.Match(dn);
                if (match.Success)
                {
                    yield return match.Groups["value"].Value;
                }
            }
        }
        private static string GetRoleFromRoleString(string roleName)
        {
            string role = string.Empty;

            bool IsRoleContainDamain = string.IsNullOrEmpty(roleName) ? false : roleName.Contains("/");
            if (IsRoleContainDamain)
            {
                if (roleName.Split('/').Length == 2)
                {
                    role = roleName.Split('/')[1];
                }
            }
            else
            {
                role = roleName;
            }
            return role;
        }
        private static string GetRoleDomainFromRole(string roleName)
        {
            string roleDomain = string.Empty;

            bool IsRoleContainDamain = string.IsNullOrEmpty(roleName) ? false : roleName.Contains("/");
            if (IsRoleContainDamain)
            {
                if (roleName.Split('/').Length == 2)
                {
                    roleDomain = roleName.Split('/')[0];
                }
                if (roleDomain.ToLower() == "All Domains".ToLower())  //special
                {
                    roleDomain = string.Empty;
                }
            }
            return roleDomain;
        }
        private bool IsUserInRole(DirectoryEntry de, ActiveDirectoryUser user, string role)
        {
            string currentAccessDomainName = de.Name.Replace("dc=", "").Replace("DC=", "");

            string roleDomain = GetRoleDomainFromRole(role);
            role = GetRoleFromRoleString(role);

            if (user.Roles == null)
            {
                var items = GetRolesOfUser(de, user);
                user.Roles = new List<string>(items);
            }
            if (user.Roles.Any(x => string.Equals(x, role, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            //if roles are in same domain
            if (string.IsNullOrEmpty(roleDomain) || roleDomain.ToLower() == currentAccessDomainName.ToLower() || roleDomain.ToLower() == "All Domains".ToLower())
            {
                string searchText = "(&(objectClass=group)(CN=" + role + "))";
                DirectorySearcher dsSystem = new DirectorySearcher(de, searchText);
                dsSystem.SearchScope = SearchScope.Subtree;
                dsSystem.PropertiesToLoad.Clear();
                dsSystem.PropertiesToLoad.Add("member");

                string userDomainInfo = String.Empty;
                foreach (SearchResult mSearchResult in dsSystem.FindAll())
                {
                    ResultPropertyValueCollection allMebers = mSearchResult.Properties["member"];
                    if (allMebers != null && allMebers.Count > 0)
                    {
                        foreach (var meberItem in allMebers)
                        {
                            userDomainInfo = meberItem as string;

                            List<string> memberList = GetADSIRoles(userDomainInfo).ToList();
                            if (memberList.Count > 0)
                            {
                                string member = memberList[0];
                                if (user.UserName.Equals(member, StringComparison.OrdinalIgnoreCase)
                                    || string.Equals(user.UserSid, member, StringComparison.OrdinalIgnoreCase))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        static Regex RegexCnEquals = new Regex("CN=(?<value>.+?),", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static IEnumerable<string> GetADSIRoles(string strObj)
        {
            // CN=S-1-5-21-854245398-1004336348-725345543-197870,CN=ForeignSecurityPrincipals,DC=karmalab,DC=net
            if (!strObj.EndsWith(","))
                strObj += ",";
            foreach (Match match in RegexCnEquals.Matches(strObj))
            {
                yield return match.Groups["value"].Value;
            }
        }
        public ActiveDirectoryUser GetActiveDirectoryUser(string domainUserName)
        {
            ActiveDirectoryUser user = new ActiveDirectoryUser();
            string[] userNameArray = domainUserName.Split(new char[] { '/', '\\', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            user.UserDomain = userNameArray[0];
            user.UserName = userNameArray[1];
            //SecurityIdentifier si = (SecurityIdentifier)new NTAccount(user.UserDomain, user.UserName)
            //    .Translate(typeof(SecurityIdentifier));
            //user.UserSid = si.Value;
            user.UserSid = "S-1-5-21-854245398-1004336348-725345543-152698";
            user.Parent = this;
            return user;
        }
        private DirectoryEntry CreateDirectoryEntry()
        {
            DirectoryEntry deSystem = string.IsNullOrEmpty(Password) ?
                new DirectoryEntry(BaseUrl) :
                new DirectoryEntry(BaseUrl, UserName, Password);

            //deSystem.RefreshCache();
            return deSystem;
        }

    }


    public class ActiveDirectoryUser
    {
        private List<string> _roles;
        public string UserSid { get; set; }

        public string UserName { get; set; }

        public string UserDomain { get; set; }

        public List<string> Roles
        {
            get
            {
                LoadRoles();
                return _roles;
            }
            set { _roles = value; }
        }

        private void LoadRoles()
        {

        }

        internal ActiveDirectoryEntry Parent { get; set; }
    }
}

