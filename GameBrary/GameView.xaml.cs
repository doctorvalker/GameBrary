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
using System.Windows.Shapes;
using System.IO;

namespace GameBrary
{
    /// <summary>
    /// Логика взаимодействия для GameView.xaml
    /// </summary>
    public partial class GameView : Window
    {
        private string ConStr = @"Data Source=DESKTOP-FBTM7V3\SQLEXPRESS01; Initial Catalog=KursPr; Integrated Security=True;";

        public int UserID;
        public string GameName;
        public int GameID (string Game)
        {
            string US = "SELECT * FROM Game WHERE GameName = @GN";

            using (SqlConnection CN = new SqlConnection(ConStr))
            {
                CN.Open();
                SqlCommand USC = new SqlCommand(US, CN);
                USC.Parameters.AddWithValue("@GN", Game);
                SqlDataReader UC = USC.ExecuteReader();
                UC.Read();
                int ID = (int)UC["GameID"];
                UC.Close();
                CN.Close();
                return ID;
            }
        }

        private void DataLoad(String GameName)
        {
            string DL = "SELECT * FROM Game WHERE GameName = @GN";
            string GeN = "SELECT GenreName From Genre INNER JOIN Game ON Genre.GenreID = Game.GenreID WHERE GameName = @GN";
            string PlN = "SELECT PlatformName From Platform INNER JOIN Game ON Platform.PlatformID = Game.PlatformID WHERE GameName = @GN";
            string StN = "SELECT StudioName From Studio INNER JOIN Game ON Studio.StudioID = Game.StudioID WHERE GameName = @GN";
            string AvScore = "SELECT AVG(Rating.Mark) AS AvS FROM dbo.Game INNER JOIN Rating ON Game.GameID = Rating.GameID WHERE Game.GameName = @GN"; 

            using (SqlConnection CN = new SqlConnection(ConStr))
            {
                CN.Open();
                SqlCommand GameInfo = new SqlCommand(DL, CN);
                GameInfo.Parameters.AddWithValue("@GN", GameName);
                SqlDataReader GI = GameInfo.ExecuteReader();
                GI.Read();
                GName.Text = GI["GameName"].ToString();
                GaDe.Text = GI["GameDescription"].ToString();
                var PSTR = GI["Poster"] as byte[];
                MemoryStream MSP = new MemoryStream();
                MSP.Write(PSTR, 0, PSTR.Length);
                BitmapImage BIP = new BitmapImage();
                BIP.BeginInit();
                BIP.StreamSource = MSP;
                BIP.EndInit();
                Poster.Source = BIP;
                var SCRN = GI["Screen"] as byte[];
                MemoryStream MSS = new MemoryStream();
                MSS.Write(SCRN, 0, SCRN.Length);
                BitmapImage BIS = new BitmapImage();
                BIS.BeginInit();
                BIS.StreamSource = MSS;
                BIS.EndInit();
                Screen.Source = BIS;
                GI.Close();
                SqlCommand GeInfo = new SqlCommand(GeN, CN);
                GeInfo.Parameters.AddWithValue("@GN", GameName);
                SqlDataReader Genre = GeInfo.ExecuteReader();
                Genre.Read();
                GeNa.Text = Genre["GenreName"].ToString();
                Genre.Close();
                SqlCommand PlInfo = new SqlCommand(PlN, CN);
                PlInfo.Parameters.AddWithValue("@GN", GameName);
                SqlDataReader Platform = PlInfo.ExecuteReader();
                Platform.Read();
                PlNa.Text = Platform["PlatformName"].ToString();
                Platform.Close();
                SqlCommand StInfo = new SqlCommand(StN, CN);
                StInfo.Parameters.AddWithValue("@GN", GameName);
                SqlDataReader Studio = StInfo.ExecuteReader();
                Studio.Read();
                StNa.Text = Studio["StudioName"].ToString();
                Studio.Close();
                SqlCommand AvgScore = new SqlCommand(AvScore, CN);
                AvgScore.Parameters.AddWithValue("@GN", GameName);
                SqlDataReader Avg = AvgScore.ExecuteReader();
                Avg.Read();
                float AvS = float.Parse(Avg["AvS"].ToString());
                AvS = (float)Math.Round(AvS, 2);
                AS.Text = AvS.ToString();
                Avg.Close();
                CN.Close();
            }
        }

        private bool USCheck(int User, string Game)
        {
            string US = "SELECT count(*) FROM Rating INNER JOIN Game ON Rating.GameID = Game.GameID WHERE UserID = @UID AND GameName = @GN";

            using (SqlConnection CN = new SqlConnection(ConStr))
            {
                CN.Open();
                SqlCommand USC = new SqlCommand(US, CN);
                USC.Parameters.AddWithValue("@UID", User);
                USC.Parameters.AddWithValue("@GN", Game);
                int UserScore = (int)USC.ExecuteScalar();
                if (UserScore == 1)
                {
                    CN.Close();
                    return true;
                }
                CN.Close();
            }
            return false;
        }

        private void USView(int User, string Game)
        {
            string US = "SELECT * FROM Rating INNER JOIN Game ON Rating.GameID = Game.GameID WHERE UserID = @UID AND GameName = @GN";

            using (SqlConnection CN = new SqlConnection(ConStr))
            {
                CN.Open();
                TextBlock UScore = new TextBlock();
                UScore.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
                UScore.FontSize = 25;
                UScore.Foreground = Brushes.White;
                UScore.Text = "Ваша оценка:";
                Grid.SetColumn(UScore, 10);
                Grid.SetRow(UScore, 12);
                SqlCommand USV = new SqlCommand(US, CN);
                USV.Parameters.AddWithValue("@UID", User);
                USV.Parameters.AddWithValue("@GN", Game);
                SqlDataReader UserSV = USV.ExecuteReader();
                TextBlock UserScore = new TextBlock();
                UserScore.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
                UserScore.FontSize = 25;
                UserScore.Foreground = Brushes.White;
                Grid.SetColumn(UserScore, 11);
                Grid.SetRow(UserScore, 12);
                UserSV.Read();
                UserScore.Text = UserSV["Mark"].ToString();
                UserSV.Close();
                GView.Children.Add(UScore);
                GView.Children.Add(UserScore);
                CN.Close();
            }
        }

        private void ScoreAdd(object sender, RoutedEventArgs e)
        {
            string II = "INSERT INTO Rating (Mark, GameID, UserID) VALUES (@Mark, @GID, @UID)";

            using (SqlConnection CN = new SqlConnection(ConStr))
            {
                CN.Open();
                SqlCommand INI = new SqlCommand(II, CN);
                float Mark = float.Parse(((Control)sender).Tag.ToString());
                INI.Parameters.AddWithValue("@Mark", Mark);
                INI.Parameters.AddWithValue("@GID", GameID(GameName));
                INI.Parameters.AddWithValue("@UID", UserID);
                INI.ExecuteNonQuery();
                CN.Close();
            }

            GameView GV = new GameView(GameName, UserID);
            GV.Show();
            this.Close();
        }

        private void ScoreCOpen(object sender, MouseEventArgs e)
        {
            var SC = GView.FindName("SChoice") as StackPanel;
            SC.Opacity = 1;
        }

        private void ScoreCClose(object sender, MouseEventArgs e)
        {
            var SC = GView.FindName("SChoice") as StackPanel; 
            SC.Opacity = 0;
        }

        private void USAdd(int User, string Game)
        {
            StackPanel ScoreChoice = new StackPanel();
            Grid.SetColumn(ScoreChoice, 11);
            Grid.SetRow(ScoreChoice, 12);
            Grid.SetRowSpan(ScoreChoice, 6);
            ScoreChoice.Background = Brushes.Black;
            ScoreChoice.Opacity = 0;
            ScoreChoice.Name = "SChoice";
            GView.RegisterName(ScoreChoice.Name, ScoreChoice);
            GView.Children.Add(ScoreChoice);
            for (int count = 1; count <= 5; count++)
            {
                Button Score = new Button();
                Score.Margin = new Thickness(0, 10, 0, 15);
                Score.Background = null;
                Score.BorderBrush = null;
                Score.Foreground = Brushes.White;
                Score.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
                Score.FontSize = 25;
                Score.Content = count.ToString();
                Score.Tag = count.ToString();
                Score.Click += new RoutedEventHandler(this.ScoreAdd);
                ScoreChoice.Children.Add(Score);
            }
            ScoreChoice.MouseLeave += new MouseEventHandler(this.ScoreCClose);
            Button UWR = new Button();
            UWR.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
            UWR.FontSize = 25;
            UWR.Foreground = Brushes.White;
            UWR.Background = null;
            UWR.BorderBrush = Brushes.White;
            UWR.Content = "Оценить";
            Grid.SetColumn(UWR, 10);
            Grid.SetRow(UWR, 12);
            UWR.MouseEnter += new MouseEventHandler(this.ScoreCOpen);
            UWR.MouseLeave += new MouseEventHandler(this.ScoreCClose);
            GView.Children.Add(UWR);
        }

        public GameView(string GN, int UID)
        {
            InitializeComponent();
            UserID = UID;
            GameName = GN;
            DataLoad(GN);
            if (USCheck(UID, GN) == true)
            {
                USView(UID, GN);
            }
            else
            {
                USAdd(UID, GN);
            }
            MMOpen.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
            GamOpen.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
            GenOpen.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
            SOpen.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
            POpen.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
            GName.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
            Genre.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
            Platform.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
            Studio.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
            GeNa.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
            PlNa.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
            StNa.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
            GaDe.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
            AvSc.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
            AS.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
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
