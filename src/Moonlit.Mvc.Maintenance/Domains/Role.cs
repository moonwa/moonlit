﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Moonlit.Mvc.Maintenance.Domains
{
    public class Role : IKeyObject
    {
        public int RoleId { get; set; }
        [StringLength(32)]
        public string Name { get; set; }
        [StringLength(8000)]
        public string Privileges { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsBuildIn { get; set; }
        
        public string[] GetPrivileges()
        {
            if (string.IsNullOrEmpty(Privileges))
            {
                return new string[0];
            }
            return Privileges.ToLowerInvariant().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

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

        #region Implementation of IKeyObject

        string IKeyObject.Key
        {
            get { return this.RoleId.ToString(); }
        }

        #endregion
    }

}