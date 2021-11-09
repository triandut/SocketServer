using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace SocketServer
{
    public class ClientBotNet
    {
        public string MacAddress;
        public Socket socket;

        public ClientBotNet(Socket socketClient, string mac)
        {
            socket = socketClient;
            MacAddress = mac;
        }
    }
}
