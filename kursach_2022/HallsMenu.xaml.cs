using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для HallsMenu.xaml
    /// </summary>
    public partial class HallsMenu : Window
    {
        public HallsMenu()
        {
            InitializeComponent();

            Update();
        }

        public void Update()
        {
            grid.Children.Clear();

            DataTable data = Select("SELECT TABLE_NAME AS [Название таблицы] FROM INFORMATION_SCHEMA.TABLES WHERE table_type = 'BASE TABLE'");

            List<string> listOfHalls = new List<string>();
            List<string> list = new List<string>();

            for (int j = 0; j < data.Rows.Count; j++)
            {
                if (data.Rows[j][0].ToString().ToLower().StartsWith("hall"))
                {
                    listOfHalls.Add(data.Rows[j][0].ToString());
                    App.hallId = j+1;
                    //DataTable table = Select($"select name from films");
                    //list.Add(table.Rows[j][0].ToString());
                    //MessageBox.Show(table.Rows[j][0].ToString());
                }
            }

            int top = 80;
            int bottom = 33;

            int counter = 1;
            int enabled = 0;

            for (int i = 1; i < 4; i++)
            {
                for (int g = 1; g < 4; g++)
                {
                    if (listOfHalls.Count >= counter)
                    {
                        Button button = new Button();

                        button.Width = 131;
                        button.Height = 112;
                        button.HorizontalAlignment = HorizontalAlignment.Left;
                        button.VerticalAlignment = VerticalAlignment.Top;
                        button.Background = null;
                        button.Content = "Зал " + counter;
                        button.FontSize = 40;
                        button.BorderThickness = new Thickness(5);
                        button.Foreground = Brushes.Black;
                        button.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FF0365CF");
                        button.Margin = new Thickness(top, bottom, 0, 0);
                        button.MouseRightButtonDown += Dell_Click;

                        button.Click += Hall_Click;

                        grid.Children.Add(button);

                        top += 250;
                        counter++;
                    }
                    else
                    {
                        if (enabled == 0)
                        {
                            Button button = new Button();

                            button.Width = 131;
                            button.Height = 112;
                            button.HorizontalAlignment = HorizontalAlignment.Left;
                            button.VerticalAlignment = VerticalAlignment.Top;
                            button.Background = null;
                            button.Content = "➕";
                            button.FontSize = 72;
                            button.BorderThickness = new Thickness(5);
                            button.Foreground = (Brush)new BrushConverter().ConvertFrom("#FFFF3E28");
                            button.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FF0365CF");
                            button.Margin = new Thickness(top, bottom, 0, 0);

                            button.Click += Add_Click;

                            grid.Children.Add(button);

                            top += 250;
                            counter++;
                            enabled = 1;
                        }
                        else
                        {
                            Button button = new Button();

                            button.Width = 131;
                            button.Height = 112;
                            button.HorizontalAlignment = HorizontalAlignment.Left;
                            button.VerticalAlignment = VerticalAlignment.Top;
                            button.Background = null;
                            button.Content = "➕";
                            button.FontSize = 72;
                            button.BorderThickness = new Thickness(5);
                            button.Foreground = (Brush)new BrushConverter().ConvertFrom("#FFFF3E28");
                            button.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FF0365CF");
                            button.Margin = new Thickness(top, bottom, 0, 0);
                            button.IsEnabled = false;

                            button.Click += Add_Click;

                            grid.Children.Add(button);

                            top += 250;
                            counter++;
                        }
                    }
                }
                top = 80;
                bottom += 150;
            }
        }

        public DataTable Select(string selectSQL)
        {
            DataTable data = new DataTable("dataBase");

            //SqlConnection sqlConnection = new SqlConnection($@"server = DESKTOP-ITVEB8Q\SQLEXPRESS;Trusted_connection=No;DataBase=Kassir;User=ws1;PWD=ws1");
            SqlConnection sqlConnection = new SqlConnection($@"server = DESKTOP-ITVEB8Q\SQLEXPRESS;Trusted_connection=Yes;DataBase=Kassir");
            sqlConnection.Open();

            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = selectSQL;

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            sqlDataAdapter.Fill(data);

            return data;
        }

        private void Dell_Click(object sender, RoutedEventArgs e)
        {
            string f = sender.ToString();
            string[] mass = f.Split(' ');

            if (MessageBox.Show("Вы точно хотите удалить эту таблицу?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No) { }
            else
            {
                DataTable dataTable = Select($"drop table Hall_{mass[2]}");
            }
            Update();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            Close();
        }

        private void Hall_Click(object sender, RoutedEventArgs e)
        {
            string f = sender.ToString();
            string[] mass = f.Split(' ');
            App.hallId = Convert.ToInt32(mass[2]);

            Halls halls = new Halls();
            halls.Show();
            Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            int randint = 1;

            Random random = new Random();
            randint = random.Next(1, 12);

            int id = App.hallId;

            MessageBox.Show(id.ToString());

            DataTable data = Select($"USE [Kassir] CREATE TABLE[dbo].[Hall_{id}]( [numberOfRow][int] IDENTITY(1, 1) NOT NULL,"+
                $" [column1][int] NULL, [column2][int] NULL, [column3][int] NULL, [column4][int] NULL, [column5][int] NULL, [column6][int] NULL, [filmId][int] NULL) "+ 
                $"SET IDENTITY_INSERT [dbo].[Hall_{id}] ON "+
                $"INSERT[dbo].[Hall_{id}]([numberOfRow], [column1], [column2], [column3], [column4], [column5], [column6], [filmId]) VALUES(1, 0, 0, 0, 0, 0, 0, {randint})" +
                $"INSERT[dbo].[Hall_{id}]([numberOfRow], [column1], [column2], [column3], [column4], [column5], [column6], [filmId]) VALUES(2, 0, 0, 0, 0, 0, 0, {randint})" +
                $"INSERT[dbo].[Hall_{id}]([numberOfRow], [column1], [column2], [column3], [column4], [column5], [column6], [filmId]) VALUES(3, 0, 0, 0, 0, 0, 0, {randint})" +
                $"INSERT[dbo].[Hall_{id}]([numberOfRow], [column1], [column2], [column3], [column4], [column5], [column6], [filmId]) VALUES(4, 0, 0, 0, 0, 0, 0, {randint})" +
                $"INSERT[dbo].[Hall_{id}]([numberOfRow], [column1], [column2], [column3], [column4], [column5], [column6], [filmId]) VALUES(5, 0, 0, 0, 0, 0, 0, {randint})" +
                $"INSERT[dbo].[Hall_{id}]([numberOfRow], [column1], [column2], [column3], [column4], [column5], [column6], [filmId]) VALUES(6, 0, 0, 0, 0, 0, 0, {randint})");
            Update();
        }
    }
}
