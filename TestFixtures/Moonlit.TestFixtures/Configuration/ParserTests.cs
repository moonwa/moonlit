
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moonlit.Configuration.ConsoleParameter;

namespace Moonlit.TestFixtures.Configuration
{
    /// <summary>
    ///这是 ParserTest 的测试类，旨在
    ///包含所有 ParserTest 单元测试
    ///</summary>
    [TestClass()]
    public class ParserTests
    {
        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        ///Parse 的测试
        ///</summary>
        [TestMethod()]
        public void Parse_String_Test()
        {
            string[] arr = Parser.Parse(@"a ""c:\a b"" /d . """"");
            //Assert.AreEqual(5, arr.Length);
            Assert.AreEqual("a", arr[0]);
            Assert.AreEqual(@"c:\a b", arr[1]);
            Assert.AreEqual(@"/d", arr[2]);
            Assert.AreEqual(@".", arr[3]);
            Assert.AreEqual(@"", arr[4]);
        }
        enum MyEnum
        {
            A,
            B,
            C
        }

        /// <summary>
        ///A test for Parse
        ///</summary>
        [TestMethod()]
        public void Parse_Test()
        {
            string Argument_Destination = "destination";
            string Argument_Source = "source";
            string Argument_Archive = "archive";
            string Argument_Length = "length";
            string Argument_LastWriteTime = "lastwritetime";
            string Argument_CleanArchive = "cleanarchive";

            Parser arg = new Parser();

            arg.AddArguments(new ValueParameter(Argument_Destination, "目标路径", true, (ShortPrefix)'d', (LongOrSplitPrefix)"dest"),
               new ValueParameter(Argument_Source, "被比对路径", true, (ShortPrefix)'s', (LongOrSplitPrefix)"source"),
               new DefinitionParameter(Argument_Length, "按文件长度是否一致来进行同步", (LongPrefix)"len", (ShortPrefix)'l'),
               new DefinitionParameter(Argument_LastWriteTime, "按最后修改时间是否一致来进行同步", (LongPrefix)"time", (ShortPrefix)'t'),
               new DefinitionParameter(Argument_CleanArchive, "是否清除存档标识", (LongPrefix)"clean", (ShortPrefix)'c'),
               new DefinitionParameter(Argument_Archive, "按是否存档来进行同步", (LongPrefix)"archive", (ShortPrefix)'a'))
               ;
            arg.Parse(new string[] {
                "mwwtools" ,"io" ,"synch" ,@"/s", @"c:\a" ,@"/dest:c:\b" ,"/length" ,"-t"
            });

            Assert.IsTrue(arg.GetEntity<ValueParameter>(Argument_Source).Defined);
            Assert.IsFalse(arg.GetEntity<DefinitionParameter>(Argument_Archive).Defined);
            Assert.IsTrue(arg.GetEntity<DefinitionParameter>(Argument_LastWriteTime).Defined);
        }

        [TestMethod]
        public void Parse_Test1()
        {
            var args = new IParameterEntity[]{
                new DefinitionParameter("char", "测试", (LongOrSplitPrefix)"char", (ShortPrefix)'c'),
                new ValueParameter("better", "好的", true, (LongOrSplitPrefix)"bet",(ShortPrefix)'b'),
                new EnumParameter<MyEnum>("em", MyEnum.B, (LongOrSplitPrefix)"enum", (ShortPrefix)'e')
            };
            Parser target = new Parser();
            target.AddArguments(args);

            string[] argStrings = new string[] { "-char", "cvalue", "/bet:bvalue", "--enum", "c" };
            target.Parse(argStrings);
            Assert.AreEqual(false, (target.GetEntity<DefinitionParameter>("char").Defined));
            Assert.AreEqual("bvalue", (target.GetEntity<ValueParameter>("better").Value));
            Assert.AreEqual(MyEnum.C, (target.GetEntity<EnumParameter<MyEnum>>("em").Value));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ErrorTest()
        {
            Parser target = new Parser();
            IParameterEntity[] args = new IParameterEntity[]{
                new DefinitionParameter("char", "测试", (LongOrSplitPrefix)"char", (ShortPrefix)'c'),
                new ValueParameter("better", "好的", true, (LongOrSplitPrefix)"bet",(ShortPrefix)'b'),
                new EnumParameter<MyEnum>("em", MyEnum.B, (LongOrSplitPrefix)"enum", (ShortPrefix)'e')
            };

            target.AddArguments(args);

            target.Parse(new string[] { });
        }
    }
}
