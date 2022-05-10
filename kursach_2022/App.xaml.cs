using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace kursach_2022
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static KassirEntities db = new KassirEntities();
        public static string name = "";
        public static int korzina = 0;
        public static int hallId = 0;
        public static int userId = 0;
    }
}
