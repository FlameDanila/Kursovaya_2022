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

namespace kursach_2022
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Object sender = new Object();
            RoutedEventArgs e = new RoutedEventArgs();

            authButton_Click(sender, e);
        }
        public int Flag = 0;
        public int close = 0;

        private void authButton_Click(object sender, RoutedEventArgs e)
        {
            List<Stuff> stuffList = App.db.Stuff.ToList();

            var PasswordList = stuffList.Select(n => n.Password).ToList();
            var LoginList = stuffList.Select(n => n.Login).ToList();

            for (int i = 0; i < stuffList.Count; i++)
            {
                if (LoginBox.Text == LoginList[i])
                {
                    if (PasswordBox.Text == PasswordList[i] || MyPasswordBox.Password == PasswordList[i])
                    {
                        Menu menu = new Menu();
                        menu.Show();
                        Close();
                        break;
                    }
                    else
                    {
                        MessageBox.Show("Пароль введен неверно");

                        PasswordBox.Text = ""; MyPasswordBox.Password = "";
                        break;
                    }
                }
            }
        }
        private void ShowPassword(object sender, RoutedEventArgs e)
        {
            PasswordBox.Text = MyPasswordBox.Password;
            MyPasswordBox.Visibility = Visibility.Hidden;
            PasswordBox.Visibility = Visibility.Visible;
        }

        private void UnshowPassword(object sender, RoutedEventArgs e)
        {
            MyPasswordBox.Password = PasswordBox.Text;
            PasswordBox.Visibility = Visibility.Hidden;
            MyPasswordBox.Visibility = Visibility.Visible;
        }

        private void regButton_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            Close();
        }
    }
}