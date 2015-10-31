using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.Diagnostics;

    #region TestFrameworkUsing
#if !NUNIT

#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestContext = Moonlit.Diagnostics.TestContextHelper;
#endif
#endregion
namespace Moonlit.TestFixtures.Diagnostics
{


    /// <summary>
    ///���� Profier_Test �Ĳ����ּ࣬��
    ///�������� Profier_Test ��Ԫ����
    ///</summary>
    [TestClass()]
    public class ProfierTests
    {


        private TestContext testContextInstance;

        /// <summary>
        ///��ȡ�����ò��������ģ��������ṩ
        ///�йص�ǰ�������м��书�ܵ���Ϣ��
        ///</summary>
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

        #region ���Ӳ�������
        // 
        //��д����ʱ������ʹ����������:
        //
        //ʹ�� ClassInitialize ���������еĵ�һ������ǰ�����д���
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //ʹ�� ClassCleanup �����������е����в��Ժ������д���
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //ʹ�� TestInitialize ������ÿ������ǰ�����д���
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //ʹ�� TestCleanup ��������ÿ�����Ժ����д���
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///Profier ���캯�� �Ĳ���
        ///</summary>
        [TestMethod()]
        public void Start_Test()
        {
            Profiler target = new Profiler();
            target.Start();
            Assert.IsTrue(target.Enabled);
        }

        /// <summary>
        ///Profier ���캯�� �Ĳ���
        ///</summary>
        [TestMethod()]
        public void Record_Test()
        {
            Profiler target = new Profiler();
            target.Start();
            target.Record(15);
            Thread.Sleep(300);
            target.Record(12);
            Thread.Sleep(1000);

            Assert.AreEqual(27, target.Fps);

            target.Record(7);
            Thread.Sleep(1000);

            Assert.AreEqual(7, target.Fps);
        }
    }
}
