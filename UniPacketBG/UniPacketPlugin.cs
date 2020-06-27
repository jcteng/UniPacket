using Windows.Networking.Vpn;
using System.Diagnostics;
using Windows.Networking.Sockets;
using Windows.Networking;
using System;
using System.Collections.Generic;
using Windows.UI.Text;

namespace UniPacketBG
{

    class UniPacketPlugin : IVpnPlugIn
    {
        DatagramSocket tunnelSocket = new DatagramSocket();
        String remoteHost = null;
        private void promoteGUI(VpnChannel channel) {
            channel.RequestCustomPrompt(new List<IVpnCustomPrompt> {
                new VpnCustomTextBox(){Label="this is label",DisplayText="DPT"},
                new VpnCustomEditBox(){Label="AccessKey" },
                new VpnCustomEditBox(){ Label="AccessKey2"},
            });
        }
        public void Connect(VpnChannel channel)
        {
            Debug.WriteLine("UniPacketConnect++");
            try
            {
                channel.LogDiagnosticMessage("Start");
                
                foreach (var shn in channel.Configuration.ServerHostNameList) {
                    Debug.WriteLine("serverHostName->{0}",shn);
                    remoteHost = shn.ToString();
                }
                foreach (var ssn in channel.Configuration.ServerServiceName)
                {
                    Debug.WriteLine("ServerServiceName->{0}", ssn);
                }

                
                tunnelSocket.ConnectAsync(new HostName(remoteHost), "2312").AsTask().Wait();                           
                channel.AssociateTransport(tunnelSocket, null);
                channel.Start(
                        new[] { new HostName("66.32.12.12") }, //此处可从服务端获取ip配置 或者直接通过dhcp来设置，此处需注意随机IP生成不得与本地网络中所有的网段碰撞
                        null, //此处为V6地址指派，用于设置虚拟nic的v6地址，也可通过远端dhcp配置
                        null, //未弄清，此处为绑定的网卡id还是创建的虚拟nic id
                        new VpnRouteAssignment
                        {
                            ExcludeLocalSubnets = true,
                            Ipv4InclusionRoutes = new[] { new VpnRoute(new HostName("66.32.12.0"), 24) }
                        }, //路由表可在此处设置
                        new VpnNamespaceAssignment() { }, //值得研究，此处似乎可以筛选来源和目标APP，此处是支持ProxyAutoConfigUri，可考虑使用js代理脚本
                        512, //NIC设置， MTU
                        1024, //NIC设置， maxFrame 用于ip层拆包策略
                        false, 
                        tunnelSocket, //main tunnel
                        null
                    );
            }
            catch (System.Exception e)
            {
                channel.SetErrorMessage(e.Message);
            }
            Debug.WriteLine("UniPacketConnect--");
        }

        public void Disconnect(VpnChannel channel)
        {
            Debug.WriteLine("Disconnect++");
            Debug.WriteLine("Disconnect--");
        }

        public void GetKeepAlivePayload(VpnChannel channel, out VpnPacketBuffer keepAlivePacket)
        {
            Debug.WriteLine("GetKeepAlivePayload++");
            
            keepAlivePacket = null;
            Debug.WriteLine("GetKeepAlivePayload--");
        }

        public void Encapsulate(VpnChannel channel, VpnPacketBufferList packets, VpnPacketBufferList encapulatedPackets)
        {
            //Debug.WriteLine("Encapsulate++");
            while (packets.Size>0) {
                encapulatedPackets.Append(packets.RemoveAtBegin());
                
            }
            
            //Debug.WriteLine("Encapsulate--");
        }

        public void Decapsulate(VpnChannel channel, VpnPacketBuffer encapBuffer, VpnPacketBufferList decapsulatedPackets, VpnPacketBufferList controlPacketsToSend)
        {
            //Debug.WriteLine("Decapsulate++");
            //Debug.WriteLine("Decapsulate--");
        }
    }
}
