using System;
using System.Collections.Generic;
using System.Text;

namespace Moonlit.PatternDesign
{
    /// <summary>
    /// ����ģʽ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : new()
    {
        Singleton() { }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance
        {
            get { return SingletonCreator._instance; }
        }

        /// <summary>
        /// ����������
        /// </summary>
        class SingletonCreator
        {
            static SingletonCreator() { }

            internal static readonly T _instance = new T();
        }
    }
}
