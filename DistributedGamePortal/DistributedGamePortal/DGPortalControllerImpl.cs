using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DistributedGameData;
using DistributedGameServer;

namespace DistributedGamePortal
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     UseSynchronizationContext = false)]
    class DGPortalControllerImpl : IDGPortalController
    {
        private IDGDataController m_database;
        private ServerList m_serverList;
        private delegate bool VerifyOperation(string username, string password, out User user);
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
                m_serverList = new ServerList();
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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddServerInfo(Server server)
        {
            m_serverList.AddServer(server);
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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool VerifyUser(string username, string password, out User user)
        {
            string errMsg = null;
            user = null;
            for (int i = 0; i < m_database.GetNumUsers(out errMsg); i ++)
            {
                if (m_database.GetUsernamePassword(i, out string un, out string pw, out errMsg))
                {
                    if (username == un && password == pw)
                    {
                        FriendList list = new FriendList(m_database.GetFriendsByID(i, out errMsg));
                        user = new User(i, un, pw, list);
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Server GetServerInfo()
        {
            int id = m_serverList.ServerCount + 1;
            string name = "DGServer" + id;
            string url = "net.tcp://localhost:6"+id.ToString("0000")+"/" + name;
            Server server = new Server(id, url, name);
            m_serverList.AddServer(server);
            return server;
        }
    }
}