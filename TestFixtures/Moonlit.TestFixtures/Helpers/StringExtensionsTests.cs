using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Moonlit.TestFixtures.Helpers
{

    [TestClass()]
    public class StringExtensionsTests
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        [TestMethod()]
        public void FromCame_Test()
        {
            Assert.AreEqual("ACD Ticket", StringHelper.FromCamel("ACDTicket"));
            Assert.AreEqual("Ticket", StringHelper.FromCamel("Ticket"));
            Assert.AreEqual("NBA Current APC Ticket CCTV", StringHelper.FromCamel("NBACurrentAPCTicketCCTV"));
            Assert.AreEqual("ETicket", StringHelper.FromCamel("ETicket", 3));
            Assert.AreEqual("ETTicket", StringHelper.FromCamel("ETTicket", 3));
            Assert.AreEqual("ETC Ticket", StringHelper.FromCamel("ETCTicket", 3));
            Assert.AreEqual("ABCDEF Ticket", StringHelper.FromCamel("ABCDEFTicket", 3));
            Assert.AreEqual("ABC DEF Ticket", StringHelper.FromCamel("ABCDEFTicket", 3, "ABC"));
        }
    }
}
