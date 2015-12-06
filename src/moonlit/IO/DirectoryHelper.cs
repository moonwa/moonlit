using System.Collections.Generic;
using System.IO;

namespace Moonlit.IO
{
  
    public static class DirectoryHelper
    {
        public static void EnsureFromFile(string filename)
        {
            var path = Path.GetDirectoryName(filename);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        public static void Ensure(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        public static void CopyTo(this DirectoryInfo di, string to, string searchParttern, bool recursive)
        {
            Ensure(to);
            foreach (var fileInfo in di.GetFiles(searchParttern))
            {
                var destFile = Path.Combine(to, fileInfo.Name);
                if (File.Exists(destFile))
                {
                    FileInfo fi = new FileInfo(destFile);
                    fi.RemoveAttribute(FileAttributes.ReadOnly);
                    fi.RemoveAttribute(FileAttributes.System);
                }
                fileInfo.CopyTo(destFile, true);
            }
            if (recursive)
                foreach (DirectoryInfo child in di.GetDirectories())
                {
                    child.CopyTo(Path.Combine(to, child.Name), searchParttern, recursive);
                }
        }
        public static void Delete(this DirectoryInfo di, string searchParttern, bool recursive)
        {
            foreach (var fileInfo in di.GetFiles(searchParttern))
            {
                fileInfo.RemoveAttribute(FileAttributes.ReadOnly);
                fileInfo.RemoveAttribute(FileAttributes.System);
                fileInfo.Delete();
            }
            if (recursive)
                foreach (DirectoryInfo child in di.GetDirectories())
                {
                    child.Delete(searchParttern, recursive);
                }
        }
    }
}