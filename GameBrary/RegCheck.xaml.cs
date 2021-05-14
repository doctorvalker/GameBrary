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

namespace GameBrary
{
    /// <summary>
    /// Логика взаимодействия для RegCheck.xaml
    /// </summary>
    public partial class RegCheck : Window
    {
        private string ConStr = @"Data Source=DESKTOP-FBTM7V3\SQLEXPRESS01; Initial Catalog=KursPr; Integrated Security=True;";

        private bool AddUser()
        {
            string UpdComm = "INSERT INTO [User] (Nickname, Password) VALUES (@nick, @pass)";
            string NickCheck = "SELECT Count(*) FROM [User] WHERE Nickname = @Nc";

            using (SqlConnection Con = new SqlConnection(ConStr))
            {
                Con.Open();
                SqlCommand NCC = new SqlCommand(NickCheck, Con);
                NCC.Parameters.AddWithValue("@Nc", LogReg.Text);
                int NaCo = (int)NCC.ExecuteScalar();
                if (NaCo > 0)
                {
                    LogReg.Text = "";
                    LogReg.Foreground = Brushes.Red;
                    LogReg.Text = "Логин уже сущевствует";
                    return false;
                }
                else
                {
                    SqlCommand CMMND = new SqlCommand(UpdComm, Con);
                    CMMND.Parameters.AddWithValue("@nick", LogReg.Text);
                    CMMND.Parameters.AddWithValue("@pass", PassReg.Text);
                    CMMND.ExecuteNonQuery();
                    Con.Close();
                    return true;
                }                
            }
        }

        private int AddUID()
        {
            string PassComm = "SELECT * FROM [User] WHERE Nickname = @nick AND Password = @pass";

            using (SqlConnection Con = new SqlConnection(ConStr))
            {
                Con.Open();
                SqlCommand CMMND = new SqlCommand(PassComm, Con);
                CMMND.Parameters.AddWithValue("@nick", LogReg.Text);
                CMMND.Parameters.AddWithValue("@pass", PassReg.Text);
                SqlDataReader UsID = CMMND.ExecuteReader();
                UsID.Read();
                int ID = (int)UsID["UserID"];
                UsID.Close();
                Con.Close();
                return ID;
            }
        }

        public RegCheck()
        {
            InitializeComponent();
            EditUser.FontFamily = new FontFamily("Pixel Times");
        }

        private void MoveScreen(object sender, MouseEventArgs e)
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
            if (LogReg.Text == "Логин" || LogReg.Text == "Логин уже сущевствует")
            {
                LogReg.Clear();
                LogReg.Foreground = Brushes.Black;
            }
        }

        private void LogFill(object sender, RoutedEventArgs e)
        {
            if (LogReg.Text == "")
            {
                LogReg.Foreground = Brushes.LightGray;
                LogReg.Text = "Логин";
            }
        }

        private void PasClear(object sender, RoutedEventArgs e)
        {
            if (PassReg.Text == "Пароль")
            {
                PassReg.Clear();
                PassReg.Foreground = Brushes.Black;
            }
        }

        private void PasFill(object sender, RoutedEventArgs e)
        {
            if (PassReg.Text == "")
            {
                PassReg.Foreground = Brushes.LightGray;
                PassReg.Text = "Пароль";
            }
        }

        private void RegMenu(object sender, RoutedEventArgs e)
        {
            if (AddUser() == true)
            {
                MainMenu MM = new MainMenu(AddUID());
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
