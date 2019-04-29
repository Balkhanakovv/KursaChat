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
                string username = "cl" + LoginTb.Text;
                string password = "cp" + PasswdTb.Password.ToString();

                byte[] un = Encoding.Unicode.GetBytes(username);
                byte[] pw = Encoding.Unicode.GetBytes(password);

                stream.Write(un, 0, un.Length);
                stream.Write(pw, 0, pw.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            SignInGrid.Visibility = Visibility.Hidden;
            MainSpace.Visibility = Visibility.Visible;
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

                    switch (serverCodeResponse)
                    {
                        case "swp": break;
                        case "srp": break;
                        case "sub": break;
                        case "scm": break;
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

        private void LoginReg_TextChanged(object sender, TextChangedEventArgs e)
        {

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
    }
}