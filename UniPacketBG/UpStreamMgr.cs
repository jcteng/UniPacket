using System;
using Windows.Networking;

namespace UniPacketBG
{
    class UpStreamMgr
    {
        public UpStreamMgr() {
            var tunnelSocket = new Windows.Networking.Sockets.DatagramSocket();
            tunnelSocket.ConnectAsync(new HostName("127.0.0.1"), "23123").AsTask().Wait();
        }
        private object upstream;
        public object GetUpstream() {
            return upstream;
        }
    }
}
