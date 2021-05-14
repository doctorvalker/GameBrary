﻿using System;
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
    /// Логика взаимодействия для PlatformView.xaml
    /// </summary>
    public partial class PlatformView : Window
    {
        private string ConStr = @"Data Source=DESKTOP-FBTM7V3\SQLEXPRESS01; Initial Catalog=KursPr; Integrated Security=True;";
        public int UserID;

        private void DataLoad(string PlatName)
        {
            string DL = "SELECT * FROM Platform WHERE PlatformName = @SN";

            using (SqlConnection CN = new SqlConnection(ConStr))
            {
                CN.Open();
                SqlCommand StInfo = new SqlCommand(DL, CN);
                StInfo.Parameters.AddWithValue("@SN", PlatName);
                SqlDataReader PI = StInfo.ExecuteReader();
                PI.Read();
                PlNa.Text = PI["PlatformName"].ToString();
                PlDe.Text = PI["PlatformDescription"].ToString();
                var StLog = PI["PlatformPhoto"] as byte[];
                MemoryStream MSL = new MemoryStream();
                MSL.Write(StLog, 0, StLog.Length);
                BitmapImage BIL = new BitmapImage();
                BIL.BeginInit();
                BIL.StreamSource = MSL;
                BIL.EndInit();
                PtPh.Source = BIL;
                PI.Close();
                CN.Close();
            }
        }

        public PlatformView(string PN, int UID)
        {
            InitializeComponent();
            UserID = UID;
            DataLoad(PN);
            MMOpen.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
            GamOpen.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
            GenOpen.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
            SOpen.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
            POpen.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
            PlNa.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
            PlDe.FontFamily = new FontFamily("EA Font(RUS BY LYAJKA)");
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
