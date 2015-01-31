using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Moonlit.Diagnostics
{
    /// <summary>
    /// ������չ����
    /// </summary>
    public static class ProcessExtension
    {
        /// <summary>
        /// ��ȡ��һ���������е�ʵ��
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
                throw new InvalidOperationException(string.Format("�޷��ҵ�����{0}��ģ��", currentProcess.Id));
            }
        }
        /// <summary>
        /// �жϵ�ǰ�����Ƿ��Ϲܴ���
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