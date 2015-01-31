using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.ObjectConverts;
using Moonlit.ObjectConverts.ObjectReaders;

namespace Moonlit.TestFixtures.ObjectTranslators
{
    [TestClass()]
    public class ReaderTranslator_Test
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
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

        [TestMethod()]
        public void Map_Test()
        {
            ObjectConverter objectConverter = new ObjectConverter();


            DataTable dt = new DataTable();
            dt.Columns.Add("int16Var", typeof(Int16));
            dt.Columns.Add("int32Var", typeof(Int32));
            dt.Columns.Add("int64Var", typeof(Int64));
            dt.Columns.Add("datetimeVar", typeof(DateTime));
            dt.Columns.Add("decimalVar", typeof(decimal));
            dt.Columns.Add("floatVar", typeof(float));
            dt.Columns.Add("doubleVar", typeof(double));

            dt.Columns.Add("stringVar", typeof(string));
            dt.Columns.Add("MyEnum", typeof(int));
            DateTime tm = DateTime.Parse("1998-01-01");
            dt.Rows.Add(1, 2, 3, tm, 1M, 2f, 3d, "hello", MyEnum.A);

            DataTableReader reader = new DataTableReader(dt);
            MapTestObject1 obj = new MapTestObject1();
            reader.Read();
            objectConverter.MapObject(reader, obj);
            Assert.AreEqual(1, obj.int16Var);
            Assert.AreEqual(2, obj.int32Var);
            Assert.AreEqual(3, obj.int64Var);
            Assert.AreEqual(tm, obj.datetimeVar);
            Assert.AreEqual(1M, obj.decimalVar);
            Assert.AreEqual(2f, obj.floatVar);
            Assert.AreEqual(3d, obj.doubleVar);
            Assert.AreEqual(MyEnum.A, obj.MyEnum);

            MapTestObject2 obj2 = new MapTestObject2();
            objectConverter.MapObject(reader, obj2);
            Assert.AreEqual((short)1, obj2.int16Var);
            Assert.AreEqual(2, obj2.int32Var);
            Assert.AreEqual((long)3, obj2.int64Var);
            Assert.AreEqual(tm, obj2.datetimeVar);
            Assert.AreEqual(1M, obj2.decimalVar);
            Assert.AreEqual(2f, obj2.floatVar);
            Assert.AreEqual(3d, obj2.doubleVar);
            Assert.AreEqual(MyEnum.A, obj2.MyEnum);
        }



        [TestMethod()]
        public void MapUnexistProperty_Test()
        {
            ObjectConverter objectConverter = new ObjectConverter()
            {
                IsIgnoreNotExistingProperty = true,
            };


            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));

            dt.Rows.Add("QQ");

            MapWithoutPropertyTestObject1 obj = new MapWithoutPropertyTestObject1();
            var reader = new DataTableReader(dt);
            reader.Read();
            objectConverter.MapObject(reader, obj);
            Assert.AreEqual("QQ", obj.Name);
            Assert.AreEqual(100, obj.Age);
        }

        [TestMethod()]
        [ExpectedException(typeof(FieldNotFoundException))]
        public void MapUnexistPropertyException_Test()
        {
            ObjectConverter objectConverter = new ObjectConverter();


            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));

            dt.Rows.Add("QQ");
            var reader = new DataTableReader(dt);
            reader.Read();
            MapWithoutPropertyTestObject1 obj = new MapWithoutPropertyTestObject1();
            objectConverter.MapObject(reader, obj);

            Assert.AreEqual("QQ", obj.Name);
            Assert.AreEqual(100, obj.Age);
        }
    }
}
