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

        /// <summary>
        /// MainWindow
        /// Constructor 
        /// </summary>
        public MainWindow()
        {
            m_user = null;
            m_portal = null;
            m_server = null;
        }

        /// <summary>
        /// 
        /// </summary>
        ~MainWindow()
        {
            if (m_server != null)
            {
                m_server.Unsubscribe(m_user);
            }
        }

        /// <summary>
        /// Window_Loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                MessageBox.Show("Portal not avialable at this time, please  try again later\n");
                this.Close();
            }
        }

        /// <summary>
        /// Window_Closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                if (m_server != null)
                    m_server.Unsubscribe(m_user);

                this.Close();
            }
            catch (CommunicationObjectAbortedException)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Login
        /// </summary>
        private void Login()
        {
            LoginWindow loginWind = null;
            Verify verify = m_portal.VerifyUser;
            AsyncCallback callback = this.LoginOnComplete;
            
            loginWind = new LoginWindow();
            if (loginWind.ShowDialog() == true)
            {
                verify.BeginInvoke(loginWind.Username, loginWind.Password, out User user, callback, null);
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
                TxtBoxUsername.Text = user.UserName;

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
        /// SelectServer
        /// </summary>
        private void SelectServer()
        {
            ServerSelect select = new ServerSelect(m_portal.GetServerList());
            bool done = true;
            do
            {
                if (select.ShowDialog() == true && select.Server != null)
                {
                    ConnectToServer(select.Server);
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
        /// MenuItem_ClickFriends
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_ClickFriends(object sender, RoutedEventArgs e)
        {
            DisplayFriendList friendWind = new DisplayFriendList(m_user.FriendList);
            friendWind.Show(); 
        }

        /// <summary>
        /// ConnectToServer
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
                m_server.Subscribe(m_user);
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Error Connecting to Server, please  try again later\n");
                this.Close();
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Error Connecting to Server, please  try again later\n");
                this.Close();
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show("Server Endpoint not avialable at this time, please  try again later\n");
                this.Close();
            }
        }

        /// <summary>
        /// SelectHero
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
                        if ((hero = heroWind.Hero) != null)
                        {
                            m_server.SelectHero(m_user, hero);
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
        /// MenuItem_ClickHeroes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_ClickHeroes(object sender, RoutedEventArgs e)
        {
            SelectHero();
        }

        /// <summary>
        /// NotifyPlayerDied
        /// </summary>
        public void NotifyPlayerDied(User player)
        {
            MessageBox.Show(player.UserName + " has died");
        }

        /// <summary>
        /// NotifyGameEnded
        /// </summary>
        public bool NotifyGameEnded()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you wish to continue", "The Game Has Ended",  MessageBoxButton.YesNo,MessageBoxImage.Question);

            return messageBoxResult == MessageBoxResult.Yes ? true : false;
        }

        /// <summary>
        /// MenuItem_ClickLogin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_ClickLogin(object sender, RoutedEventArgs e)
        {
            Login();
        }

        /// <summary>
        /// TakeTurn
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="abilityIdx"></param>
        /// <param name="targetIdx"></param>
        public void TakeTurn(Hero hero, out int abilityIdx, out int targetIdx)
        {
            TakeTurn turn;
            targetIdx = -1;
            abilityIdx = -1;
            turn = new TakeTurn(hero);

            if (turn.ShowDialog() == true)
            {
                abilityIdx = turn.Ability.AbilityID;
                targetIdx = turn.Index;
            }
        }

        public void NotifyGameStats(Boss boss, Dictionary<User, Hero> heros)
        {
            this.Dispatcher.Invoke(() =>
            {
                TxtBoxName.Text = boss.BossName;
                TxtBoxHP.Text = boss.HealthPoints.ToString();
                TxtBoxDef.Text = boss.Defence.ToString();
            });
        }

        public void ServerFull()
        {
            MessageBox.Show("Server is full", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void NotifyGameStart()
        {
            MessageBox.Show("Game has started");
        }
    }
}
