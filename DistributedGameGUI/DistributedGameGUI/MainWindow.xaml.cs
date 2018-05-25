﻿using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
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
        private Hero m_hero;
        private Ability m_ability;
        private int m_target;

        /// <summary>
        /// MainWindow
        /// Constructor 
        /// </summary>
        public MainWindow()
        {
            m_user = null;
            m_portal = null;
            m_server = null;
            m_ability = null;
            m_target = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        ~MainWindow()
        {
            CloseWindow();
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
                CloseWindow();
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Error Connecting to Portal, please  try again later\n");
                CloseWindow();
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show("Portal not avialable at this time, please  try again later\n");
                CloseWindow();
            }
        }

        /// <summary>
        /// Window_Closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Window_Closed(object sender, EventArgs e)
        {
            CloseWindow();
        }

        /// <summary>
        /// Login
        /// </summary>
        private void Login()
        {
            try
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
            catch (FaultException<PortalServerFault>)
            {
                MessageBox.Show("Error Authenticating User please try Again later");
                CloseWindow();
            }
            catch (CommunicationException)
            {
                MessageBox.Show("Error Communicating with Portal please try again later");
                CloseWindow();
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
                activityBox.Items.Add("Login Successful.");
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
                activityBox.Items.Add("Unable to Login Please try again");
                Login();
            }
        }

        /// <summary>
        /// SelectServer
        /// </summary>
        private void SelectServer()
        {
            try
            {
                ServerSelect select;
                bool done = false;
                do
                {
                    select = new ServerSelect(m_portal.GetServerList());
                    if (select.ShowDialog() == true)
                    { 
                        if (select.Server != null)
                        {
                            ConnectToServer(select.Server);
                            done = true;
                        }
                    }
                }
                while (!done);
            }
            catch (CommunicationException)
            {
                MessageBox.Show("Error Communicating with portal please try again later");
                CloseWindow();
            }
        }

        /// <summary>
        /// MenuItem_ClickFriends
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_ClickFriends(object sender, RoutedEventArgs e)
        {
            DisplayFriendList friendWind = new DisplayFriendList(m_portal.GetFriendList(m_user));
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
                CloseWindow();
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Error Connecting to Server, please  try again later\n");
                CloseWindow();
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show("Server Endpoint not avialable at this time, please  try again later\n");
                CloseWindow();
            }
        }

        /// <summary>
        /// SelectHero
        /// </summary>
        private void SelectHero()
        {
            try
            {
                if (m_server != null)
                {
                    HeroSelect heroWind = null;
                    Hero hero = null;
                    bool done = false;
                    do
                    {
                        heroWind = new HeroSelect(m_server.GetHeroList());
                        if (heroWind.ShowDialog() == true)
                        {
                            if ((hero = heroWind.Hero) != null)
                            {
                                m_hero = hero;
                                IvwAbilities.ItemsSource = hero.Abilities;
                                m_server.SelectHero(m_user, hero);
                                done = true;
                            }
                        }
                    }
                    while (!done);
                }
            }
            catch (FaultException<GameServerFault>)
            {
                MessageBox.Show("Error Communicating with Game Server");
                CloseWindow();
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
        public void NotifyClient(string msg)
        {
            this.Dispatcher.Invoke(() =>
            {
                activityBox.Items.Add(msg);
            });
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
        public void TakeTurn(out Ability ability, out int targetIdx)
        {
            do
            {
                this.Dispatcher.Invoke(() =>
                {
                    activityBox.Items.Add("It Your turn");
                    activityBox.Items.Add("Please Select Ability and target");
                });
                Thread.Sleep(5000); // wait 5 seconds

                ability = m_ability;
                targetIdx = m_target;
            } while (m_ability == null || m_target == -1);
        }

        public void NotifyGameStats(Boss boss, Dictionary<int, Hero> players, string lastAttacked)
        {
            this.Dispatcher.Invoke((Action)delegate 
            {
                TxtBoxName.Text = boss.BossName;
                TxtBoxHP.Text = boss.HealthPoints.ToString();
                TxtBoxDef.Text = boss.Defence.ToString();
                TxtBoxLastAtatcked.Text = lastAttacked;
                IvwPlayers.ItemsSource = players;
            });
        }

        private void IvwAbilities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
               m_ability = (Ability)e.AddedItems[0];
        }

        private void IvwPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            KeyValuePair<int, Hero> pair;
            if ((e.AddedItems.Count > 0))
            {
                pair = (KeyValuePair<int, Hero>)e.AddedItems[0];
                m_target = pair.Key;
            }
        }

        private void CloseWindow()
        {
            try
            {
                if (m_server != null)
                    m_server.Unsubscribe(m_user);

                if (m_portal != null)
                    m_portal.LogOff(m_user);
                Dispatcher.BeginInvoke(new ThreadStart(() => this.Close()));
            }
            catch (CommunicationException)
            {
                Dispatcher.BeginInvoke(new ThreadStart(() => this.Close()));
            }
        }
    }
}
