using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Moonlit.TestFixtures.Helpers
{
    [TestClass()]
    public class ByteExtensionsTests
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


        [TestMethod()]
        public void ToBcd_Test()
        {
            Assert.AreEqual(0x12, ((byte)12).ToBcd());
            Assert.AreEqual(0x2, ((byte)2).ToBcd());
            Assert.AreEqual(0x10, ((byte)10).ToBcd());
        }
        /// <summary>
        ///GetPath �Ĳ���
        ///</summary>
        [TestMethod()]
        public void FromBcd_Test()
        {
            Assert.AreEqual(12, ((byte)0x12).FromBcd());
            Assert.AreEqual(2, ((byte)2).FromBcd());
            Assert.AreEqual(10, ((byte)10).FromBcd());
        }
    }
}