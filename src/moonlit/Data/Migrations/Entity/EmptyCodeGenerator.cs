using System;
using System.Collections.Generic;
using Moonlit.Data.Migrations;

namespace Moonlit.Data.Design.Entity
{
    internal class EmptyCodeGenerator : MigratorsCodeGenerator
    {
        internal EmptyCodeGenerator()
            : base(new List<DbMigrator>(), "")
        {
        }

        public override string Generate(string className, string @namespace, Version version)
        {
            return "";
        }
    }
}