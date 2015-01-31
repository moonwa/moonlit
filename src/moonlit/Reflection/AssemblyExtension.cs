using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.Serialization;

namespace Moonlit.Reflection
{
    /// <summary>
    /// ���͸�������
    /// </summary>
    public static class AssemblyExtension
    { 
        /// <summary>
        /// ���ݳ���Ŀ¼���س��򼯣����ظ����أ�
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <returns></returns>
        public static Assembly SafeLoadFile(string assemblyFile)
        {
            return Assembly.Load(File.ReadAllBytes(assemblyFile));
        }
        public static string GetLanguage(this Assembly assembly)
        {
            Type attributeType = typeof(NeutralResourcesLanguageAttribute);
            var language = (NeutralResourcesLanguageAttribute)Attribute.GetCustomAttribute(assembly, attributeType);
            return language == null ? "neutral" : language.CultureName;
        }
        public static IEnumerable<Type> GetTypesSafty(this Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(x => x != null);
            }
        }
    }
}