using System;
using System.Windows;
using System.Windows.Controls;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Data.SQLite;
using System.Collections.Generic;

namespace server
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int port = 666;
        static TcpListener listener;
        string currentTime = DateTime.Now.ToLongTimeString();

        struct SClient
        {
            public TcpClient client;
            public NetworkStream stream;
            public string us;
        }

        List<SClient> clients = new List<SClient>();


        public MainWindow()
        {
            InitializeComponent();
        }

        private void startServerBt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
                listener.Start();
                ServerLog.Items.Add(currentTime + ":\tServer has been started");

                Thread clientThread = new Thread(() => listen());
               // Dispatcher.BeginInvoke(new Action(() => ServerLog.Items.Add(currentTime + ":\tNew connection")));
                clientThread.Start();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void stopServerBt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ConnectedUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ServerLog_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void banUserBt_Click(object sender, RoutedEventArgs e)
        {

        }

        public void listen()
        {
            try
            {
                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Thread clientThread = new Thread(() => Process(client));
                    Dispatcher.BeginInvoke(new Action(() => ServerLog.Items.Add(currentTime + ":\tNew connection")));
                    clientThread.Start();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Process(TcpClient tcpClient)
        {
            int ex = -1;
            
            //TcpClient client = tcpClient;
            //NetworkStream stream = null;

            SClient client = new SClient();

            client.client = tcpClient;

            client.stream = client.client.GetStream();

            clients.Add(client);

            byte[] data = new byte[64];
            try
            {
                while (true)
                {
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = client.stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (client.stream.DataAvailable);

                    string message = builder.ToString();
                    ServerLog.Items.Add(message);
                    string clientCode = message.Substring(0, 3);
                    message = message.Substring(3);

                    ServerLog.Items.Add(clientCode + "\t" + message);

                    string db_name = "C:\\Users\\Admin\\Documents\\РИ-89\\KursaChat\\KursaChat.db";
                    SQLiteConnection m_dbConnection;
                    m_dbConnection = new SQLiteConnection("Data Source=" + db_name + ";Version=3;");
                    m_dbConnection.Open();

                    string response = "";


                    switch (clientCode)
                    {
                        case "cl":
                        {                           
                             string sqlCL = "SELECT COUNT(*) FROM Users WHERE Username = '" + message + "'";
                             SQLiteCommand command = new SQLiteCommand(sqlCL, m_dbConnection);
                             object reader = command.ExecuteScalar();
                             int exist = Convert.ToInt32(reader);
                             client.us = message;
                             if (exist == 0)
                             { 
                                ex = 0;
                             }
                             else
                             {
                                ex = 1;
                             }
                        }
                        break;

                        case "cp":
                            {

                                if (ex == 0)
                                {
                                    string add = "INSERT INTO Users(Username, Password) VALUES('" + client.us + "', '" + message + "')";
                                    response = "srp";
                                    data = Encoding.Unicode.GetBytes(response);
                                    client.stream.Write(data, 0, data.Length);
                                }
                                else
                                {
                                    string sqlCP = "SELECT COUNT(*) FROM Users WHERE Username = '" + client.us + "' AND Password = '" + message + "'";
                                    SQLiteCommand command = new SQLiteCommand(sqlCP, m_dbConnection);
                                    object reader = command.ExecuteScalar();
                                    int exist = Convert.ToInt32(reader);
                                    if(exist == 0)
                                    {
                                        response = "swp";
                                        data = Encoding.Unicode.GetBytes(response);
                                        client.stream.Write(data, 0, data.Length);
                                    }
                                    else
                                    {
                                        response = "srp";
                                        data = Encoding.Unicode.GetBytes(response);
                                        client.stream.Write(data, 0, data.Length);
                                    }
                                }
                            }
                            break;

                        case "cm":
                            string sqlCM = "INSERT INTO GeneralMes(Username, Message) VALUES('" + client.us + "', '" + message + "')";
                            data = Encoding.Unicode.GetBytes("scm" + client.us + "§" + message);
                            foreach (SClient cl in clients)
                            {
                                if (cl.client != client.client)
                                    cl.stream.Write(data, 0, data.Length);
                            }
                            break;
                    }

                    m_dbConnection.Close();
                }
            }
            catch
            {

            }
            finally
            {
                if (client.client != null)
                {
                    client.client.Close();
                }
                if (client.stream != null)
                {
                    client.stream.Close();
                }
            }
        }
    }
}