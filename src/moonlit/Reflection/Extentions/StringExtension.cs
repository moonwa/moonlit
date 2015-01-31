using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moonlit.Reflection.Extentions
{
    /// <summary>
    /// 
    /// </summary>
   public static  class StringExtension
    {
       /// <summary>
       /// Evals the specified STR.
       /// </summary>
       /// <param name="str">The STR.</param>
       /// <returns></returns>
       public static object Eval(this string str)
       {
           return Moonlit.Reflection.Eval.EvalString(str);
       }
       /// <summary>
       /// Evals the specified STR.
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="str">The STR.</param>
       /// <returns></returns>
       public static T Eval<T>(this string str)
       {
           return Moonlit.Reflection.Eval.EvalString<T>(str);
       }
    }
}
