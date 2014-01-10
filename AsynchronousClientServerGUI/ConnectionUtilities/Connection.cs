using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace ConnectionUtilities
{
    public class Connection
    {
        public Socket connectionSocket = null;
        public Label pingLabel = null;
        public Button chatButton = null;
        public Button disconnectButton = null;
        public string name = null;
        public bool successfulPingExecuted = false;
        public string remoteEndPoint = null;

        public Connection(Socket socket)
        {
            connectionSocket = socket;
            remoteEndPoint = socket.RemoteEndPoint.ToString();
        }
    }
}
