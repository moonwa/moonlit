using System;

namespace Moonlit.Mvc.Maintenance.Domains
{
    public class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Privileges { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsBuildIn { get; set; }
        
        public string[] GetPrivileges()
        {
            if (string.IsNullOrEmpty(Privileges))
            {
                return new string[0];
            }
            return Privileges.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        }
        public void SetPrivileges(string[] value)
        {
            if (value == null)
            {
                Privileges = "";
            }
            else
            {
                Privileges = "," + string.Join(",", value) + ",";
            }

        }
    }
}