using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Moonlit.UpdateAndRestarter
{
    public class Updater
    {
        public static void UpdateAndRestart(string downloadUrl, string[] keepFiles)
        {
            var kv = string.Join(",", keepFiles);
            var startApp = Assembly.GetEntryAssembly().Location;
            var updateFolder = Path.GetDirectoryName(startApp);

            var tmpFileName = Path.GetTempFileName() + ".exe";
            var stream =
                typeof(Updater).Assembly.GetManifestResourceStream(
                    "Moonlit.UpdateAndRestarter.Moonlit.UpdateAndRestarterPoxy.exe");
            var buffer = new byte[stream.Length];

            var readCnt = 0;
            while (readCnt < stream.Length)
            {
                readCnt += stream.Read(buffer, readCnt, (int)(stream.Length - readCnt));
            }
            File.WriteAllBytes(tmpFileName, buffer);
            Process.Start(tmpFileName,
                string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" ", downloadUrl, updateFolder, startApp, kv));
        }
    }
}
