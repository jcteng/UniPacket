# UniPacket
A IP packet capture tool for Windows10.

## 目的
Win上如需要TAP/TUN应用，目前已知的手段只有有OpenVPN的Driver。
为避免使用第三方签名的驱动程序通过UWP的VPNPlugin接口来加载SSPD虚拟网卡。

## 限制
1. 仅可用于UWP平台，WIN7或其他更低版本不支持。
2. 对于使用同类技术的实现需要通过微软的Restricted capability approval process。
3. 托管结构，性能较差。


