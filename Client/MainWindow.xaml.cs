using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
   
    public partial class MainWindow : Window
    {
        const string address = "127.0.0.1";
        TcpClient client = null;
        NetworkStream stream;
        const int port = 666;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client = new TcpClient(address, port);
                stream = client.GetStream();

                Thread myThread1 = new Thread(new ThreadStart(Count));
                myThread1.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                string uspa = "up" + LoginTb.Text + "¬" + PasswdTb.Password.ToString();

                byte[] up = Encoding.Unicode.GetBytes(uspa);

                stream.Write(up, 0, up.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        public void Count()
        {
            try
            {
                while (true)
                {
                    byte[] data = new byte[64];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();
                    string serverCodeResponse = message.Substring(0, 3);
                    message = message.Substring(3);

                    switch (serverCodeResponse)
                    {
                        case "swp": Dispatcher.BeginInvoke(new Action(() => responseLog.Content = "Wrong password")); break;
                        case "srp":
                            Dispatcher.BeginInvoke(new Action(() => SignInGrid.Visibility = Visibility.Hidden));
                            Dispatcher.BeginInvoke(new Action(() => MainSpace.Visibility = Visibility.Visible));
                            break;
                        case "sub":
                            Dispatcher.BeginInvoke(new Action(() => SignInGrid.Visibility = Visibility.Visible));
                            Dispatcher.BeginInvoke(new Action(() => MainSpace.Visibility = Visibility.Hidden));
                            Dispatcher.BeginInvoke(new Action(() => responseLog.Content = "You have been baned"));
                            break;
                        case "scm":
                            int pos = message.IndexOf('§');
                            string us = message.Substring(0, pos);
                            message = message.Substring(pos+1);
                            Dispatcher.BeginInvoke(new Action(() => history.Items.Add(DateTime.Now + "\t" + us + ": " + message)));
                            break;
                        case "shm":
                            pos = message.IndexOf("shm");
                            while (pos != -1)
                            {
                                string HMes = message.Substring(0, pos);
                                Dispatcher.BeginInvoke(new Action(() => history.Items.Add(HMes)));
                                message = message.Substring(pos + 3);
                                pos = message.IndexOf("shm");
                            }
                            Dispatcher.BeginInvoke(new Action(() => history.Items.Add(message)));
                            break;
                        case "sum":
                            pos = message.IndexOf("sum");
                            while (pos != -1)
                            {
                                string UMes = message.Substring(0, pos);
                                Dispatcher.BeginInvoke(new Action(() => Users.Items.Add(UMes)));
                                message = message.Substring(pos + 3);
                                pos = message.IndexOf("shm");
                            }
                            Dispatcher.BeginInvoke(new Action(() => Users.Items.Add(message)));
                            break;
                    }
                }
            }
            catch
            {

            }
            finally
            {

            }


        }
             
        private void SendMess_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string message = "cm" + Message.Text;

                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string message = "dsc0nn3c710n_c10s3";
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
            stream.Close();
            client.Close();
        }
    }
}