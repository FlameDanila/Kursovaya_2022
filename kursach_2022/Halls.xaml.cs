using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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

            hallText.Text = "Зал " + App.hallId;

            DataTable data = Select($"select * from dbo.hall_{App.hallId}");

            int top = 150;
            int bottom = 18;

            for (int j = 0; j < data.Rows.Count; j++)
            {
                for (int i = 1; i < data.Columns.Count; i++)
                {
                    if (data.Rows[j][i].Equals(1))
                    {
                        Button button = new Button();
                        button.Width = 80;
                        button.Height = 60;
                        button.HorizontalAlignment = HorizontalAlignment.Left;
                        button.VerticalAlignment = VerticalAlignment.Top;
                        button.Content = "Забронировано";
                        button.FontSize = 10;

                        button.Margin = new Thickness(top, bottom, 0, 0);
                        grid.Children.Add(button);
                        top += 100;
                    }
                    else
                    {
                        Button button = new Button();
                        button.Width = 80;
                        button.Height = 60;
                        button.HorizontalAlignment = HorizontalAlignment.Left;
                        button.VerticalAlignment = VerticalAlignment.Top;
                        button.Background = Brushes.AliceBlue;
                        button.Content = "Свободно";

                        button.Margin = new Thickness(top, bottom, 0, 0);
                        grid.Children.Add(button);
                        top += 100;
                    }
                }
                top = 150;
                bottom += 80;
            }
        }
        public DataTable Select(string selectSQL)
        {
            DataTable data = new DataTable("dataBase");

            SqlConnection sqlConnection = new SqlConnection($@"server = DESKTOP-ITVEB8Q\SQLEXPRESS;Trusted_connection=Yes;DataBase=Kassir");
            sqlConnection.Open();

            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = selectSQL;

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            sqlDataAdapter.Fill(data);

            return data;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HallsMenu menu = new HallsMenu();
            menu.Show();
            Close();
        }
    }
}
