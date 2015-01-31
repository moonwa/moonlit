using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.Caching;
using Moonlit.CastleExtensions.Caching;

namespace Moonlit.CastleExtensions.TextFixtures.Caching
{
    [TestClass]
    public class CachingTest
    {
        [TestMethod]
        public void CacheAndRefresh()
        {
            WindsorContainer container = new WindsorContainer();
            container.AddFacility<CacheFacility>();

            container.Register(Component.For<IPerson>().ImplementedBy<Person>());
            var person = container.Resolve<IPerson>();
            person.SetName("QQ");
            Assert.AreEqual("QQ", person.GetName());
            Assert.AreEqual("QQ", person.GetName());
            person.SetName("TT");
            Assert.AreEqual("QQ", person.GetName());

            person.Refresh();
            Assert.AreEqual("TT", person.GetName());
        }
    }
}