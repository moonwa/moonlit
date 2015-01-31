using System;

namespace Moonlit.DirectoryServices.IIS
{
    public class ServerBinding
    {
        public ServerBinding(string propertieValue)
        {
            // ":8013:aaa,172.18.2.5:8014:bbb"
            var values = propertieValue.Split(new[] { ":" }, StringSplitOptions.None);
            this.IpAddress = string.IsNullOrEmpty(values[0]) ? "0.0.0.0" : values[0];
            this.Port = Convert.ToInt32(values[1]);
            this.HostName = values[2];
        }

        public ServerBinding(string ipAddress, int port, string hostName)
        {
            IpAddress = ipAddress;
            Port = port;
            HostName = hostName;
        }
        public override string ToString()
        {
            return string.Join(":", new[]
                                        {
                                            this.IpAddress == "0.0.0.0" ? "" : this.IpAddress,
                                            Port.ToString(),
                                            HostName??"",
                                        });
        }

        public string IpAddress { get; set; }
        public int Port { get; set; }
        public string HostName { get; set; }
        public string Schema { get; set; }
    }
}