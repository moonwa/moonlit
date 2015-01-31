using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Moonlit.Diagnostics
{
    /// <summary>
    /// 进程扩展方法
    /// </summary>
    public static class ProcessExtension
    {
        /// <summary>
        /// 获取另一个正在运行的实例
        /// </summary>
        /// <param name="currentProcess"></param>
        /// <returns></returns>
        public static List<Process> GetOtherInstance(this Process currentProcess)
        {
            if (currentProcess.MainModule != null)
            {
                string currentFileName = currentProcess.MainModule.FileName;
                Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);

                var ps = from p in processes
                         where p.MainModule != null
                         && p.MainModule.FileName == currentFileName && p.Id != currentProcess.Id
                         select p;

                return ps.ToList();
            }
            else
            {
                throw new InvalidOperationException(string.Format("无法找到进程{0}主模块", currentProcess.Id));
            }
        }
        /// <summary>
        /// 判断当前进程是否拖管代码
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool IsManaged(this Process instance)
        {
            try
            {
                foreach (ProcessModule module in instance.Modules)
                {
                    if (string.Compare(module.ModuleName, "mscorlib.dll", StringComparison.InvariantCultureIgnoreCase) == 0 ||
                           string.Compare(module.ModuleName, "mscorlib.ni.dll", StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        var assemblyName = AssemblyName.GetAssemblyName(module.FileName);
                        if (assemblyName != null && assemblyName.Version.Major >= 2)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}