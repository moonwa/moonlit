using System.Reflection;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.Reflection;
 
namespace Moonlit.TestFixtures
{


    /// <summary>
    ///���� AssemblyDescriptor_Test �Ĳ����ּ࣬��
    ///�������� AssemblyDescriptor_Test ��Ԫ����
    ///</summary>
    [TestClass()]
    public class AssemblyDescriptor_Test
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
        ///GetPath �Ĳ���
        ///</summary>
        [TestMethod()]
        public void GetPath_Test()
        {
            Assembly assembly = typeof(AssemblyDescriptor_Test).Assembly;
            string assemblyPath = assembly.Location;

            Assert.IsTrue(File.Exists(assemblyPath));

            var assembly1 = AssemblyExtension.SafeLoadFile(assemblyPath);

            Assert.AreNotSame(assembly, assembly1);
            var type = assembly1.GetType("Moonlit.TestFixtures.AssemblyDescriptor_Test");
            Assert.IsNotNull(type);
        }
    }
}
