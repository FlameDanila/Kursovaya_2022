using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace kursach_2022
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void regButton_Click(object sender, RoutedEventArgs e)
        {
            if (Error.Visibility == Visibility.Visible)
            { MessageBox.Show("Этот логин уже занят, придумайте другой"); }
            else
            {
                if (FirstNameBox.Text == "" || LastNameBox.Text == "" || MiddleNameBox.Text == "" || PhoneBox.Text == "" || PasswordBox.Text == "" || LoginBox.Text == "")
                { MessageBox.Show("У вас остались незаполненые поля"); }
                else
                {
                    if (PasswordBox.Text != RepeatPassBox.Text)
                    { MessageBox.Show("Пароли должны совпадать!"); }
                    else
                    {
                        Stuff purchasers = new Stuff()
                        {
                            Name = LastNameBox.Text + " " + FirstNameBox.Text + " " + MiddleNameBox.Text,
                            Phone = PhoneBox.Text,
                            Login = LoginBox.Text,
                            Password = PasswordBox.Text,
                            Gender = FloorCombo.Text
                        };

                        App.db.Stuff.Add(purchasers);
                        App.db.SaveChanges();
                        MessageBox.Show("Пользователь " + FirstNameBox.Text + " добавлен");

                        List<Stuff> OwnersLoginList = App.db.Stuff.ToList();

                        var LoginList = OwnersLoginList.Select(n => n.Login).ToList();

                        if (LoginList.Contains(LoginBox.Text))
                        {
                            Error.Visibility = Visibility.Visible;
                        }
                        else { Error.Visibility = Visibility.Hidden; }
                    }
                }
            }
        }

        private void LoginBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Stuff> OwnersLoginList = App.db.Stuff.ToList();

            var LoginList = OwnersLoginList.Select(n => n.Login).ToList();

            if (LoginList.Contains(LoginBox.Text))
            {
                Error.Visibility = Visibility.Visible;
            }
            else { Error.Visibility = Visibility.Hidden; }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        public new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-Я]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        public void DigitsTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
