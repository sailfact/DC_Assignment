using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DistributedGameData;
using DistributedGameServer;

namespace DistributedGamePortal
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     UseSynchronizationContext = false)]
    class DGPortalControllerImpl : IDGPortalController
    {
        private IDGDataController m_database;
        private User m_user;
        private ServerList m_serverList;
        private IDGPortalControllerCallback m_callback;
        /// <summary>
        /// Constructor
        /// Connects to the data base
        /// </summary>
        public DGPortalControllerImpl()
        {
            ChannelFactory<IDGDataController> channelFactory;

            NetTcpBinding tcpBinding = new NetTcpBinding();
            string url = "net.tcp://localhost:50001/DGData";
            try
            {
                // incease default message size quota
                tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;
                tcpBinding.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;

                // bind channel to url
                channelFactory = new ChannelFactory<IDGDataController>(tcpBinding, url);   // bind url to channel factory

                m_database = channelFactory.CreateChannel();  // create database on remote server
                m_user = null;
                m_callback = OperationContext.Current.GetCallbackChannel<IDGPortalControllerCallback>();
            }
            catch (ArgumentNullException e1)
            {
                Console.WriteLine("\nError: Binding URL to ChannelFactory\n" + e1.Message);
                Environment.Exit(1);

            }
            catch (CommunicationException e2)
            {
                Console.WriteLine("\nError: Communicating with Data Server \n" + e2.Message);
                Environment.Exit(1);
            }
            catch (InvalidOperationException e3)
            {
                Console.WriteLine("\nError: Modifying TcpBinding Message Quota\n" + e3.Message);
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="server"></param>
        public void AddServerInfo(Server server)
        {
            m_serverList.AddServer(server);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FriendList GetFriendList()
        {
            return m_user != null ? m_user.FriendList : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetServerID()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ServerList GetServerList()
        {
            return m_serverList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool VerifyUser(string username, string password, out int clientID)
        {
            string errMsg = null;
            m_user = null;
            clientID = -1;
            for (int i = 0; i < m_database.GetNumHeroes(out errMsg); i ++)
            {
                if (m_database.GetUsernamePassword(i, out string un, out string pw, out errMsg))
                {
                    if (username == un && password == pw)
                    {
                        FriendList list = new FriendList(m_database.GetFriendsByID(i, out errMsg));
                        m_user = new User(i, un, pw, list);
                        clientID = 1;
                        return true;
                    }
                }
                else
                {
                    Console.WriteLine(errMsg);
                    return false;
                }
            }

            return false;
        }
    }
}
