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
        private List<Server> m_serverList;
        private List<User> m_users;
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
                m_users = new List<User>();
                m_serverList = new List<Server>();
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
        /// GetServeList
        /// </summary>
        /// <returns>returns the list of available servers</returns>
        public List<Server> GetServerList()
        {
            return m_serverList;
        }

        /// <summary>
        /// Verify User
        /// checks with the datatier if the user info is valid and returns a user object
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>returns boolean and user object</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool VerifyUser(string username, string password, out User user)
        {
            user = null;
            try
            {
                for (int i = 0; i < m_database.GetNumUsers(); i ++)
                {
                    m_database.GetUsernamePassword(i, out string un, out string pw);
                    if (username == un && password == pw)
                    {
                        FriendList list = new FriendList();
                        user = new User(i, un, pw, m_database.GetFriendsByID(i));
                        m_users.Add(user);
                        return true;
                    }
                }
            }
            catch (FaultException<DataServerFault> )
            {
                throw new FaultException<PortalServerFault>(new PortalServerFault("DGPortalController.VerifyUser", "FaultException", "Login Server was unable to retrieve login information from the data server"));
            }
            catch (CommunicationObjectFaultedException)
            {
                throw new FaultException<PortalServerFault>(new PortalServerFault("DGPortalController.VerifyUser", "CommunicationObjectFaultedException", "An unexpected error occurred in the Portal please try later"));
            }

            return false;
        }

        /// <summary>
        /// Subscribe 
        /// used by servers to get their hosting information 
        /// and add the server to the server list
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Server Subscribe()
        {
            int id = m_serverList.Count + 1;
            string name = "DGServer" + id;
            string url = "net.tcp://localhost:6" + id.ToString("0000") + "/" + name;
            Server server = new Server(id, url, name);
            m_serverList.Add(server);
            return server;
        }

        /// <summary>
        /// Unsubscribed
        /// used by server to remove server info from the list of servers
        /// </summary>
        /// <param name="server"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Unsubscribe(Server server)
        {
            if (m_serverList.Contains(server))
            {
                m_serverList.Remove(server);
            }
        }

        /// <summary>
        /// LogOff
        /// used by user to remove the user from the list of users when the disconnect 
        /// </summary>
        /// <param name="user"></param>
        public void LogOff(User user)
        {
            if (m_users.Contains(user))
            {
                m_users.Remove(user);
            }
        }

        /// <summary>
        /// GetFriendList
        /// takes a given user 
        /// and allocates a friendlist
        /// </summary>
        /// <param name="user"></param>
        /// <returns>returns a friendlist</returns>
        public FriendList GetFriendList(User user)
        {
            List<string> names = user.FriendList;
            FriendList friendList = new FriendList();
            bool found;
            foreach (var name in names)
            {
                found = false;
                foreach (var usr in m_users)
                {
                    if (usr.UserName == name)   // if the user is online
                        found = true;
                }
                friendList.AddFriend(new Friend(name, found ? Status.Online : Status.Offline)); 
            }

            return friendList;
        }
    }
}