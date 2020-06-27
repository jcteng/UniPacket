const dgram = require('dgram');
const server = dgram.createSocket('udp4');

server.on('error', (err) => {
  console.log(`server error:\n${err.stack}`);
  server.close();
});

const protocolTable = {
    1:"ICMP",
    2:"IGMP",
    6:"TCP",
    17:"UDP",
}
function getProtocolInfo(protId){
    
    return protocolTable[protId]||protId

}

function parseIPHeader(packetV4n6) {

  if (((packetV4n6[0] >> 4) & 0xf) == 4) {
    return {
      version: (packetV4n6[0] >> 4) & 0xf,
      IHL: 4 * ((packetV4n6[0] >> 0) & 0xf),
      dscp: (packetV4n6[1]>>2)&0x3f,
      ecn: (packetV4n6[1]>>6)&0x3,
      totalLen: (packetV4n6[2]<<8|packetV4n6[3]),
      ident: (packetV4n6[4]<<8|packetV4n6[5]),
      flags:{
          DF:(packetV4n6[6]>>6)&0x1,
          MF:(packetV4n6[6]>>5)&0x1
      }, 
      fragOffset:(((packetV4n6[6])&0x1f)<<8)|packetV4n6[7],
      ttl:packetV4n6[8],
      protocl:getProtocolInfo(packetV4n6[9]),
      HeadCS:(packetV4n6[10]<<8|packetV4n6[11]),
      srcAddr:`${packetV4n6[12]}.${packetV4n6[13]}.${packetV4n6[14]}.${packetV4n6[15]}`,
      destAddr:`${packetV4n6[16]}.${packetV4n6[17]}.${packetV4n6[18]}.${packetV4n6[19]}`
    }


  }
  else {
    return {
      version: (packetV4n6[0] >> 4) & 0xf,
      IHL: 4 * ((packetV4n6[0] >> 0) & 0xf),
    }
  }
}
server.on('message', (msg, rinfo) => {
  console.log(`server got: ${Buffer.from(msg).toString("hex")} from ${rinfo.address}:${rinfo.port}`);
  try{
    console.log(parseIPHeader(Buffer.from(msg)))
  }catch(e){

  }
});

server.on('listening', () => {
  const address = server.address();
  console.log(`server listening ${address.address}:${address.port}`);
});

server.bind(2312,"0.0.0.0");
