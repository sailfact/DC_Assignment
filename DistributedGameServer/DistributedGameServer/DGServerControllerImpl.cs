using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DistributedGamePortal;

namespace DistributedGameServer
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     UseSynchronizationContext = false)]
    class DGServerControllerImpl : IDGServerController
    {
        private IDGPortalController m_portal;
        private List<User> m_users;
        private int m_count;
        private int m_serverID;

        public DGServerControllerImpl()
        {
            ChannelFactory<IDGPortalController> channelFactory;

            NetTcpBinding tcpBinding = new NetTcpBinding();
            string url = "net.tcp://localhost:50002/DGPortal";
            try
            {
                // incease default message size quota
                tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;
                tcpBinding.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;

                // bind channel to url
                channelFactory = new ChannelFactory<IDGPortalController>(tcpBinding, url);   // bind url to channel factory

                m_portal = channelFactory.CreateChannel();
                m_serverID = m_portal.GetServerID();
                m_users = new List<User>();
                m_count = -1;
            }
            catch (ArgumentNullException e1)
            {
                Console.WriteLine("\nError: Binding URL to ChannelFactory\n" + e1.Message);
                Environment.Exit(1);

            }
            catch (CommunicationObjectFaultedException e3)
            {
                Console.WriteLine("\nError: Communicating with Portal \n" + e3.Message);
                Environment.Exit(1);
            }
            catch (CommunicationException e2)
            {
                Console.WriteLine("\nError: Communicating with Portal \n" + e2.Message);
                Environment.Exit(1);
            }
            catch (InvalidOperationException e4)
            {
                Console.WriteLine("\nError: Modifying TcpBinding Message Quota\n" + e4.Message);
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        public bool AddUser(User newUser, out string errMsg)
        {
            errMsg = null;
            if (m_count < 12)
            {
                m_users.Add(newUser);
                ++m_count;
                return true;
            }
            else
            {
                errMsg = "Server is Full";
                return false;
            }
        }

        public int GetServerID()
        {
            return m_serverID;
        }

        public void OnVerifyUsersComplete(bool result)
        {
            throw new NotImplementedException();
        }
    }
}
