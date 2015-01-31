//using Moonlit.IO;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moonlit;
//using System.Collections.Generic;
//using System;
//using System.Text;

//namespace Moonlit.IO
//{


//    /// <summary>
//    ///这是 ProtocolAdapter_Test 的测试类，旨在
//    ///包含所有 ProtocolAdapter_Test 单元测试
//    ///</summary>
//    [TestClass()]
//    public class ProtocolAdapter_Test
//    {


//        private TestContext testContextInstance;

//        /// <summary>
//        ///获取或设置测试上下文，上下文提供
//        ///有关当前测试运行及其功能的信息。
//        ///</summary>
//        public TestContext TestContext
//        {
//            get
//            {
//                return testContextInstance;
//            }
//            set
//            {
//                testContextInstance = value;
//            }
//        }

//        #region 附加测试属性
//        // 
//        //编写测试时，还可使用以下属性:
//        //
//        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
//        //[ClassInitialize()]
//        //public static void MyClassInitialize(TestContext testContext)
//        //{
//        //}
//        //
//        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
//        //[ClassCleanup()]
//        //public static void MyClassCleanup()
//        //{
//        //}
//        //
//        //使用 TestInitialize 在运行每个测试前先运行代码
//        //[TestInitialize()]
//        //public void MyTestInitialize()
//        //{
//        //}
//        //
//        //使用 TestCleanup 在运行完每个测试后运行代码
//        //[TestCleanup()]
//        //public void MyTestCleanup()
//        //{
//        //}
//        //
//        #endregion


//        /// <summary>
//        ///Network_Received 的测试
//        ///</summary>
//        [TestMethod]
//        public void Network_Received_TestHelper()
//        {
//            try
//            {
//                MockIAsyNetwork network = new MockIAsyNetwork();
//                MockProtocol proc = new MockProtocol();

//                ProtocolAdapter<TestProtocol> adapter = new ProtocolAdapter<TestProtocol>(network, proc);
//                string s = string.Empty;
//                int receivedCount = 0;
//                adapter.Received += new System.EventHandler<EventArgs<TestProtocol>>((sender, e) => { s += e.Value.Value; ++receivedCount; });


//                network.RaiseReceivedString("hell");

//                Assert.AreEqual(4, receivedCount);
//                Assert.AreEqual("hell", s);
//            }
//            catch (Exception ex)
//            { }
//        }

//        #region Mock
//        class TestProtocol
//        {
//            public char Value { get; set; }
//        }
//        class MockProtocol : IProtocolPacker<TestProtocol>
//        {

//            #region IProtocolPacker<TestProtocol> 成员

//            public TestProtocol Pack(System.IO.MemoryStream data)
//            {
//                if (data.Length < 1) return null;
//                byte[] buf = new byte[1];
//                data.Read(buf, 0, 1);
//                return new TestProtocol { Value = (char)buf[0] };
//            }

//            public byte[] Unpack(TestProtocol pack)
//            {
//                return new byte[] { (byte)pack.Value };
//            }

//            #endregion
//        }
//        class MockIAsyNetwork : IAsyNetwork
//        {
//            public MockIAsyNetwork()
//            {
//                Datas = new List<byte>();
//            }
//            public List<byte> Datas { get; set; }
//            #region IAsyNetwork 成员
//            public void RaiseReceivedString(string s)
//            {
//                var bytes = Encoding.ASCII.GetBytes(s);
//                RaiseReceived(bytes);
//            }
//            public void RaiseReceived(byte[] data)
//            {
//                if (Received != null)
//                    Received(this, new EventArgs<byte[]>(data));
//            }
//            public event System.EventHandler<EventArgs<byte[]>> Received;

//            public void Send(byte[] data)
//            {
//                Datas.AddRange(data);
//            }

//            #endregion
//        }
//        #endregion
//    }
//}
