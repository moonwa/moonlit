using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.Mef;
using Moonlit.ObjectConverts;
using Moonlit.ObjectConverts.ObjectConverters;
using Moonlit.ObjectConverts.ObjectReaders;

namespace Moonlit.TestFixtures.ObjectTranslators
{
    [TestClass()]
    public class DictionaryTranslator_Test
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
            var converter = new ObjectConverter();

            DateTime tm = DateTime.Parse("1998-01-01");
            Dictionary<string, object> dt = new Dictionary<string, object>();
            dt.Add("int16Var", 1);
            dt.Add("int32Var", 2);
            dt.Add("int64Var", 3);
            dt.Add("datetimeVar", tm);
            dt.Add("decimalVar", 1m);
            dt.Add("floatVar", 2f);
            dt.Add("doubleVar", 3d);
            dt.Add("stringVar", "hello");
            dt.Add("MyEnum", MyEnum.B);

            MapTestObject1 obj = new MapTestObject1();
            converter.MapObject(dt, obj);
            Assert.AreEqual(1, obj.int16Var);
            Assert.AreEqual(2, obj.int32Var);
            Assert.AreEqual(3, obj.int64Var);
            Assert.AreEqual(tm, obj.datetimeVar);
            Assert.AreEqual(1M, obj.decimalVar);
            Assert.AreEqual(2f, obj.floatVar);
            Assert.AreEqual(3d, obj.doubleVar);
            Assert.AreEqual(MyEnum.B, obj.MyEnum);

            dt["MyEnum"] = "B";
            MapTestObject2 obj2 = new MapTestObject2();
            converter.MapObject(dt, obj2);
            Assert.AreEqual((short)1, obj2.int16Var);
            Assert.AreEqual(2, obj2.int32Var);
            Assert.AreEqual((long)3, obj2.int64Var);
            Assert.AreEqual(tm, obj2.datetimeVar);
            Assert.AreEqual(1M, obj2.decimalVar);
            Assert.AreEqual(2f, obj2.floatVar);
            Assert.AreEqual(3d, obj2.doubleVar);
            Assert.AreEqual(MyEnum.B, obj2.MyEnum);
        }

        [TestMethod()]
        public void MapUnexistProperty_Test()
        {
            ObjectConverter objectConverter = new ObjectConverter()
                {
                    IsIgnoreNotExistingProperty = true,
                };

            Dictionary<string, object> dt = new Dictionary<string, object>();
            dt.Add("Name", "QQ");

            MapWithoutPropertyTestObject1 obj = new MapWithoutPropertyTestObject1();
            objectConverter.MapObject(dt, obj);
            Assert.AreEqual("QQ", obj.Name);
            Assert.AreEqual(100, obj.Age);
        }

        [TestMethod()]
        public void PrivateSetter_Test()
        {
            ObjectConverter objectConverter = new ObjectConverter()
            {
                IsIgnoreNotExistingProperty = true,
            };

            Dictionary<string, object> dt = new Dictionary<string, object>();
            dt.Add("Name", "QQ");

            var obj = new PrivateSetterObject();
            objectConverter.MapObject(dt, obj);
            Assert.IsNull(obj.Name);
        }
        [TestMethod()]
        public void PrivateGetter_Test()
        {
            ObjectConverter objectConverter = new ObjectConverter()
                {
                    IsIgnoreNotExistingProperty = true,
                };

            Dictionary<string, object> dt = new Dictionary<string, object>();
            dt.Add("Name", "QQ");

            var obj = new PrivateGetterObject();
            objectConverter.MapObject(dt, obj);
            Assert.AreEqual("QQ", obj.GetName());
        }
        public class PrivateSetterObject
        {
            public string Name { get; private set; }
        }
        public class PrivateGetterObject
        {
            public string Name { private get; set; }
            public string GetName()
            {
                return this.Name;
            }
        }
        [TestMethod()]
        [ExpectedException(typeof(FieldNotFoundException))]
        public void MapUnexistPropertyException_Test()
        {
            ObjectConverter objectConverter = new ObjectConverter();
            objectConverter.RegisterConverter(1, new CreationDictionaryObjectConverter("xtype", new DomainTypeResolver(true)));

            Dictionary<string, object> dt = new Dictionary<string, object>();
            dt.Add("Name", "QQ");

            MapWithoutPropertyTestObject1 obj = new MapWithoutPropertyTestObject1();
            objectConverter.MapObject(dt, obj);
            Assert.AreEqual("QQ", obj.Name);
            Assert.AreEqual(100, obj.Age);
        }

        [TestMethod()]
        public void DictionObject_Test()
        {
            ObjectConverter objectConverter = new ObjectConverter();
            objectConverter.RegisterConverter(1, new CreationDictionaryObjectConverter("xtype", new DomainTypeResolver(true)));

            Dictionary<string, object> routeValue = new Dictionary<string, object>();
            routeValue["Name"] = "A";

            Dictionary<string, object> xcat = new Dictionary<string, object>();
            xcat["Name"] = "Q";
            routeValue["RouteObjects"] = xcat;

            RouteValue obj = new RouteValue();
            objectConverter.MapObject(routeValue, obj);

            Assert.IsNotNull(obj.RouteObjects);
            Assert.AreEqual("Q", obj.RouteObjects["Name"]);
        }

        [TestMethod()]
        public void ComplexObject_Test()
        {
            ObjectConverter objectConverter = new ObjectConverter();
            objectConverter.RegisterConverter(1, new CreationDictionaryObjectConverter("xtype", new DomainTypeResolver(true)));

            Dictionary<string, object> Peter = new Dictionary<string, object>();
            Peter["Name"] = "Peter";

            List<object> pets = new List<object>();
            Dictionary<string, object> xcat = new Dictionary<string, object>();
            xcat["xtype"] = typeof(Cat).FullName;
            xcat["Name"] = "cat1";
            xcat["Age"] = 3;
            pets.Add(xcat);

            Dictionary<string, object> xdog = new Dictionary<string, object>();
            xdog["xtype"] = typeof(Dog).FullName;
            xdog["Name"] = "dog1";
            xdog["Age"] = 5;
            pets.Add(xdog);

            Peter["Pets"] = pets.ToArray();
            Peter["PetList"] = pets.ToArray();

            Person obj = new Person();
            objectConverter.MapObject(Peter, obj);

            Assert.IsNotNull(obj.Pets);
            Assert.AreEqual(2, obj.Pets.Length);

            var cat = obj.Pets[0] as Cat;
            Assert.IsNotNull(cat);
            Assert.AreEqual("cat1", cat.Name);
            Assert.AreEqual(3, cat.Age);

            var dot = obj.Pets[1] as Dog;
            Assert.IsNotNull(dot);
            Assert.AreEqual("dog1", dot.Name);
            Assert.AreEqual(5, dot.Age);

            // check list
            Assert.IsNotNull(obj.PetList);
            Assert.AreEqual(2, obj.PetList.Count);

            cat = obj.PetList[0] as Cat;
            Assert.IsNotNull(cat);
            Assert.AreEqual("cat1", cat.Name);
            Assert.AreEqual(3, cat.Age);

            dot = obj.PetList[1] as Dog;
            Assert.IsNotNull(dot);
            Assert.AreEqual("dog1", dot.Name);
            Assert.AreEqual(5, dot.Age);
        }
    }


    class RouteValue
    {
        public string Name { get; set; }
        public IDictionary<string, object> RouteObjects { get; set; }
    }
    class Person
    {
        public string Name { get; set; }
        public Animate[] Pets { get; set; }
        public List<Animate> PetList { get; set; }
    }

    class Animate
    {
        public string Name { get; set; }
    }

    class Cat : Animate
    {
        public int Age { get; set; }
    }

    class Dog : Animate
    {
        public int Age { get; set; }
    }
    class MapWithoutPropertyTestObject1
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public MapWithoutPropertyTestObject1()
        {
            Age = 100;
        }
    }
    class MapWithoutPropertyTestObject1_Copy
    {
        public string Name { get; set; }

        public MapWithoutPropertyTestObject1_Copy()
        {
        }
    }

    enum MyEnum
    {
        A, B
    }
    class MapTestObject1_Copy
    {
        // it's a static property, shouldn't work on this property
        public static string StaticName { get; set; }

        public Int16 int16Var { get; set; }
        public Int32 int32Var { get; set; }
        public Int64 int64Var { get; set; }
        public DateTime datetimeVar { get; set; }
        public decimal decimalVar { get; set; }
        public float floatVar { get; set; }
        public double doubleVar { get; set; }
        public string stringVar { get; set; }
        public MyEnum MyEnum { get; set; }
    }
    class MapTestObject1
    {
        // it's a static property, shouldn't work on this property
        public static string StaticName { get; set; }

        public Int16 int16Var { get; set; }
        public Int32 int32Var { get; set; }
        public Int64 int64Var { get; set; }
        public DateTime datetimeVar { get; set; }
        public decimal decimalVar { get; set; }
        public float floatVar { get; set; }
        public double doubleVar { get; set; }
        public string stringVar { get; set; }
        public MyEnum MyEnum { get; set; }
    }
    class MapTestObject2
    {
        public Int16? int16Var { get; set; }
        public Int32? int32Var { get; set; }
        public Int64? int64Var { get; set; }
        public DateTime? datetimeVar { get; set; }
        public decimal? decimalVar { get; set; }
        public float? floatVar { get; set; }
        public double? doubleVar { get; set; }
        public string stringVar { get; set; }
        public MyEnum MyEnum { get; set; }
    }
    class MapTestObject2_Copy
    {
        public Int16? int16Var { get; set; }
        public Int32? int32Var { get; set; }
        public Int64? int64Var { get; set; }
        public DateTime? datetimeVar { get; set; }
        public decimal? decimalVar { get; set; }
        public float? floatVar { get; set; }
        public double? doubleVar { get; set; }
        public string stringVar { get; set; }
    }
}