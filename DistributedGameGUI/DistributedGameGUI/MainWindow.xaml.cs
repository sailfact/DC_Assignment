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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DistributedGameGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
        }

        private void MenuItem_Login(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWind = new LoginWindow();
            loginWind.ShowDialog();
        }

        private void MenuItem_Portal(object sender, RoutedEventArgs e)
        {
            PortalWindow portalWind = new PortalWindow();
            portalWind.ShowDialog();
        }

        private void MenuItem_Friends(object sender, RoutedEventArgs e)
        {
            FriendList friendWind = new FriendList();
            friendWind.Show(); ;
        }

        private void MenuItem_Heroes(object sender, RoutedEventArgs e)
        {
            HeroSelect heroWind = new HeroSelect();
            heroWind.Show();
        }
    }
}
