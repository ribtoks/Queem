using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace DesignDrafts
{
    // TODO make normal class and delete this one:
    public static class Configuration
    {
        public static string DataFolderName = "data";
        public static BitmapImage KidPicture;
        public static BitmapImage YoungPicture;
        public static BitmapImage AdultPicture;

        static Configuration()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string slash = System.IO.Path.DirectorySeparatorChar.ToString();
            string kidPath = path + "data\\user_kid.png";
            string youngPath = path + "data\\user_young.png";
            string adultPath = path + "data\\user_adult.png";

            KidPicture = new BitmapImage(new Uri(kidPath));
            YoungPicture = new BitmapImage(new Uri(youngPath));
            AdultPicture = new BitmapImage(new Uri(adultPath));
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected PlayersXMLFile playersFile;        

        public MainWindow()
        {
            InitializeComponent();
            playersFile = new PlayersXMLFile();
            playersFile.Loaded += new EventHandler(playersFile_Loaded);
            playersFile.LoadPlayersFile();
        }

        void playersFile_Loaded(object sender, EventArgs e)
        {
            var args = (PlayersLoadedEventArgs)e;

            if (args.Collection == null)
            {
                // TODO write some code here
                // error occured

                return;
            }

            PlayerCollection players = Resources["PlayersList"] as PlayerCollection;

            foreach (var player in args.Collection)
            {
                players.Add(player);
            }

            playerSelect.ItemsSource = players;
        }
    }
}
