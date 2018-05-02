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
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     UseSynchronizationContext = false)]
    class DGPortalControllerImpl : IDGPortalController
    {
        private IDGDataController m_database;
        private int m_serverCount;

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
                m_serverCount = -1;
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

        public int GetServerID()
        {
            ++m_serverCount;
            return m_serverCount;
        }

        public bool VerifyUser(string username, string password)
        {
            string errMsg = null;
            for (int i = 0; i < m_database.GetNumHeroes(out errMsg); i ++)
            {
                if (m_database.GetUsernamePassword(i, out string un, out string pw, out errMsg))
                {
                    if (username == un && password == pw)
                    {
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
