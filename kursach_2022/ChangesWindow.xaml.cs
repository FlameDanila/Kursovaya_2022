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
    /// Логика взаимодействия для ChangesWindow.xaml
    /// </summary>
    public partial class ChangesWindow : Window
    {
        public ChangesWindow()
        {
            InitializeComponent();
            Update();
        }

        public void Update()
        {

            var OwnersLoginList = App.db.Stuff.Where(n => n.Id == App.userId).ToList();
            foreach (var a in OwnersLoginList)
            {
                string[] nameUser = a.Name.Split(' ');
                LoginBox.Text = a.Login;
                if (a.Gender == "Мужской")
                {
                    FloorCombo.SelectedIndex = 1;
                }
                else
                { FloorCombo.SelectedIndex = 0; }
                PhoneBox.Text = a.Phone;
                MiddleNameBox.Text = nameUser[2];
                LastNameBox.Text = nameUser[0];
                FirstNameBox.Text = nameUser[1];
            }
        }

        private void regButton_Click(object sender, RoutedEventArgs e)
        {

            MyPasswordBox.Password = PasswordBox.Text;
            if (FirstNameBox.Text == "" || LastNameBox.Text == "" || MiddleNameBox.Text == "" || PhoneBox.Text == "" || LoginBox.Text == "" || oldPassText.Text == "")
            {
                if (oldPassText.Text == "")
                {
                    MessageBox.Show("Чтобы сохранить изменения введите пароль");
                }
                else
                {
                    MessageBox.Show("У вас остались незаполненые поля");
                }
            }
            else
            {
                if (PasswordBox.Text != RepeatPassBox.Text)
                { MessageBox.Show("Пароли должны совпадать!"); }
                else
                {
                    var stuff = App.db.Stuff.Where(n => n.Id == App.userId).FirstOrDefault();

                    stuff.Name = LastNameBox.Text.Replace(" ", "") + " " + FirstNameBox.Text.Replace(" ", "") + " " + MiddleNameBox.Text.Replace(" ", "");
                    stuff.Phone = PhoneBox.Text.Replace(" ", "");
                    stuff.Login = LoginBox.Text.Replace(" ", "");
                    stuff.Password = PasswordBox.Text.Replace(" ", "");
                    stuff.Gender = FloorCombo.Text;

                    App.db.SaveChanges();
                    MessageBox.Show("Профиль изменён");
                }
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Menu Menu = new Menu();
            Menu.Show();
            Close();
        }

        public new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"([^а-яА-Я]+)");
            e.Handled = regex.IsMatch(e.Text);
        }
        public void DigitsTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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

        private void refrechButton_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }
    }
}
