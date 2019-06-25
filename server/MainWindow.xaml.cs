using System;
using System.Windows;
using System.Windows.Controls;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Data.SQLite;
using System.Collections.Generic;
using System.IO;

namespace server
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int number;
        const int port = 11337;
        static TcpListener listener;
        string currentTime = DateTime.Now.ToLongTimeString();
        string path = "C:\\Users\\Admin\\source\\repos\\KursaChat\\logList.txt";
        //string path = "C:\\Users\\FOX\\source\\repos\\KursaChat\\logList.txt";
        //string path = "C:\\Users\\Admin\\Desktop\\KursaChat\\KursaChat.db";

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
                listener = new TcpListener(IPAddress.Parse("10.23.168.35"), port);
                listener.Start();
                ServerLog.Items.Add(currentTime + ":\tServer has been started");

                Thread clientThread = new Thread(() => listen());
                // Dispatcher.BeginInvoke(new Action(() => ServerLog.Items.Add(currentTime + ":\tNew connection")));
                clientThread.Start();

                foreach (SClient cl in clients)
                {
                    byte[] data = new byte[64];
                    data = Encoding.Unicode.GetBytes("sta");
                    cl.stream.Write(data, 0, data.Length);
                }

            }
            catch (Exception ex)
            {
                
            }
        }

        private void stopServerBt_Click(object sender, RoutedEventArgs e)
        {
            foreach (SClient cl in clients)
            {
                byte[] data = new byte[64];
                data = Encoding.Unicode.GetBytes("sto");
                cl.stream.Write(data, 0, data.Length);
            }
            listener.Stop();
            ServerLog.Items.Add("Server has been stopped");
            ConnectedUsers
                .Items.Clear();
            
        }

        private void ConnectedUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ServerLog_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void banUserBt_Click(object sender, RoutedEventArgs e)
        {
            if (ConnectedUsers.SelectedIndex > -1)
            {
                string db_name = "C:\\Users\\Admin\\source\\repos\\KursaChat\\KursaChat.db";
                //string db_name = "C:\\Users\\FOX\\source\\repos\\KursaChat\\KursaChat.db";
                //string db_name = "C:\\Users\\Admin\\Desktop\\KursaChat\\KursaChat.db";
                SQLiteConnection m_dbConnection;
                m_dbConnection = new SQLiteConnection("Data Source=" + db_name + ";Version=3;");
                m_dbConnection.Open();

                string banned = ConnectedUsers.SelectedItem.ToString();
                foreach (SClient cl in clients)
                {
                    byte[] data = new byte[64];
                    if (cl.us == banned)
                    {
                        string sqlInsertBan = "INSERT INTO BanUsers(Username, Password) SELECT Username, Password FROM Users WHERE Username = '" + cl.us + "'";
                        string sqlDeleteBan = "DELETE FROM Users WHERE Username = '" + cl.us + "'";
                        SQLiteCommand commandI = new SQLiteCommand(sqlInsertBan, m_dbConnection);
                        commandI.ExecuteNonQuery();
                        SQLiteCommand commandD = new SQLiteCommand(sqlDeleteBan, m_dbConnection);
                        commandD.ExecuteNonQuery();

                        string response = "sub";
                        data = Encoding.Unicode.GetBytes(response);
                        cl.stream.Write(data, 0, data.Length);
                        clients.Remove(cl);
                        ConnectedUsers.Items.RemoveAt(ConnectedUsers.SelectedIndex);
                        ConnectedUsers.Items.Refresh();
                        if (cl.client != null)
                        {
                            cl.client.Close();
                        }
                        if (cl.stream != null)
                        {
                            cl.stream.Close();
                        }
                        break;
                    }

                    data = Encoding.Unicode.GetBytes("sds" + banned);
                    if (cl.us != banned)
                    {
                        cl.stream.Write(data, 0, data.Length);
                    }
                }

                m_dbConnection.Close();
                
            }
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
                
            }
        }

        public void Process(TcpClient tcpClient)
        {

            //TcpClient client = tcpClient;
            //NetworkStream stream = null;

            SClient client = new SClient();

            client.client = tcpClient;

            client.stream = client.client.GetStream();

            byte[] data = new byte[64];

            string db_name = "C:\\Users\\Admin\\source\\repos\\KursaChat\\KursaChat.db";
            //string db_name = "C:\\Users\\FOX\\source\\repos\\KursaChat\\KursaChat.db";
            //string db_name = "C:\\Users\\Admin\\Desktop\\KursaChat\\KursaChat.db";
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=" + db_name + ";Version=3;");
            m_dbConnection.Open();
            number = 0;
            foreach (string lineLog in File.ReadLines(path))
            {
                number++;
            }

            int ni = number;

            foreach (string lineLog in File.ReadLines(path))
            {
                if(ni<=10)
                {
                    //Dispatcher.BeginInvoke(new Action(() => ServerLog.Items.Add(Encoding.Unicode.GetBytes("sсm" + lineLog).Length.ToString())));

                    data = Encoding.Unicode.GetBytes("shm" + lineLog);
                    client.stream.Write(data, 0, data.Length);
                }
                else
                {
                    ni--;
                }
            }

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

                    //Dispatcher.BeginInvoke(new Action(() => ServerLog.Items.Add(clientCode + "\t" + message)));

                    string response = "";


                    switch (clientCode)
                    {
                        case "up":
                            
                            int indexOfChar = message.IndexOf("¬");
                            client.us = message.Substring(0, indexOfChar);

                            int bu = 0;

                            foreach (SClient cl in clients)
                            {
                                if (cl.us == client.us)
                                {
                                    //Dispatcher.BeginInvoke(new Action(() => ServerLog.Items.Add("bu")));
                                    bu = 1;
                                    break;
                                }
                            }

                            if (bu == 1)
                            {
                                break;
                            }

                            string sqlBan = "SELECT COUNT(*) FROM BanUsers WHERE Username = '" + client.us + "'";
                            SQLiteCommand commandB = new SQLiteCommand(sqlBan, m_dbConnection);
                            object readerB = commandB.ExecuteScalar();
                            int existB = int.Parse(readerB.ToString());
                            if(existB == 1)
                            {
                                string responseB = "sub";
                                data = Encoding.Unicode.GetBytes(responseB);
                                client.stream.Write(data, 0, data.Length);
                                break;
                            }


                            string sqlCL = "SELECT COUNT(*) FROM Users WHERE Username = '" + client.us + "'";
                            SQLiteCommand command = new SQLiteCommand(sqlCL, m_dbConnection);
                            object reader = command.ExecuteScalar();
                            int exist = int.Parse(reader.ToString());
                            client.password = message.Substring(indexOfChar + 1);
                            

                            if (exist == 0)
                            {
                                string add = "INSERT INTO Users(Username, Password) VALUES('" + client.us + "', '" + client.password + "')";
                                SQLiteCommand command1 = new SQLiteCommand(add, m_dbConnection);
                                command1.ExecuteNonQuery();
                                response = "srp";
                                data = Encoding.Unicode.GetBytes(response);
                                client.stream.Write(data, 0, data.Length);
                                clients.Add(client);
                                foreach (SClient cl in clients)
                                {
                                    data = Encoding.Unicode.GetBytes("sum" + cl.us);
                                    client.stream.Write(data, 0, data.Length);
                                }
                                Dispatcher.BeginInvoke(new Action(() => ConnectedUsers.Items.Add(client.us)));
                                Thread.Sleep(100);

                                foreach (SClient cl in clients)
                                {
                                    data = Encoding.Unicode.GetBytes("sms" + client.us);
                                    if (cl.client != client.client)
                                    {
                                        cl.stream.Write(data, 0, data.Length);
                                    }
                                }
                            }
                            else
                            {
                                string sqlCP = "SELECT COUNT(*) FROM Users WHERE Username = '" + client.us + "' AND Password = '" + client.password + "'";
                                SQLiteCommand command2 = new SQLiteCommand(sqlCP, m_dbConnection);
                                object reader1 = command2.ExecuteScalar();
                                int exist1 = int.Parse(reader1.ToString());
                                if (exist1 == 0)
                                {
                                    response = "swp";
                                    data = Encoding.Unicode.GetBytes(response);
                                    client.stream.Write(data, 0, data.Length);
                                }
                                else
                                {
                                    Thread.Sleep(100);
                                    response = "srp";
                                    data = Encoding.Unicode.GetBytes(response);
                                    client.stream.Write(data, 0, data.Length);
                                    clients.Add(client);
                                    Thread.Sleep(150);

                                    foreach (SClient cl in clients)
                                    {
                                        data = Encoding.Unicode.GetBytes("sum" + cl.us);
                                        client.stream.Write(data, 0, data.Length);
                                    }
                                    Dispatcher.BeginInvoke(new Action(() => ConnectedUsers.Items.Add(client.us)));
                                    Thread.Sleep(100);

                                    foreach (SClient cl in clients)
                                    {
                                        data = Encoding.Unicode.GetBytes("sms" + client.us);
                                        if (cl.client != client.client)
                                        {
                                            cl.stream.Write(data, 0, data.Length);
                                        }
                                    }
                                }
                            }

                            break;

                        case "cm":
                            string appendText = DateTime.Now.ToString() + '\t' + client.us + ": " + message + Environment.NewLine;
                            File.AppendAllText(path, appendText);
                            
                            data = Encoding.Unicode.GetBytes("scm" + client.us + "§" + message);
                            foreach (SClient cl in clients)
                            {
                                if (cl.client != client.client)
                                    cl.stream.Write(data, 0, data.Length);
                            }
                            client.stream.Write(data, 0, data.Length);
                            break;

                        
                        case "ds":
                            foreach (SClient cl in clients)
                            {
                                data = Encoding.Unicode.GetBytes("sds" + client.us);
                                if (cl.client != client.client)
                                {
                                    cl.stream.Write(data, 0, data.Length);
                                }
                            }

                            Dispatcher.BeginInvoke(new Action(() => ConnectedUsers.Items.Remove(client.us)));
                            Dispatcher.BeginInvoke(new Action(() => ServerLog.Items.Add(client.us + ":Connection lost")));
                            clients.Remove(client);
                            Dispatcher.BeginInvoke(new Action(() => ConnectedUsers.Items.Refresh()));
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                //if (client.client != null)
                //{
                //    client.client.Close();
                //}
                //if (client.stream != null)
                //{
                //    client.stream.Close();
                //}
            }
            m_dbConnection.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listener.Stop();
        }
    }
}