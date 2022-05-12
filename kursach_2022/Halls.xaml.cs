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
using word = Microsoft.Office.Interop.Word;

namespace kursach_2022
{
    /// <summary>
    /// Логика взаимодействия для Halls.xaml
    /// </summary>
    public partial class Halls : Window
    {
        public int counter = 0;
        public Halls()
        {
            InitializeComponent();
            Update();
        }

        public void Update()
        {
            DataTable data = Select($"select * from hall_{App.hallId}");

            string f = data.Rows[0][7].ToString();
            int m = Convert.ToInt32(f);

            var nameid = App.db.Films.Where(n => n.id == m);
            foreach (var item in nameid)
            {
                hallText.Text = item.name;
            }

            int top = 150;
            int bottom = 18;
            int schetchik = 0;

            for (int j = 0; j < data.Rows.Count; j++)
            {
                for (int i = 1; i < data.Columns.Count - 1; i++)
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
                        schetchik++;
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
                        button.Click += add_Click;
                        button.Name = "d" + (j + 1) + "s" + (i);

                        button.Margin = new Thickness(top, bottom, 0, 0);
                        grid.Children.Add(button);
                        top += 100;
                        schetchik++;
                    }
                }
                top = 150;
                bottom += 80;
            }
        }
        public DataTable Select(string selectSQL)
        {
            DataTable data = new DataTable("dataBase");

            string path = "ConnectionString.txt";

            string text = File.ReadAllText(path);

            string[] vs = text.Split('"');

            SqlConnection sqlConnection = new SqlConnection($"server = {vs[1]};Trusted_connection={vs[3]};DataBase={vs[5]};User={vs[7]};PWD={vs[9]}");
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

        public void add_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Content.Equals("Свободно"))
            {
                button.Content = "✓";
                counter++;
                App.buttonsColumn += button.Name + " ";
            }
            else if (button.Content.Equals("✓"))
            {
                button.Content = "Свободно";
                counter--;
                App.buttonsColumn.Replace(button.Name + " ", ""); 
            }

            costText.Text = (App.filmcost * counter).ToString() + " Рублей";
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            string[] buttonId = App.buttonsColumn.Split(' ');

            for (int i = 0; i < buttonId.Count() - 1; i++)
            {
                string[] vs = buttonId[i].Split('d');
                string text = "";

                for (int j = 1; j <= vs.Count(); j += 2)
                {
                    text += vs[j];
                    string[] textSplit = text.Split('s');
                    for (int h = 0; h < textSplit.Count(); h += 2)
                    {
                        DataTable data = Select($"UPDATE Hall_{App.hallId} SET column{textSplit[h+1]} = REPLACE(column{textSplit[h+1]}, '0', '1') WHERE numberOfRow = '{textSplit[h]}'");
                    }
                }
            }

            if (MessageBox.Show($"Вы точно хотите приобрести {counter} билетов за {costText.Text}?", "Уверены?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No) { }
            else
            {
                for (int i = 0; i < counter; i++)
                {
                    var application = new word.Application();
                    word.Document document = application.Documents.Add();

                    word.Paragraph nameParagraph = document.Paragraphs.Add();
                    word.Range nameRange = nameParagraph.Range;

                    nameRange.Text = "Название фильма " + App.filname + "\n" + "Стоимость " + App.filmcost + "\n" + "Длительность " + App.filmTime + "\n" + "Время сеанса от " + App.showTimeFilm;
                    nameRange.Font.Size = 30;
                    nameRange.Collapse(word.WdCollapseDirection.wdCollapseEnd);

                    application.Visible = true;
                    document.SaveAs2($@"Ticket-{i+1}.doc", FileMode.OpenOrCreate);
                }
            }
            Update();
        }
    }
}
