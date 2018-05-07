using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
using DistributedGamePortal;
using DistributedGameServer;

namespace DistributedGameGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
                      UseSynchronizationContext = false)]
    public partial class MainWindow : Window, IDGServerControllerCallback
    {
        private IDGPortalController m_portal;
        private IDGServerController m_server;
        public MainWindow()
        {
            m_portal = null;
            m_server = null;
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        { 
            ChannelFactory<IDGPortalController> channelFactory;

            NetTcpBinding tcpBinding = new NetTcpBinding();
            string url = "net.tcp://localhost:50002/DGPortal";
            try
            {
                channelFactory = new ChannelFactory<IDGPortalController>(tcpBinding, url);   // bind url to channel factory
                m_portal = channelFactory.CreateChannel();  // create portal on remote server
                Login();
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Error Connecting to Portal, please  try again later\n");
                this.Close();
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Error Connecting to Portal, please  try again later\n");
                this.Close();
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show("Service not avialable at this time, please  try again later\n");
                this.Close();
            }
        }

        private void Login()
        {
            LoginWindow loginWind = new LoginWindow();
            if (loginWind.ShowDialog() == true)
            {
               if (m_portal.VerifyUser(loginWind.GetUsername(), loginWind.GetPassword()))
                {
                    MessageBox.Show("Login Successful");
                }
                else
                {
                    MessageBox.Show("Unable to Login");
                    Login();    // reopen login window
                }
            }
        } 

        private void MenuItem_Friends(object sender, RoutedEventArgs e)
        {
            DisplayFriendList friendWind = new DisplayFriendList(m_portal.GetFriendList());
            friendWind.Show(); 
        }

        private void MenuItem_Heroes(object sender, RoutedEventArgs e)
        {
            HeroSelect heroWind = new HeroSelect();
            heroWind.Show();
        }
    }
}
