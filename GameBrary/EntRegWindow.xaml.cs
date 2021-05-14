using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для EntRegWindow.xaml
    /// </summary>
    public partial class EntRegWindow : Window
    {
        public EntRegWindow()
        {
            InitializeComponent();
            EntryButton.FontFamily = new FontFamily("Pixel Times");
            RegButton.FontFamily = new FontFamily("Pixel Times");
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

        private void EntryWin(object sender, RoutedEventArgs e)
        {
            EntCheck EntryCheck = new EntCheck();
            EntryCheck.Show();
            this.Close();
        }

        private void RegisWin(object sender, RoutedEventArgs e)
        {
            RegCheck RegistCheck = new RegCheck();
            RegistCheck.Show();
            this.Close();
        }
    }
}
