using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
using ConnectionUtilities;

namespace Server
{
    public partial class ServerForm : Form
    {
        delegate bool delBoolSocket(Socket s);
        delegate bool delBoolConnection(Connection c);
        delegate void delVoidConnection(Connection c);
        delegate void delVoidConnectionObject(Connection c, object o);
        delegate void delVoidConnectionString(Connection c, string s);
        delegate void delVoidString(string s);
        delegate void delVoidStringString(string s1, string s2);
        delegate void delVoidSocket(Socket s);
        delegate void delVoidSocketObject(Socket s, object o);

        Socket serverListeningSocket = null;
        List<Connection> connections = new List<Connection>();
        bool serverUp = false;

        public ServerForm()
        {
            InitializeComponent();
            UpdateConnectedClientsLabel();
        }

        private void startServerButton_Click(object sender, EventArgs e)
        {
            if (!serverUp)
            {
                serverListeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    serverListeningSocket.Bind(new IPEndPoint(IPAddress.Parse(ipAddressTextBox.Text), int.Parse(portNumberTextBox.Text)));
                }
                catch
                {
                    return;
                }

                serverListeningSocket.Listen(10);

                StartNewThread(AcceptConnection, serverListeningSocket);

                serverStatusLabel.Text = "Server is up. (" + serverListeningSocket.LocalEndPoint.ToString() + ")";
                AppendToLog("Server has been brought up.");
                UpdateConnectedClientsLabel();

                startServerButton.Text = "Stop Server";
                ipAddressTextBox.Enabled = false;
                portNumberTextBox.Enabled = false;

                serverUp = true;
            }
            else
            {
                startServerButton.Text = "Start Server";
                serverStatusLabel.Text = "Server is down";
                AppendToLog("Server has been shut down.");
                ipAddressTextBox.Enabled = true;
                portNumberTextBox.Enabled = true;

                for (int i = connections.Count - 1; i >= 0; i--)
                {
                    BootClient(connections[i]);
                }

                serverListeningSocket.Close();

                serverUp = false;

            }
        }

        private void StartNewThread(ParameterizedThreadStart threadStart, object parameter)
        {
            Thread thread = new Thread(threadStart);
            thread.IsBackground = true;
            thread.Start(parameter);
        }

        private void UpdateConnectedClientsLabel()
        {
            connectedClientsLabel.Text = "Clients Connected: " + connections.Count;
        }

        private void AcceptConnection(object socket)
        {
            Socket listeningSocket = (Socket)socket;
            Socket connectedSocket = null;

            while (true)
            {
                try
                {
                    connectedSocket = listeningSocket.Accept();
                }
                catch
                {
                    //MessageBox.Show(ex.Message, ex.Source);
                    break;
                }

                Connection connection = new Connection(connectedSocket);
                connections.Add(connection);
                Invoke(new delVoidConnection(HandleAcceptedConnection), connection);

            }
        }

        private void HandleAcceptedConnection(Connection connection)
        {
            UpdateConnectedClientsLabel();

            sendAllButton.Enabled = true;

            //add a button that will disconnect the client
            Button button = new Button();
            connection.disconnectButton = button;
            button.Text = "Boot";
            button.Tag = connection;
            button.AutoSize = true;
            button.Click += new EventHandler(bootButton_Click);
            button.Location = new Point(connectedClientsLabel.Location.X, connectedClientsLabel.Location.Y + connectedClientsLabel.Size.Height + 10 + ((connections.Count - 1) * button.Height));
            this.Controls.Add(button);

            //add a button that will send a message to the client
            button = new Button();
            connection.chatButton = button;
            button.Text = "";
            button.Tag = connection;
            button.AutoSize = true;
            button.Click += new EventHandler(sendMessageButton_Click);
            button.Location = new Point(connection.disconnectButton.Location.X + connection.disconnectButton.Width + 5, connection.disconnectButton.Location.Y);
            this.Controls.Add(button);

            //create a ping label for the client
            Label pingLabel = new Label();
            connection.pingLabel = pingLabel;
            pingLabel.Location = new Point(button.Location.X + button.Width + 5, button.Location.Y);
            pingLabel.Text = "ping";
            this.Controls.Add(pingLabel);

            //be able to receive messages from this client
            StartNewThread(ReceiveThread, connection);

            //initiate ping with client
            StartNewThread(PingClientThread, connection);

            StartNewThread(RequestClientInformationThread, connection);
            //RequestClientInformation(connection);
        }



        void bootButton_Click(object sender, EventArgs e)
        {
            BootClient(connections.Find(x => x.disconnectButton == (Button)sender));
        }

        void sendMessageButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Connection connection = connections.Find(x => x.chatButton == (Button)sender);
            if (connection != null)
            {
                if (connection.name != null)
                {
                    try
                    {
                        Send(connection, new SocketData(SocketData.DataType.Chat, messageTextBox.Text));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }

                    AppendToLog("[" + connection.name + "]: " + messageTextBox.Text);
                    messageTextBox.Text = "";
                }
                else
                {
                    RequestClientInformationThread(connection);
                }
            }
        }

        private void PingClientThread(object connectionIn)
        {
            Connection connection = (Connection)connectionIn;

            while (connection.connectionSocket.Connected && (bool)Invoke(new delBoolConnection(RequestPing), connection))
            {
                Thread.Sleep(1000);
            }
        }

        void BootClient(Connection connection)
        {
            try
            {
                Send(connection, new SocketData(SocketData.DataType.Boot, null));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        bool RequestPing(Connection connection)
        {

            try
            {
                Send(connection, new SocketData(SocketData.DataType.RequestPing, Stopwatch.GetTimestamp()));
                return true;
            }
            catch
            {
                //try and do nothing
                //AppendToLog("a client has disconnected");
                return false;
            }
        }

        void Send(Connection connection, SocketData socketFrame)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, socketFrame);

            connection.connectionSocket.Send(ms.GetBuffer(), (int)ms.Length, SocketFlags.None);
        }

        void RequestClientInformationThread(object connectionIn)
        {
            Connection connection = (Connection)connectionIn;

            //make sure the server is ready to receive data from client
            while (!connection.successfulPingExecuted)
                Thread.Sleep(1);

            try
            {
                Send(connection, new SocketData(SocketData.DataType.UserInformation, null));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ReceiveThread(object obj)
        {
            byte[] buff = new byte[10000];
            Connection connection = (Connection)obj;
            Socket sock = connection.connectionSocket;
            sock.ReceiveTimeout = 0;

            while (sock.Connected)
            {

                try
                {
                    sock.Receive(buff);
                }
                catch
                {
                    //disconnect client
                    Invoke(new delVoidConnection(DisconnectClient), (Connection)obj);
                    break;
                }

                BinaryFormatter bf = new BinaryFormatter();
                SocketData socketFrame = (SocketData)bf.Deserialize(new MemoryStream(buff));

                if (!this.IsDisposed)
                {
                    switch (socketFrame.dataType)
                    {
                        case SocketData.DataType.Chat:
                            Invoke(new delVoidString(AppendToLog), connection.name + ": " + (string)socketFrame.data);
                            break;
                        case SocketData.DataType.RequestPing:
                            Invoke(new delVoidConnectionObject(PingClient), connection, socketFrame.data);
                            break;
                        case SocketData.DataType.RespondToPing:
                            connection.successfulPingExecuted = true;
                            long pingTime = (Stopwatch.GetTimestamp() - (long)socketFrame.data) / (Stopwatch.Frequency / 1000);
                            Invoke(new delVoidConnectionString(UpdateClientPing), connection, pingTime.ToString());
                            break;
                        case SocketData.DataType.Boot:
                            Invoke(new delVoidConnection(BootClient), connection);
                            break;
                        case SocketData.DataType.UserInformation:
                            Invoke(new delVoidConnectionObject(ProcessClientInformation), connection, socketFrame.data);
                            break;
                    }
                }
            }
        }


        void ProcessClientInformation(Connection connection, object data)
        {
            connection.name = (string)data;
            connection.chatButton.Text = connection.name;
            AppendToLog(connection.name + " has connected (" + connection.connectionSocket.RemoteEndPoint.ToString() + ")");
        }


        void UpdateClientPing(Connection connection, string ping)
        {
            connection.pingLabel.Text = ping;
        }

        void DisconnectClient(Connection connection)
        {
            connection.connectionSocket.Close();
            AppendToLog(connection.name + " has disconnected (" + connection.remoteEndPoint + ")");
            connections.Remove(connection);

            this.Controls.Remove(connection.chatButton);
            this.Controls.Remove(connection.pingLabel);
            this.Controls.Remove(connection.disconnectButton);

            if (connections.Count == 0)
                sendAllButton.Enabled = false;

            UpdateConnectedClientsLabel();

            for (int i = 0; i < connections.Count; ++i)
            {
                connections[i].disconnectButton.Location = new Point(connectedClientsLabel.Location.X, connectedClientsLabel.Location.Y + connectedClientsLabel.Size.Height + 10 + (i * connections[i].disconnectButton.Height));
                connections[i].chatButton.Location = new Point(connections[i].disconnectButton.Location.X + connections[i].disconnectButton.Width + 5, connections[i].disconnectButton.Location.Y);
                connections[i].pingLabel.Location = new Point(connections[i].chatButton.Location.X + connections[i].chatButton.Width + 5, connections[i].chatButton.Location.Y);
            }
        }

        void PingClient(Connection connection, object data)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, new SocketData(SocketData.DataType.RespondToPing, data));

                connection.connectionSocket.Send(ms.GetBuffer(), (int)ms.Length, SocketFlags.None);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void AppendToLog(string message)
        {
            logTextBox.Text = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + ": " + message + Environment.NewLine + logTextBox.Text;
        }

        private void sendAllButton_Click(object sender, EventArgs e)
        {
            foreach (Connection connection in connections)
                Send(connection, new SocketData(SocketData.DataType.Chat, messageTextBox.Text));
            AppendToLog("[ALL] Server: " + messageTextBox.Text);
            messageTextBox.Text = "";
        }

        private void messageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                sendAllButton.PerformClick();
                e.SuppressKeyPress = true;
            }
        }
    }
}