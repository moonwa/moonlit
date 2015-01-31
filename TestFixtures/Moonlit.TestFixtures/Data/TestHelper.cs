using System;
using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.Data.Fixtures.Models;

namespace Moonlit.Data.Fixtures
{
    public class TestHelper
    {
        public const string ConnectionString = "server=.;database=Test;Integrated security=true";
    }
}
