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
        private User m_user;
        public MainWindow()
        {
            m_user = null;
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
          
                SelectServer();
                if (m_server != null)
                {
                    SelectHero();
                }
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
            LoginWindow loginWind = null;
            bool done = true;
            do
            {
                loginWind = new LoginWindow();
                if (loginWind.ShowDialog() == true)
                {
                    if (m_portal.VerifyUser(loginWind.GetUsername(), loginWind.GetPassword(), out User user))
                    {
                        MessageBox.Show("Login Successful.");
                        m_user = user;
                        done = true;
                    }
                    else
                    {
                        MessageBox.Show("Unable to Login.");
                        done = false;
                    }
                }
            }
            while (!done);
        } 

        private void SelectServer()
        {
            ServerSelect select = new ServerSelect(m_portal.GetServerList());
            bool done = true;
            do
            {
                if (select.ShowDialog() == true && select.GetServer() != null)
                {
                    ConnectToServer(select.GetServer());
                    done = true;
                }
                else
                {
                    done = false;
                }
            }
            while (!done);
        }

        private void MenuItem_ClickFriends(object sender, RoutedEventArgs e)
        {
            DisplayFriendList friendWind = new DisplayFriendList(m_user.FriendList);
            friendWind.Show(); 
        }

        private void ConnectToServer(Server newServer)
        {
            DuplexChannelFactory<IDGServerController> channelFactory;

            NetTcpBinding tcpBinding = new NetTcpBinding();
            string url = newServer.Url;

            try
            {
                channelFactory = new DuplexChannelFactory<IDGServerController>(new InstanceContext(this), tcpBinding, url);   // bind url to channel factory
                m_server = channelFactory.CreateChannel();  // create portal on remote server
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

        private void SelectHero()
        {
            if (m_server != null)
            {
                HeroSelect heroWind = new HeroSelect(m_server.GetHeroList());
                heroWind.Show();
            }
        }

        private void MenuItem_ClickHeroes(object sender, RoutedEventArgs e)
        {
            SelectHero();
        }

        public void NotifyPlayerDied()
        {
            throw new NotImplementedException();
        }

        public void NotifyGameEnded()
        {
            throw new NotImplementedException();
        }

        private void MenuItem_ClickLogin(object sender, RoutedEventArgs e)
        {
            Login();
        }
    }
}
