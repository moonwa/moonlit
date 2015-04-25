using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Moonlit.UpdateAndRestarterPoxy
{
    public class MainWindowModel : BaseObject
    {
        private bool _isBusy = false;

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }
        private string _status;

        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }
        public DelegateCommand<object> UpdateCommand { get; set; }

        public MainWindowModel()
        {
            Status = "点击更新按钮，准备更新";

            this.UpdateCommand = new DelegateCommand<object>(ExecuteUpdatae, CanExecuteUpdate, "更新");
            this.DoneCommand = new DelegateCommand<object>(ExecuteDone, CanExecuteDone, "完成");
        }

        public DelegateCommand<object> DoneCommand { get; set; }

        private bool _done = false;
        private bool CanExecuteDone(object arg)
        {
            return _done;
        }

        private void ExecuteDone(object obj)
        {
            Process.Start(App.StartApp);
            App.Current.Shutdown(0);
        }

        private bool CanExecuteUpdate(object arg)
        {
            return !_done && !IsBusy;
        }

        private async void ExecuteUpdatae(object obj)
        {
            try
            {
                IsBusy = true;
                UpdateCommand.RaiseCanExecuteChanged();
                byte[] buffer = await DownloadFile();
                CleanFiles();
                ExtraFiles(buffer);
                _done = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                IsBusy = false;
                DoneCommand.RaiseCanExecuteChanged();
                UpdateCommand.RaiseCanExecuteChanged();
            }
        }

        private void ExtraFiles(byte[] buffer)
        {
            this.Status = "Download files completed. \r\n Clean files Completed.\r\n Extracting files";
            ZipArchive zip = new ZipArchive(new MemoryStream(buffer));
            zip.ExtractToDirectory(App.UpdatePath);
        }

        private void CleanFiles()
        {
            this.Status = "Download files completed. \r\n Cleaning files.";
            var ignoreNames = App.IgnorePath.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (FileSystemInfo fse in new DirectoryInfo(App.UpdatePath).GetFileSystemInfos())
            {
                if (ignoreNames.Any(x => string.Equals(x, fse.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }
                var fi = fse as FileInfo;
                if (fi != null)
                {
                    fi.Delete();
                }
                var di = fse as DirectoryInfo;
                if (di != null)
                {
                    di.Delete(true);
                }
            }
            this.Status = "Download files completed. \r\n Clean files Completed.";
        }

        private async Task<byte[]> DownloadFile()
        {
            this.Status = "Downloading files.";
            int size = HttpHelper.FileSize(App.DownloadUrl);
            using (HttpClient client = new HttpClient())
            {
                var stream = await client.GetStreamAsync(App.DownloadUrl).ConfigureAwait(false);

                byte[] buffer = new byte[size];
                int readLength = 0;
                while (readLength < size)
                {
                    readLength += await stream.ReadAsync(buffer, readLength, size - readLength).ConfigureAwait(false);
                    this.Status = string.Format("Downloading files {0} / {1}", readLength, size);
                }
                this.Status = "Download files completed.";
                return buffer;
            }
        }
    }

    public class HttpHelper
    {
        public static int FileSize(string url)
        {
            System.Net.WebRequest req = System.Net.HttpWebRequest.Create(url);
            req.Method = "HEAD";
            using (System.Net.WebResponse resp = req.GetResponse())
            {
                int ContentLength;
                if (int.TryParse(resp.Headers.Get("Content-Length"), out ContentLength))
                {
                    return ContentLength;
                }
            }
            return 0;
        }
    }
}
