using Windows.Networking.Vpn;
using System.Diagnostics;
using Windows.Networking.Sockets;
using Windows.Networking;
using System;

namespace UniPacketBG
{
    class UniPacketPlugin : IVpnPlugIn
    {
        public void Connect(VpnChannel channel)
        {
            Debug.WriteLine("VPN run ++");
            try
            {
                var tunnelSocket = new DatagramSocket();
                tunnelSocket.ConnectAsync(new HostName("127.0.0.1"), "23123").AsTask().Wait();
                channel.AssociateTransport(tunnelSocket, null);
                channel.Start(
                        new[] { new HostName("66.32.12.12") }, //此处可从服务端获取ip配置 或者直接通过dhcp来设置
                        null,
                        null,
                        new VpnRouteAssignment() { ExcludeLocalSubnets = true },
                        new VpnNamespaceAssignment() { },
                        512,
                        1024,
                        false,
                        tunnelSocket,
                        null
                    );
            }
            catch (System.Exception e) {
                channel.SetErrorMessage(e.Message);
            }
        }

        public void Disconnect(VpnChannel channel)
        {
            
        }

        public void GetKeepAlivePayload(VpnChannel channel, out VpnPacketBuffer keepAlivePacket)
        {
            keepAlivePacket = null;
        }

        public void Encapsulate(VpnChannel channel, VpnPacketBufferList packets, VpnPacketBufferList encapulatedPackets)
        {
            
        }

        public void Decapsulate(VpnChannel channel, VpnPacketBuffer encapBuffer, VpnPacketBufferList decapsulatedPackets, VpnPacketBufferList controlPacketsToSend)
        {
            
        }
    }
}
