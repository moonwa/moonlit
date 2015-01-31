using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Moonlit.TestFixtures.Helpers
{
    [TestClass()]
    public class UriBuilderExtensions_Test
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
            UriBuilder uri = new UriBuilder("http://a/b/c");
            uri = uri.AddQuery("name", "z");

            Assert.AreEqual("http://a:80/b/c?name=z", uri.ToString());
            uri = uri.AddQuery("age", "12");
            Assert.AreEqual("http://a:80/b/c?name=z&age=12", uri.ToString());
        }
       
    }
}