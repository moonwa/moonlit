using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.Proxy.WcfClient;

namespace Moonlit.Proxy.TextFixtures.WcfClient
{
    [TestClass]
    public class WcfClientTests
    {
        private ServiceHost _serviceHost;
        [TestInitialize]
        public void Init()
        {
            _serviceHost = new ServiceHost(typeof(TestSvc));
            _serviceHost.Open();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _serviceHost.Close();
        }
        [TestMethod]
        public async Task CallAsyncTest()
        {
            var client = WcfClientProxy.CreateProxy<ITestSvc>();
            var text = await client.GetMessageAsync("zz");
            Assert.AreEqual("hello zz", text);
        }
        [TestMethod]
        public void CallTest()
        {
            var client = WcfClientProxy.CreateProxy<ITestSvc>();
            var text = client.GetMessageSync("zz");
            Assert.AreEqual("hello zz", text);
        }
        [TestMethod]
        public async Task CallInAsync100Test()
        {
            var client1 = WcfClientProxy.CreateProxy<ITestSvc>();
            for (int i = 0; i < 100; i++)
            {
                var text = await client1.GetMessageAsync("zz");
                Assert.AreEqual("hello zz", text);
            }
            for (int i = 0; i < 100; i++)
            {
                var client2 = WcfClientProxy.CreateProxy<ITestSvc>();
                var text = await client2.GetMessageAsync("zz");
                Assert.AreEqual("hello zz", text);
            }
        }
        [TestMethod]
        public void CallIn100Test()
        {
            var client1 = WcfClientProxy.CreateProxy<ITestSvc>();
            for (int i = 0; i < 100; i++)
            {
                var text = client1.GetMessageSync("zz");
                Assert.AreEqual("hello zz", text);
            }
            for (int i = 0; i < 100; i++)
            {
                var client2 = WcfClientProxy.CreateProxy<ITestSvc>();
                var text = client2.GetMessageSync("zz");
                Assert.AreEqual("hello zz", text);
            }
        }
        [TestMethod]

        public void CallInExceptionTest()
        {
            bool isFault = false;

            try
            {
                var client = WcfClientProxy.CreateProxy<ITestSvc>();
                var text = client.GetMessageSync("fault");
            }
            catch (FaultException<MyFault> ex) when (ex.Detail.Text == "fault")
            {
                isFault = true;
            }
            catch (Exception ex)
            {

            }
            Assert.IsTrue(isFault);
        }

        [TestMethod]

        public async Task CallInAsyncExceptionTest()
        {
            bool isFault = false;

            try
            {
                var client = WcfClientProxy.CreateProxy<ITestSvc>();
                var text = await client.GetMessageAsync("fault");
            }
            catch (FaultException<MyFault> ex) when (ex.Detail.Text == "fault")
            {
                isFault = true;
            }
            catch (Exception ex)
            {

            }
            Assert.IsTrue(isFault);
        }

    }

    [ServiceContract]
    public interface ITestSvc
    {
        [OperationContract]
        [FaultContract(typeof(MyFault))]
        Task<string> GetMessageAsync(string text);
        [OperationContract]
        [FaultContract(typeof(MyFault))]
        string GetMessageSync(string text);
    }

    [DataContract]
    public class MyFault
    {
        [DataMember]
        public string Text { get; set; }
    }
    public class TestSvc : ITestSvc
    {
        public Task<string> GetMessageAsync(string text)
        {
            if (text == "fault")
            {
                throw new FaultException<MyFault>(new MyFault() { Text = text });
            }
            return Task.FromResult("hello " + text);
        }

        public string GetMessageSync(string text)
        {
            if (text == "fault")
            {
                throw new FaultException<MyFault>(new MyFault() { Text = text });
            }
            return "hello " + text;
        }
    }
}
