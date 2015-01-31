using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Moonlit.PatternDesign
{
    /// <summary>
    /// 单实例进程
    /// </summary>
    public static class SingletonProcess
    {
        private static Mutex _mutex = null;
        /// <summary>
        /// 运行参数
        /// </summary>
        /// <typeparam name="TSuccess">The type of the success.</typeparam>
        /// <param name="success">The success.</param>
        /// <param name="successarg">The successarg.</param>
        public static void Run<TSuccess>(Action<TSuccess> success, TSuccess successarg)
        {
            Run<TSuccess, object>(success, successarg, null, null);
        }
        /// <summary>
        /// Runs the specified success.
        /// </summary>
        /// <typeparam name="TSuccess">The type of the success.</typeparam>
        /// <typeparam name="TFail">The type of the fail.</typeparam>
        /// <param name="success">The success.</param>
        /// <param name="successarg">The successarg.</param>
        /// <param name="fail">The fail.</param>
        /// <param name="failarg">The failarg.</param>
        public static void Run<TSuccess, TFail>(Action<TSuccess> success, TSuccess successarg, Action<TFail> fail, TFail failarg)
        {
            if (CreateMutex())
            {
                try
                {
                    success(successarg);
                }
                finally
                {
                    Release();
                }
            }
            else
            {
                if (null != fail)
                {
                    fail(failarg);
                }
            }
        }

        private static void Release()
        {
            if (_mutex != null)
            {
                _mutex.Close();
            }
        }

        private static bool CreateMutex()
        {
            return CreateMutex(Assembly.GetEntryAssembly().FullName);
        }

        private static bool CreateMutex(string name)
        {
            bool result = false;
            _mutex = new Mutex(true, name, out result);
            return result;
        }
    }
}
