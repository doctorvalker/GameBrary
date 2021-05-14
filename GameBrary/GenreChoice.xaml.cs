using System;
using System.Collections.Generic;
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
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;

namespace GameBrary
{
    /// <summary>
    /// Логика взаимодействия для GenreChoice.xaml
    /// </summary>
    public partial class GenreChoice : Window
    {
        private string ConStr = @"Data Source=DESKTOP-FBTM7V3\SQLEXPRESS01; Initial Catalog=KursPr; Integrated Security=True;";
        public int UserID;

        private void ButME(object sender, RoutedEventArgs e)
        {
            string GN = ((Control)sender).Tag.ToString();
            GenreView GV = new GenreView(GN, UserID);
            GV.Show();
            this.Close();
        }

        private void WrapDataLoad()
        {
            string GenComm = "SELECT COUNT(*) FROM Genre";
            string GNC = "SELECT GenreName FROM Genre WHERE GenreID = @GID";

            using (SqlConnection Con = new SqlConnection(ConStr))
            {
                Con.Open();
                SqlCommand GenView = new SqlCommand(GenComm, Con);
                int GV = (int)GenView.ExecuteScalar();
                for (int GC = 1; GC <= GV; GC++)
                {
                    Grid SP = new Grid();
                    SP.Width = 450;
                    SP.Height = 100;
                    SP.Opacity = 1;
                    SP.Margin = new Thickness(30, 20, 30, 20);
                    Button Bu = new Button();
                    SqlCommand GG = new SqlCommand(GNC, Con);
                    GG.Parameters.AddWithValue("@GID", GC);
                    SqlDataReader GNDT = GG.ExecuteReader();
                    Bu.Width = 450;
                    Bu.Height = 100;
                    Bu.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
                    Bu.FontSize = 30;
                    Bu.Background = null;
                    Bu.BorderBrush = null;
                    Bu.Foreground = Brushes.White;
                    Bu.Margin = new Thickness(0, 0, 0, 0);
                    GNDT.Read();
                    Bu.Content = GNDT["GenreName"].ToString();
                    Bu.Tag = GNDT["GenreName"].ToString();
                    GNDT.Close();
                    Bu.Click += new RoutedEventHandler(this.ButME);
                    SP.Children.Add(Bu);
                    GenreO.Children.Add(SP);
                }
            }

        }

        public GenreChoice(int UID)
        {
            InitializeComponent();
            UserID = UID;
            WrapDataLoad();
            MMOpen.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
            GamOpen.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
            GenOpen.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
            SOpen.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
            POpen.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
        }

        private void MoveScreen(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void HideWindow(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void WideWindow(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Normal)
            {
                this.WindowState = System.Windows.WindowState.Maximized;
            }
            else
            {
                this.WindowState = System.Windows.WindowState.Normal;
            }
        }

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void SPSOpen(object sender, RoutedEventArgs e)
        {
            SPS.Opacity = 1;
        }

        private void SPSClose(object sender, MouseEventArgs e)
        {
            SPS.Opacity = 0;
        }

        private void MainMOpen(object sender, RoutedEventArgs e)
        {
            MainMenu MM = new MainMenu(UserID);
            MM.Show();
            this.Close();
        }

        private void GameCOpen(object sender, RoutedEventArgs e)
        {
            GameChoice GC = new GameChoice(UserID);
            GC.Show();
            this.Close();
        }

        private void GenreCOpen(object sender, RoutedEventArgs e)
        {
            SPS.Opacity = 0;
        }

        private void SCOpen(object sender, RoutedEventArgs e)
        {
            StudioChoice SC = new StudioChoice(UserID);
            SC.Show();
            this.Close();
        }

        private void PCOpen(object sender, RoutedEventArgs e)
        {
            PlatformChoice PC = new PlatformChoice(UserID);
            PC.Show();
            this.Close();
        }
    }
}
