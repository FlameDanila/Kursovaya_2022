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
using System.Windows.Shapes;

namespace kursach_2022
{
    /// <summary>
    /// Логика взаимодействия для Halls.xaml
    /// </summary>
    public partial class Halls : Window
    {
        public Halls()
        {
            InitializeComponent();
            Update();
            NameText.Text = "Здраствуйте, " + App.name;

            List<Stuff> PurchasersLoginList = App.db.Stuff.ToList();
            var SecondLoginList = PurchasersLoginList.Select(n => n.Login).ToList();
        }

        public void Update()
        {
            List.ItemsSource = App.db.Stuff.ToList();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            Close();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
