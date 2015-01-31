using System;
using System.Collections.Generic;
using System.Text;

namespace Moonlit.PatternDesign
{
    /// <summary>
    /// 单例模式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : new()
    {
        Singleton() { }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance
        {
            get { return SingletonCreator._instance; }
        }

        /// <summary>
        /// 单例创造者
        /// </summary>
        class SingletonCreator
        {
            static SingletonCreator() { }

            internal static readonly T _instance = new T();
        }
    }
}
