using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace GameBrary
{
    /// <summary>
    /// Логика взаимодействия для EntCheck.xaml
    /// </summary>
    public partial class EntCheck : Window
    {
        
        private string ConStr = @"Data Source=DESKTOP-FBTM7V3\SQLEXPRESS01; Initial Catalog=KursPr; Integrated Security=True;";

        private bool NickCheck()
        {
            string LogComm = "SELECT count(*) FROM [User] WHERE Nickname = @nick";

            using (SqlConnection Con = new SqlConnection(ConStr))
            {
                Con.Open();
                SqlCommand CMMND = new SqlCommand(LogComm, Con);
                CMMND.Parameters.AddWithValue("@nick", Login.Text);
                int NC = (int)CMMND.ExecuteScalar();
                if (NC  > 0)
                {
                    Con.Close();
                    return true;
                }
                else
                {
                    Login.Text = "";
                    Login.Foreground = Brushes.Red;
                    Login.Text = "Неверный логин";
                }
                Con.Close();
                return false;
            }
        }

        private bool PassCheck()
        {
            string PassComm = "SELECT count(*) FROM [User] WHERE Nickname = @nick AND Password = @pass";

            using (SqlConnection Con = new SqlConnection(ConStr))
            {
                Con.Open();
                SqlCommand CMMND = new SqlCommand(PassComm, Con);
                CMMND.Parameters.AddWithValue("@nick", Login.Text);
                CMMND.Parameters.AddWithValue("@pass", Password.Text);
                int PC = (int)CMMND.ExecuteScalar();
                if (PC > 0)
                {
                    Con.Close();
                    return true;
                }
                else
                {
                    Password.Text = "";
                    Password.Foreground = Brushes.Red;
                    Password.Text = "Неверный пароль";
                }
                return false;
            }
        }

        private int AddUID ()
        {
            string PassComm = "SELECT * FROM [User] WHERE Nickname = @nick AND Password = @pass";

            using (SqlConnection Con = new SqlConnection(ConStr))
            {
                Con.Open();
                SqlCommand CMMND = new SqlCommand(PassComm, Con);
                CMMND.Parameters.AddWithValue("@nick", Login.Text);
                CMMND.Parameters.AddWithValue("@pass", Password.Text);
                SqlDataReader UsID = CMMND.ExecuteReader();
                UsID.Read();
                int ID = (int)UsID["UserID"];
                UsID.Close();
                Con.Close();
                return ID;
            }
        }

        public EntCheck()
        {
            InitializeComponent();
            EntryMenu.FontFamily = new FontFamily("Pixel Times");
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

        private void LogClear(object sender, RoutedEventArgs e)
        {
            if (Login.Text == "Логин" || Login.Text == "Неверный логин")
            {
                Login.Clear();
                Login.Foreground = Brushes.Black;
            }
        }

        private void LogFill(object sender, RoutedEventArgs e)
        {
            if (Login.Text == "")
            {
                Login.Foreground = Brushes.LightGray;
                Login.Text = "Логин";
            }
        }

        private void PasClear(object sender, RoutedEventArgs e)
        {
            if (Password.Text == "Пароль" || Password.Text == "Неверный пароль")
            {
                Password.Clear();
                Password.Foreground = Brushes.Black;
            }
        }

        private void PasFill(object sender, RoutedEventArgs e)
        {
            if (Password.Text == "")
            {
                Password.Foreground = Brushes.LightGray;
                Password.Text = "Пароль";
            }
        }

        private void EntMenu(object sender, RoutedEventArgs e)
        {
            if (NickCheck() == true && PassCheck() == true)
            {
                MainMenu MM = new MainMenu(AddUID()) ;
                MM.Show();
                this.Close();
            }
        }

        private void CheckInput(object sender, TextCompositionEventArgs e)
        {
            if ((e.Text[0] >= '0' && e.Text[0] <= '9') || (e.Text[0] >= 'A' && e.Text[0] <= 'Z') || (e.Text[0] >= 'a' && e.Text[0] <= 'z')) e.Handled = false;
            else e.Handled = true;
        }
    }
}
