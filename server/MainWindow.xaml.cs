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
            public string password;
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
                    string clientCode = message.Substring(0, 2);
                    message = message.Substring(2);

                    Dispatcher.BeginInvoke(new Action(() => ServerLog.Items.Add(clientCode + "\t" + message)));

                    //string db_name = "C:\\Users\\Admin\\Documents\\РИ-89\\KursaChat\\KursaChat.db";
                    string db_name = "C:\\Users\\Chudo\\source\\repos\\KursaChat\\KursaChat.db";
                    SQLiteConnection m_dbConnection;
                    m_dbConnection = new SQLiteConnection("Data Source=" + db_name + ";Version=3;");
                    m_dbConnection.Open();

                    string response = "";


                    switch (clientCode)
                    {
                        case "up":
                             int indexOfChar = message.IndexOf("¬");
                             client.us = message.Substring(0, indexOfChar);
                             string sqlCL = "SELECT COUNT(*) FROM Users WHERE Username = '" + client.us + "'";
                             SQLiteCommand command = new SQLiteCommand(sqlCL, m_dbConnection);
                             object reader = command.ExecuteScalar();
                             int exist = int.Parse(reader.ToString());
                             client.password = message.Substring(indexOfChar+1);
                            if (exist == 0)
                            {
                                string add = "INSERT INTO Users(Username, Password) VALUES('" + client.us + "', '" + client.password + "')";
                                SQLiteCommand command1 = new SQLiteCommand(add, m_dbConnection);
                                command1.ExecuteNonQuery();
                                response = "srp";
                                data = Encoding.Unicode.GetBytes(response);
                                client.stream.Write(data, 0, data.Length);
                            }
                            else
                            {
                                string sqlCP = "SELECT COUNT(*) FROM Users WHERE Username = '" + client.us + "' AND Password = '" + client.password + "'";
                                SQLiteCommand command2 = new SQLiteCommand(sqlCP, m_dbConnection);
                                object reader1 = command.ExecuteScalar();
                                int exist1 = Convert.ToInt32(reader);
                                if (exist1 == 0)
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