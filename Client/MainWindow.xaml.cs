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

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            SignInGrid.Visibility = Visibility.Hidden;
            RegistrationGrid.Visibility = Visibility.Visible;
            LoginReg.Clear();
            PasswdReg.Clear();
            PasswdRegVer.Clear();
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            SignInGrid.Visibility = Visibility.Hidden;
            RegistrationGrid.Visibility = Visibility.Hidden;
            MainSpace.Visibility = Visibility.Visible;
        }

        private void DoneRegBt_Click(object sender, RoutedEventArgs e)
        {
            RegistrationGrid.Visibility = Visibility.Hidden;
            SignInGrid.Visibility = Visibility.Visible;
        }

        private void LoginReg_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
