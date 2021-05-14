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
    /// Логика взаимодействия для StudioChoice.xaml
    /// </summary>
    public partial class StudioChoice : Window
    {
        private string ConStr = @"Data Source=DESKTOP-FBTM7V3\SQLEXPRESS01; Initial Catalog=KursPr; Integrated Security=True;";
        public int UserID;

        private void ButME(object sender, RoutedEventArgs e)
        {
            string SN = ((Control)sender).Tag.ToString();
            StudioView SV = new StudioView(SN, UserID);
            SV.Show();
            this.Close();
        }

        private void WrapDataLoad()
        {
            string StudComm = "SELECT COUNT(*) FROM Studio";
            string SNC = "SELECT StudioName FROM Studio WHERE StudioID = @SID";
            string SLC = "SELECT Logo FROM Studio WHERE StudioID = @SID";

            using (SqlConnection Con = new SqlConnection(ConStr))
            {
                Con.Open();
                SqlCommand StView = new SqlCommand(StudComm, Con);
                int SV = (int)StView.ExecuteScalar();
                for (int SC = 1; SC <= SV; SC++)
                {
                    Grid SP = new Grid();
                    SP.Width = 480;
                    SP.Height = 270;
                    SP.Opacity = 1;
                    SP.Margin = new Thickness(10, 20, 10, 20);
                    Image PSTR = new Image();
                    PSTR.Width = 480;
                    PSTR.Height = 270;
                    PSTR.Margin = new Thickness(0, 0, 0, 0);
                    SqlCommand PV = new SqlCommand(SLC, Con);
                    PV.Parameters.AddWithValue("@SID", SC);
                    SqlDataReader PDT = PV.ExecuteReader();
                    PDT.Read();
                    var PSTRBTS = PDT["Logo"] as byte[];
                    MemoryStream MS = new MemoryStream();
                    MS.Write(PSTRBTS, 0, PSTRBTS.Length);
                    BitmapImage BI = new BitmapImage();
                    BI.BeginInit();
                    BI.StreamSource = MS;
                    BI.EndInit();
                    PSTR.Source = BI;
                    PDT.Close();
                    Button Bu = new Button();
                    SqlCommand SG = new SqlCommand(SNC, Con);
                    SG.Parameters.AddWithValue("@SID", SC);
                    SqlDataReader GNDT = SG.ExecuteReader();
                    Bu.Width = 480;
                    Bu.Height = 270;
                    Bu.Opacity = 0.25;
                    Bu.Background = Brushes.LightGray;
                    Bu.Foreground = null;
                    Bu.Margin = new Thickness(0, 0, 0, 0);
                    GNDT.Read();
                    Bu.Tag = GNDT["StudioName"].ToString();
                    GNDT.Close();
                    Bu.Click += new RoutedEventHandler(this.ButME);
                    SP.Children.Add(PSTR);
                    SP.Children.Add(Bu);
                    StudioO.Children.Add(SP);
                }
                Con.Close();
            }
        }

        public StudioChoice(int UID)
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
            GenreChoice SC = new GenreChoice(UserID);
            SC.Show();
            this.Close();
        }

        private void SCOpen(object sender, RoutedEventArgs e)
        {
            SPS.Opacity = 0;
        }

        private void PCOpen(object sender, RoutedEventArgs e)
        {
            PlatformChoice PC = new PlatformChoice(UserID);
            PC.Show();
            this.Close();
        }
    }
}
