using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using CSM.API;
using CSM.Util;

namespace CSM.Networking
{
    public static class IpAddress
    {
        public static string GetLocalIpAddress()
        {
            try
            {
                //Create a new socket
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    //Connect to some server to non-listening port
                    socket.Connect(CSM.Settings.ApiServer, 65530);
                    //Get the IP Address (Internal) from the IPEndPoint
                    return socket.LocalEndPoint is IPEndPoint endPoint ? endPoint.Address.ToString() : "";
                }
            }
            catch (Exception)
            {
                //On error return "Not found"
                return "Not found";
            }
        }

        public static string GetExternalIpAddress()
        {
            try
            {
                //Get the External IP address from internet
                return new CSMWebClient().DownloadString($"http://{CSM.Settings.ApiServer}/api/ip");
            }
            catch (Exception e)
            {
                //On error return "Not found"
                Log.Error("Failed to request IP: " + e.Message);
                return "Not found";
            }
        }

        public static string GetVPNIpAddress()
        {
            try
            {
                //Create a new socket
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    // Try to connect to random address in 25.0.0.0/8 subnet used by Hamachi
                    socket.Connect("25.0.0.1", 65530);
                    // If local address used starts with 25., Hamachi is installed and active
                    if (socket.LocalEndPoint is IPEndPoint endPoint && endPoint.Address.GetAddressBytes()[0] == 25)
                    {
                        return endPoint.Address.ToString();
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return null;
        }

        private class CacheEntry
        {
            public IPAddress Address;
            public DateTime Timestamp;
        }

        private static readonly Dictionary<string, CacheEntry> Ipv4Cache = new Dictionary<string, CacheEntry>();
        private static readonly TimeSpan CacheLifetime = TimeSpan.FromMinutes(5);

        public static IPAddress GetIpv4(string host)
        {
            lock (Ipv4Cache)
            {
                if (Ipv4Cache.TryGetValue(host, out CacheEntry entry))
                {
                    if (DateTime.UtcNow - entry.Timestamp < CacheLifetime)
                    {
                        return entry.Address;
                    }

                    Ipv4Cache.Remove(host);
                }

                IPAddress address = Dns.GetHostEntry(host).AddressList.FirstOrDefault(resolveAddress => resolveAddress.AddressFamily == AddressFamily.InterNetwork);
                Ipv4Cache[host] = new CacheEntry { Address = address, Timestamp = DateTime.UtcNow };
                return address;
            }
        }
    }
}
