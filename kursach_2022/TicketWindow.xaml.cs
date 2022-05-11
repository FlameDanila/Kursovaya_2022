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
    /// Логика взаимодействия для TicketWindow.xaml
    /// </summary>
    public partial class TicketWindow : Window
    {
        public TicketWindow()
        {
            InitializeComponent();

            DataTable data = Select($"select * from dbo.films");

            List<string> source = new List<string>();
            source.AddRange(App.db.Films.Select(n => n.folder));

            List<string> description = new List<string>();
            description.AddRange(App.db.Films.Select(n => n.name));

            int top = 70;
            int bottom = 18;
            int counter = 0;

            for (int j = 0; j < 6; j++)
            {
                StackPanel horizontal = new StackPanel();
                horizontal.Width = 800;
                horizontal.Height = 500;
                horizontal.Orientation = Orientation.Horizontal;

                for (int i = 0; i < 2; i++)
                {
                    Image image = new Image();
                    image.Width = 300;
                    image.Height = 450;
                    image.HorizontalAlignment = HorizontalAlignment.Left;
                    image.VerticalAlignment = VerticalAlignment.Top;
                    image.Stretch = Stretch.Fill;
                    image.Cursor = Cursors.UpArrow;
                    image.ToolTip = description[counter];
                    image.MouseLeftButtonDown += Film_MouseLeftButtonDown;

                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(@source[counter], UriKind.Relative);
                    logo.EndInit();

                    image.Source = logo;

                    image.Margin = new Thickness(top, bottom, 0, 0);
                    horizontal.Children.Add(image);
                    counter++;
                }
                grid.Children.Add(horizontal);
                top = 70;
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

        private void Film_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var body = sender as Image;

            var list = App.db.Films.Where(n => n.name == body.ToolTip.ToString()).ToList();

            foreach (var i in list)
            {
                App.filname = body.ToolTip.ToString();
                App.hallId = i.id;
            }

            Halls menu = new Halls();
            menu.Show();
            Close();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            Close();
        }
    }
}
