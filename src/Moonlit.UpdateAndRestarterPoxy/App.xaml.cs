using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Moonlit.UpdateAndRestarterPoxy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string DownloadUrl { get; set; }
        public static string UpdatePath { get; set; }
        public static string StartApp { get; set; }
        public static string IgnorePath { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            DownloadUrl = e.Args[0];
            UpdatePath = e.Args[1];
            StartApp = e.Args[2];
            if (e.Args.Length > 3)
            {
                IgnorePath = e.Args[3];
            }
            base.OnStartup(e);
        }
    }
}
