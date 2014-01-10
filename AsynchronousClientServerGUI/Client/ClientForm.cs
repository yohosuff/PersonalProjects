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
using ConnectionUtilities;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;

namespace Client
{
    
    public partial class ClientForm : Form
    {
        delegate bool delBoolSocket(Socket s);
        delegate void delVoid();
        delegate void delVoidSocket(Socket s);
        delegate void delVoidSocketObject(Socket s, object o);
        delegate void delVoidString(string s);
        
        Socket connectionSocket = null;
        bool connected = false;
        
        public ClientForm()
        {
            InitializeComponent();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            if (!connected)
            {
                connectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                
                try
                {
                    Thread thread = new Thread(ConnectToServerThread);
                    thread.IsBackground = true;
                    thread.Start(connectionSocket);
                }
                catch (Exception ex)
                {
                    AppendToLog(ex.Message);
                    return;
                }

            }
            else
            {
                BootMe();
            }
        }
        
        private void ConnectToServerThread(object socket)
        {
            Socket connectionSocket = (Socket)socket;
            
            try
            {
                connectionSocket.Connect(new IPEndPoint(IPAddress.Parse(ipAddressTextBox.Text), int.Parse(portNumberTextBox.Text)));
            }
            catch (Exception ex)
            {
                AppendToLog(ex.Message);
                return;
            }

            Invoke(new delVoidSocket(HandleSuccessfulConnection), connectionSocket);
        }
        
        private void HandleSuccessfulConnection(Socket connectionSocket)
        {
            AppendToLog("Connected to server (" + connectionSocket.RemoteEndPoint.ToString() + ")");
            connectButton.Text = "Disconnect";
            connected = true;
            ipAddressTextBox.Enabled = false;
            portNumberTextBox.Enabled = false;
            usernameTextBox.Enabled = false;
            sendButton.Enabled = true;
            connectionInformationLabel.Text = "Connected to server (" + connectionSocket.RemoteEndPoint.ToString() + ") as " + usernameTextBox.Text + " (" + connectionSocket.LocalEndPoint.ToString() + ")";
            
            Thread thread = new Thread(ReceiveThread);
            thread.IsBackground = true;
            thread.Start(connectionSocket);

            thread = new Thread(PingServerThread);
            thread.IsBackground = true;
            thread.Start(connectionSocket);

        }

        private void PingServerThread(object socket)
        {
            Socket connectionSocket = (Socket)socket;

            try
            {
                while (connectionSocket.Connected && (bool)Invoke(new delBoolSocket(RequestPing), connectionSocket))
                {
                    Thread.Sleep(250);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source);
            }
        }
                
        bool RequestPing(Socket connectionSocketIn)
        {
            Socket connectionSocket = (Socket)connectionSocketIn;
            try
            {
                Send(connectionSocket, new SocketData(SocketData.DataType.RequestPing, Stopwatch.GetTimestamp()));
                return true;
            }
            catch
            {
                //try and do nothing
                AppendToLog("server disconnected");
                return false;
            }
        }

        void Send(Socket connectedSocket, SocketData socketFrame)
        {
            if (connectedSocket.Connected)
            {
                MemoryStream ms = new MemoryStream();
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, socketFrame);

                try
                {
                    connectedSocket.Send(ms.GetBuffer(), (int)ms.Length, SocketFlags.None);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            
        }

        private void ReceiveThread(object obj)
        {
            byte[] buff = new byte[10000];
            Socket socket = (Socket)obj;
            socket.ReceiveTimeout = 0;

            while (socket.Connected)
            {
                try
                {
                    socket.Receive(buff);
                }
                catch
                {
                    //MessageBox.Show(ex.Message);
                    Invoke(new delVoid(DisconnectFromServer));
                    break;
                }

                BinaryFormatter bf = new BinaryFormatter();
                SocketData socketFrame = (SocketData)bf.Deserialize(new MemoryStream(buff));

                switch (socketFrame.dataType)
                {
                    case SocketData.DataType.Chat:
                        Invoke(new delVoidString(AppendToLog), socket.RemoteEndPoint.ToString() + ": " + (string)socketFrame.data);
                        break;
                    case SocketData.DataType.RespondToPing:
                        long pingTime = (Stopwatch.GetTimestamp() - (long)socketFrame.data) / (Stopwatch.Frequency / 1000);
                        Invoke(new delVoidString(UpdatePingLabel), pingTime.ToString());
                        break;
                    case SocketData.DataType.RequestPing:
                        //server has sent a ping to this client
                        Invoke(new delVoidSocketObject(PingServer), socket, socketFrame.data);
                        break;
                    case SocketData.DataType.Boot:
                        //server has requested the client to politely disconnect itself.
                        Invoke(new delVoid(DisconnectFromServer));
                        break;
                    case SocketData.DataType.UserInformation:
                        Invoke(new delVoid(SendUserInformation));
                        break;
                }
            }
        }
        
        void SendUserInformation()
        {
            try
            {
                Send(connectionSocket, new SocketData(SocketData.DataType.UserInformation, usernameTextBox.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void PingServer(Socket socket, object data)
        {
            try
            {
                Send(socket, new SocketData(SocketData.DataType.RespondToPing, data));
            }
            catch
            {
                //MessageBox.Show(ex.Message);                
            }
        }

        void UpdatePingLabel(string ping)
        {
            pingLabel.Text = "Ping: " + ping + " ms";
        }

        void AppendToLog(string message)
        {
            logTextBox.Text = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + ": " + message + Environment.NewLine + logTextBox.Text;
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            try
            {
                Send(connectionSocket, new SocketData(SocketData.DataType.Chat, messageTextBox.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            AppendToLog(connectionSocket.LocalEndPoint.ToString() + ": " + messageTextBox.Text);
            messageTextBox.Text = "";
        }

        private void messageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                sendButton.PerformClick();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        void DisconnectFromServer()
        {
            AppendToLog("Disconnected from server");
            connectionSocket.Close();
            ipAddressTextBox.Enabled = true;
            portNumberTextBox.Enabled = true;
            usernameTextBox.Enabled = true;
            connectButton.Text = "Connect";
            connectionInformationLabel.Text = "Client is not connected";
            pingLabel.Text = "Ping: N/A";
            connected = false;
        }

        void BootMe()
        {
            try
            {
                Send(connectionSocket, new SocketData(SocketData.DataType.Boot, null));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }

}
