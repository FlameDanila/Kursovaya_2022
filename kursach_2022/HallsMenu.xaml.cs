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
    /// Логика взаимодействия для HallsMenu.xaml
    /// </summary>
    public partial class HallsMenu : Window
    {
        public int number = 0;
        public HallsMenu()
        {
            InitializeComponent();

            Update();
        }

        public void Update()
        {
            grid.Children.Clear();

            DataTable data = Select("SELECT TABLE_NAME AS [Название таблицы] FROM INFORMATION_SCHEMA.TABLES WHERE table_type = 'BASE TABLE' and TABLE_NAME like 'hall_%'");

            DataTable table = Select("select name from films");

            DataTable hallsTable = new DataTable();

            List<string> listOfHalls = new List<string>();
            List<string> list = new List<string>();

            for (int j = 0; j < data.Rows.Count; j++)
            {
                listOfHalls.Add(data.Rows[j][0].ToString());
                App.hallId = j + 1;
            }

            int top = 80;
            int bottom = 33;

            int counter = 0;
            int enabled = 0;

            for (int i = 1; i < 4; i++)
            {
                for (int g = 1; g < 4; g++)
                {
                    if (listOfHalls.Count > counter)
                    {
                        Button button = new Button();

                        button.Width = 131;
                        button.Height = 112;
                        button.HorizontalAlignment = HorizontalAlignment.Left;
                        button.VerticalAlignment = VerticalAlignment.Top;
                        button.Background = null;
                        button.Content = "Зал " + (counter+1);

                        hallsTable = Select($"select filmid from hall_{counter + 1}");

                        List<int> filmidformhalls = new List<int>();
                        filmidformhalls.Clear();
                        filmidformhalls.Add(Convert.ToInt32(hallsTable.Rows[0][0]));
                        DataTable dataTable = Select($"select name from films where id = {filmidformhalls[0]}");

                        list.Add(dataTable.Rows[0][0].ToString());

                        button.ToolTip = list[counter];
                        button.BorderThickness = new Thickness(5);
                        button.Foreground = Brushes.Black;
                        button.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FF0365CF");
                        button.Margin = new Thickness(top, bottom, 0, 0);
                        button.MouseRightButtonDown += Dell_Click;
                        button.FontSize = 30;

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

            string path = "ConnectionString.txt";

            string text = File.ReadAllText(path);

            string[] vs = text.Split('"');

            //SqlConnection sqlConnection = new SqlConnection($@"server = DESKTOP-ITVEB8Q\SQLEXPRESS;Trusted_connection=No;DataBase=Kassir;User=ws1;PWD=ws1");
            SqlConnection sqlConnection = new SqlConnection($"server = {vs[1]};Trusted_connection={vs[3]};DataBase={vs[5]};User={vs[7]};PWD={vs[9]}");
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

            if (MessageBox.Show("Вы точно хотите удалить этот зал?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No) { }
            else
            {
                if (Convert.ToInt32(mass[2]) == 1)
                {
                    MessageBox.Show("Нельзя удалять зал, пока в нём проходит сеанс");
                }
                else
                {
                    DataTable dataTable = Select($"drop table Hall_{mass[2]}");
                }
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

            DataTable table = Select("SELECT TABLE_NAME AS [Название таблицы] FROM INFORMATION_SCHEMA.TABLES WHERE table_type = 'BASE TABLE' and TABLE_NAME like 'hall_%'");

            List<int> childfilms = new List<int>();
            DataTable filmsId = Select("select id from films where showTime < '18:00:00'");
            DataTable filmsId2 = Select("select id from films where showTime >= '18:00:00' and showTime < '20:00:00'");
            DataTable filmsId3 = Select("select id from films where showTime >= '20:00:00'");

            for (int g = 0; g < filmsId.Rows.Count; g++)
            {
                childfilms.Add(Convert.ToInt32(filmsId.Rows[g][0]));
            }

            List<int> middlefilms = new List<int>();

            for (int g = 0; g < filmsId2.Rows.Count; g++)
            {
                middlefilms.Add(Convert.ToInt32(filmsId2.Rows[g][0]));
            }

            List<int> oldfilms = new List<int>();

            for (int g = 0; g < filmsId3.Rows.Count; g++)
            {
                oldfilms.Add(Convert.ToInt32(filmsId3.Rows[g][0]));
            }

            List<int> listOfHalls = new List<int>();

            for (int j = 0; j < table.Rows.Count; j++)
            {
                listOfHalls.Add(Convert.ToInt32(table.Rows[j][0].ToString().Replace("Hall_","")));
                App.hallId = listOfHalls[j]+1;
            }

            int randint = 1;

            if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour < 18)
            {
                randint = childfilms[number];
                number++;
                if (number == childfilms.Count)
                {
                    number = 0;
                }

            }
            if (DateTime.Now.Hour > 12 && DateTime.Now.Hour <= 18)
            {
                randint = middlefilms[number];
                number++;
                if (number == middlefilms.Count)
                {
                    number = 0;
                }
            }
            if (DateTime.Now.Hour > 18)
            {
                randint = oldfilms[number];
                number++;
                if (number == oldfilms.Count)
                {
                    number = 0;
                }
            }
            int id = App.hallId;

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