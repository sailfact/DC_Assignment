using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
        private delegate bool Verify(string username, string password, out User user);
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
            Verify verify = m_portal.VerifyUser;
            AsyncCallback callback = this.LoginOnComplete;
            
            loginWind = new LoginWindow();
            if (loginWind.ShowDialog() == true)
            {
                verify.BeginInvoke(loginWind.GetUsername(), loginWind.GetPassword(), out User user, callback, null);
            }
        } 

        /// <summary>
        /// LoginOnComplete
        /// Callback function for Login
        /// </summary>
        /// <param name="res"></param>
        private void LoginOnComplete(IAsyncResult res)
        {
            bool result = false;
            Verify del;
            User user = null;
            AsyncResult asyncObj = (AsyncResult)res;

            if (asyncObj.EndInvokeCalled == false)
            {
                del = (Verify)asyncObj.AsyncDelegate;
                result = del.EndInvoke(out user, asyncObj);
            }
            asyncObj.AsyncWaitHandle.Close();

            if (result)
            {
                MessageBox.Show("Login Successful.");
                m_user = user;

                SelectServer();
                if (m_server != null)
                {
                    SelectHero();
                }
            }
            else
            {
                MessageBox.Show("Unable to Login.");
                Login();
            }
        }

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_ClickFriends(object sender, RoutedEventArgs e)
        {
            DisplayFriendList friendWind = new DisplayFriendList(m_user.FriendList);
            friendWind.Show(); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newServer"></param>
        private void ConnectToServer(Server newServer)
        {
            DuplexChannelFactory<IDGServerController> channelFactory;

            NetTcpBinding tcpBinding = new NetTcpBinding();
            string url = newServer.Url;

            try
            {
                channelFactory = new DuplexChannelFactory<IDGServerController>(new InstanceContext(this), tcpBinding, url);   // bind url to channel factory
                m_server = channelFactory.CreateChannel();  // create portal on remote server
                m_server.AddUser(m_user);
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

        /// <summary>
        /// 
        /// </summary>
        private void SelectHero()
        {
            if (m_server != null)
            {
                HeroSelect heroWind = null;
                Hero hero = null;
                bool done = true;
                do
                {
                    heroWind = new HeroSelect(m_server.GetHeroList());
                    if (heroWind.ShowDialog() == true)
                    {    
                        if ((hero = heroWind.GetHero()) != null)
                        {
                            m_server.SelectHero(hero, m_user);
                            done = true;
                        }
                        else
                        {
                            done = false;
                        }
                    }
                }
                while (!done);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_ClickHeroes(object sender, RoutedEventArgs e)
        {
            SelectHero();
        }

        /// <summary>
        /// 
        /// </summary>
        public void NotifyPlayerDied()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public void NotifyGameEnded()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_ClickLogin(object sender, RoutedEventArgs e)
        {
            Login();
        }
    }
}
