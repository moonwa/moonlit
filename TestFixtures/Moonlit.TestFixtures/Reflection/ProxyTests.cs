using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.Reflection;

namespace Moonlit.TestFixtures.Reflection
{
    [TestClass]
    public class ProxyTests
    {
        public interface IA
        {
            void Echo();
            string EchoString();
            object EchoClass();
            int EchoInteger();
            int? EchoNullableInteger();
            Task<int> EchoTaskInteger();
        }
        [TestMethod]
        public void EmptyTest()
        {
            EmptyProxyFactory emptyProxyFactory = new EmptyProxyFactory();
            var ia = (IA)Activator.CreateInstance(emptyProxyFactory.CreateInterfaceProxy<IA>());
            Assert.IsNotNull(ia);
            ia.Echo();
            Assert.IsNull(ia.EchoClass());
            Assert.IsNull(ia.EchoString());
            Assert.AreEqual(0, ia.EchoInteger());
            Assert.IsFalse(ia.EchoNullableInteger().HasValue);
            Assert.IsNull(ia.EchoTaskInteger());

            Assert.AreEqual(emptyProxyFactory.CreateInterfaceProxy<IA>(), emptyProxyFactory.CreateInterfaceProxy<IA>());
        }
    }
}
