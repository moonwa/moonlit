using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using VSLangProj;

namespace Moonlit.VisualStudio
{
    public static class ProjectHelper
    {
        private static readonly Type _serviceProviderType =
            Type.GetType(
                "Microsoft.VisualStudio.Shell.ServiceProvider, Microsoft.VisualStudio.Shell, Version=11.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")
            ??
            Type.GetType(
                "Microsoft.VisualStudio.Shell.ServiceProvider, Microsoft.VisualStudio.Shell, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");

        public const int S_OK = 0;
        public const string WebApplicationProjectTypeGuid = "{349C5851-65DF-11DA-9384-00065B846F21}";
        public const string WebSiteProjectTypeGuid = "{E24C65DC-7377-472B-9ABA-BC803B73C61A}";
        public const string VsProjectItemKindPhysicalFolder = "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}";

        public static string GetTargetName(this Project project)
        {
            return project.GetPropertyValue<string>("AssemblyName");
        }

        public static string GetProjectDir(this Project project)
        {
            return project.GetPropertyValue<string>("FullPath");
        }

        public static string GetTargetDir(this Project project)
        {
            var fullPath = project.GetProjectDir();

            var outputPath
                = project.IsWebSiteProject()
                      ? "Bin"
                      : project.GetConfigurationPropertyValue<string>("OutputPath");

            return Path.Combine(fullPath, outputPath);
        }

        /// <summary>
        ///     Gets the string abbreviation for the language of a VS project.
        /// </summary>
        public static string GetLanguage(this Project project)
        {
            switch (project.CodeModel.Language)
            {
                case CodeModelLanguageConstants.vsCMLanguageVB:
                    return "vb";

                case CodeModelLanguageConstants.vsCMLanguageCSharp:
                    return "cs";
            }

            return null;
        }

        /// <summary>
        ///     Gets the root namespace configured for a VS project.
        /// </summary>
        public static string GetRootNamespace(this Project project)
        {
            return project.GetPropertyValue<string>("RootNamespace");
        }

        public static string GetFileName(this Project project, string projectItemName)
        {
            ProjectItem projectItem;

            try
            {
                projectItem = project.ProjectItems.Item(projectItemName);
            }
            catch
            {
                return Path.Combine(project.GetProjectDir(), projectItemName);
            }

            return projectItem.FileNames[0];
        }

        public static bool IsWebProject(this Project project)
        {
            return project.GetProjectTypes().Any(
                g => string.Equals(WebApplicationProjectTypeGuid, g, StringComparison.OrdinalIgnoreCase)
                     || string.Equals(WebSiteProjectTypeGuid, g, StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsWebSiteProject(this Project project)
        {
            return project.GetProjectTypes().Any(g => string.Equals(WebSiteProjectTypeGuid, g, StringComparison.OrdinalIgnoreCase));
        }

        public static void EditFile(this Project project, string path)
        {
            var absolutePath = Path.Combine(project.GetProjectDir(), path);
            var dte = project.DTE;

            if (dte.SourceControl != null
                && dte.SourceControl.IsItemUnderSCC(absolutePath)
                && !dte.SourceControl.IsItemCheckedOut(absolutePath))
            {
                dte.SourceControl.CheckOutItem(absolutePath);
            }
        }

        public static void AddFile(this Project project, string path, string contents)
        {
            var absolutePath = Path.Combine(project.GetProjectDir(), path);

            project.EditFile(path);
            Directory.CreateDirectory(Path.GetDirectoryName(absolutePath));
            File.WriteAllText(absolutePath, contents);

            project.AddFile(path);
        }

        public static void AddFile(this Project project, string path)
        {
            var directory = Path.GetDirectoryName(path);
            var fileName = Path.GetFileName(path);
            var projectDir = project.GetProjectDir();
            var absolutePath = Path.Combine(projectDir, path);

            var projectItems
                = directory
                    .Split(Path.DirectorySeparatorChar)
                    .Aggregate(
                        project.ProjectItems,
                        (pi, dir) =>
                        {
                            projectDir = Path.Combine(projectDir, dir);

                            try
                            {
                                var projectItem = pi.Item(dir);

                                return projectItem.ProjectItems;
                            }
                            catch
                            {
                            }

                            return pi.AddFromDirectory(projectDir).ProjectItems;
                        });

            try
            {
                projectItems.Item(fileName);
            }
            catch
            {
                projectItems.AddFromFileCopy(absolutePath);
            }
        }

        public static bool TryBuild(this Project project)
        {
            var dte = project.DTE;
            var configuration = dte.Solution.SolutionBuild.ActiveConfiguration.Name;

            dte.Solution.SolutionBuild.BuildProject(configuration, project.UniqueName, true);

            return dte.Solution.SolutionBuild.LastBuildInfo == 0;
        }

        private static T GetPropertyValue<T>(this Project project, string propertyName)
        {
            var property = project.Properties.Item(propertyName);

            if (property == null)
            {
                return default(T);
            }

            return (T)property.Value;
        }

        private static T GetConfigurationPropertyValue<T>(this Project project, string propertyName)
        {
            var property = project.ConfigurationManager.ActiveConfiguration.Properties.Item(propertyName);

            if (property == null)
            {
                return default(T);
            }

            return (T)property.Value;
        }

        public static IEnumerable<string> GetProjectTypes(this Project project)
        {
            IVsHierarchy hierarchy;

            var serviceProvider = (IServiceProvider)Activator.CreateInstance(
                _serviceProviderType,
                (Microsoft.VisualStudio.OLE.Interop.IServiceProvider)project.DTE);

            var solution = (IVsSolution)serviceProvider.GetService(typeof(IVsSolution));
            var hr = solution.GetProjectOfUniqueName(project.UniqueName, out hierarchy);

            if (hr != S_OK)
            {
                Marshal.ThrowExceptionForHR(hr);
            }

            string projectTypeGuidsString;

            var aggregatableProject = (IVsAggregatableProject)hierarchy;
            hr = aggregatableProject.GetAggregateProjectTypeGuids(out projectTypeGuidsString);

            if (hr != S_OK)
            {
                Marshal.ThrowExceptionForHR(hr);
            }

            return projectTypeGuidsString.Split(';');
        }

        public static Reference GetReference(this Project project, SelectedItem selectedItem)
        {
            var vsProject = (VSProject)project.Object;
            foreach (Reference reference in vsProject.References)
            {
                if (reference.Name == selectedItem.Name)
                    return reference;
            }
            return null;
        }
    }
}
