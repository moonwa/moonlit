using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace Moonlit.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public static class Eval
    {
        /// <summary>
        /// 计算字符串表达式
        /// </summary>
        /// <param name="cCharpCode">The c charp code.</param>
        /// <returns></returns>
        public static object EvalString(string cCharpCode)
        {
            CSharpCodeProvider csharpCodeProvider = new CSharpCodeProvider();
#pragma warning disable 0618
            ICodeCompiler compiler = csharpCodeProvider.CreateCompiler();

            CompilerParameters cp = new CompilerParameters();
            cp.ReferencedAssemblies.Add("system.dll");
            cp.CompilerOptions = "/t:library";
            cp.GenerateInMemory = true;

            string code = CreateCode(cCharpCode);
            CompilerResults cr = compiler.CompileAssemblyFromSource(cp, code);
            Assembly assembly = cr.CompiledAssembly;
            object tmp = assembly.CreateInstance("myLib");
            Type type = tmp.GetType();
            MethodInfo mi = type.GetMethod("myMethod");
            object result = mi.Invoke(tmp, null);
            return result;
        }
        /// <summary>
        /// 计算字符串表达式
        /// </summary>
        /// <param name="cCharpCode"></param>
        /// <returns></returns>
        public static T EvalString<T>(string cCharpCode)
        {
            return (T)EvalString(cCharpCode);
        }
        private static string CreateCode(string cCharpCode)
        {
            StringBuilder myCode = new StringBuilder();
            myCode.Append("using System;"); ;
            myCode.Append("class myLib {public object myMethod(){object obj=" + cCharpCode + ";return obj;}}");
            string code = myCode.ToString();
            return code;
        }
    }
}
