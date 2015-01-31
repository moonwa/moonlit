using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.ObjectConverts;
using Moonlit.ObjectConverts.ObjectReaders;

namespace Moonlit.TestFixtures.ObjectTranslators
{
    [TestClass()]
    public class ObjectTranslatorTest
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

        class StringArrayTest
        {
            public string[] StringArray { get; set; }
        }
        [TestMethod()]
        public void MapNullStringArray_Test()
        {
            var obj1 = new StringArrayTest
                {
                    StringArray = default(string[])
                };
            var obj2 = new StringArrayTest
                {
                    StringArray = (string[])null,
                };
            Assert.IsNull(obj1.StringArray);
            ObjectConverter objectConverter = new ObjectConverter();
            objectConverter.MapObject(obj1, obj2);
            Assert.IsNull(obj2.StringArray);
        }
        [TestMethod()]
        public void Map_Test()
        {
            ObjectConverter objectConverter = new ObjectConverter();

            DateTime tm = DateTime.Parse("1998-01-01");
            MapTestObject1_Copy dt = new MapTestObject1_Copy();
            dt.int16Var = 1;
            dt.int32Var = 2;
            dt.int64Var = 3;
            dt.datetimeVar = tm;
            dt.decimalVar = 1M;
            dt.floatVar = 2f;
            dt.doubleVar = 3;

            dt.stringVar = "hello";

            MapTestObject1 obj = new MapTestObject1();
            objectConverter.MapObject(dt, obj);
            Assert.AreEqual(1, obj.int16Var);
            Assert.AreEqual(2, obj.int32Var);
            Assert.AreEqual(3, obj.int64Var);
            Assert.AreEqual(tm, obj.datetimeVar);
            Assert.AreEqual(1M, obj.decimalVar);
            Assert.AreEqual(2f, obj.floatVar);
            Assert.AreEqual(3d, obj.doubleVar);

            MapTestObject2 obj2 = new MapTestObject2();
            objectConverter.MapObject(dt, obj2);
            Assert.AreEqual((short)1, obj2.int16Var);
            Assert.AreEqual(2, obj2.int32Var);
            Assert.AreEqual((long)3, obj2.int64Var);
            Assert.AreEqual(tm, obj2.datetimeVar);
            Assert.AreEqual(1M, obj2.decimalVar);
            Assert.AreEqual(2f, obj2.floatVar);
            Assert.AreEqual(3d, obj2.doubleVar);
        }



        [TestMethod()]
        public void MapUnexistProperty_Test()
        {
            ObjectConverter objectConverter = new ObjectConverter()
            {
                IsIgnoreNotExistingProperty = true,
            };

            var target = new MapWithoutPropertyTestObject1_Copy();
            target.Name = "QQ";

            MapWithoutPropertyTestObject1 obj = new MapWithoutPropertyTestObject1();

            objectConverter.MapObject(target, obj);
            Assert.AreEqual("QQ", obj.Name);
            Assert.AreEqual(100, obj.Age);
        }

        [TestMethod()]
        [ExpectedException(typeof(FieldNotFoundException))]
        public void MapUnexistPropertyException_Test()
        {
            ObjectConverter objectConverter = new ObjectConverter();

            var target = new MapWithoutPropertyTestObject1_Copy();
            target.Name = "QQ";

            MapWithoutPropertyTestObject1 obj = new MapWithoutPropertyTestObject1();

            objectConverter.MapObject(target, obj);
            Assert.AreEqual("QQ", obj.Name);
            Assert.AreEqual(100, obj.Age);
        }
    }

}