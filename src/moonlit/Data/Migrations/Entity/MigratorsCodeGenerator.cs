using System;
using System.Collections.Generic;
using System.Linq;
using Moonlit.Data.Migrations;

namespace Moonlit.Data.Design.Entity
{
    public abstract class MigratorsCodeGenerator
    {
        protected string Target { get; set; }
        private readonly List<DbMigrator> _migrators;

        internal MigratorsCodeGenerator(IEnumerable<DbMigrator> migrators, string target)
        {
            Target = target;
            _migrators = new List<DbMigrator>(migrators);
        }
        public bool HasChanged
        {
            get { return _migrators.Any(); }
        }
        public abstract string Generate(string className, string @namespace, Version version);
        internal IEnumerable<DbMigrator> Migrators
        {
            get { return _migrators; }
        }
    }
}