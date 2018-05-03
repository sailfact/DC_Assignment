using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    internal class DGPortalControllerImpl : IDGPortalController
    {
        private IDGDataController m_database;
        private int m_serverCount;
        private delegate bool VerifyOperation(string op1, string op2);

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

        public void VerifyUserAsync(string username, string passwd)
        {
            VerifyOperation verifyDel = VerifyUser;
            AsyncCallback callback;

            callback = this.VerifyUser_OnComplete;
            verifyDel.BeginInvoke(username, passwd, callback, OperationContext.Current.GetCallbackChannel<IDGPortalControllerCallback>());
            Console.WriteLine("Verifying Username & password...");
        }

        public void VerifyUser_OnComplete(IAsyncResult res)
        {
            bool iResult = false;
            VerifyOperation verifyDel;
            IDGPortalControllerCallback ob = null;
            AsyncResult asyncObj = (AsyncResult)res;

            try
            {
                if (asyncObj.EndInvokeCalled == false)
                {
                    verifyDel = (VerifyOperation)asyncObj.AsyncDelegate;
                    ob = (IDGPortalControllerCallback)asyncObj.AsyncState;
                    iResult = verifyDel.EndInvoke(asyncObj);
                }
            }
            catch (CommunicationException e)
            {
                Console.WriteLine("\nError: Sending User Verification to Client\n" + e.Message);
            }
        }
    }
}
