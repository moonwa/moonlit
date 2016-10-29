using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moonlit.Collections;

namespace Moonlit.Configuration
{
    public class Hosts
    {
        class HostEntry
        {

            public string IP { get; set; }
            public string Host { get; set; }

            public static HostEntry Create(string s)
            {
                var arr = s.Split(' ', '\t');
                if (arr.Length < 2)
                {
                    return null;
                }

                var ip = arr[0];
                if (string.IsNullOrWhiteSpace(ip))
                {
                    return null;
                }

                var host = arr[1];
                if (string.IsNullOrWhiteSpace(host))
                {
                    return null;
                }
                return new HostEntry
                {
                    Host = host.Trim(),
                    IP = ip.Trim()
                };
            }
        }
        public static Hosts Open()
        {
            var windir = Environment.GetEnvironmentVariable("windir");
            var hosts = new Hosts();
            hosts.Open($"{windir}\\system32\\drivers\\etc\\hosts");
            return hosts;
        }
        IList<HostEntry> _hostEntries = new List<HostEntry>();
        public string FilePath { get; set; }

        private Hosts()
        {

        }

        private string _fileName;
        public void Open(string file)
        {
            _fileName = file;
            var lines = File.ReadAllLines(file);
            _hostEntries = lines.Where(x => !x.StartsWith("#") && !string.IsNullOrEmpty(x))
                .Select(x => HostEntry.Create(x))
                .Where(x => x != null)
                .ToList();
        }

        public void SetHost(string host, string ip)
        {
            var hostEntry = GetHostEntry(host);
            if (string.IsNullOrWhiteSpace(ip))
            {
                _hostEntries.Remove(hostEntry);
            }
            else
            {
                hostEntry.IP = ip;
            }
        }

        private HostEntry GetHostEntry(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
            {
                throw new ArgumentNullException(nameof(ip));
            }

            ip = ip?.Trim() ?? string.Empty;

            var hostEntry = _hostEntries.FirstOrDefault(x => x.Host == ip);

            if (hostEntry == null)
            {
                hostEntry = new HostEntry { Host = ip };
                _hostEntries.Add(hostEntry);
            }
            return hostEntry;
        }

        public string GetIP(string host)
        {
            var hostEntry = GetHostEntry(host);
            return hostEntry.IP;
        }

        public void Save()
        {
            var lines = _hostEntries.Where(x=>!string.IsNullOrWhiteSpace(x.IP))
                .Select(x => $"{x.IP} {x.Host}");
            File.WriteAllLines(_fileName, lines);
        }
    }
}
