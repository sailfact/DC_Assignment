using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DistributedGameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = null;
            NetTcpBinding tcpBinding = new NetTcpBinding();
            string url = null;
            DGServerControllerImpl dGServer = new DGServerControllerImpl();

            try
            {
                // increases message quota to max
                tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;
                tcpBinding.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;

                url = dGServer.GetServerUrl();

                host = new ServiceHost(dGServer);   // host the implementing object
                host.AddServiceEndpoint(typeof(IDGServerController), tcpBinding, url);    // access via the interface class

                host.Open();        // enter listening state ready for client requests
                Console.WriteLine("Press Enter to exit");
                Console.ReadLine(); // block waiting for client requests
                host.Close();
            }
            catch (InvalidOperationException e1)
            {
                Console.WriteLine(e1.Message);
            }
            catch (TimeoutException e2)
            {
                Console.WriteLine(e2.Message);
            }
            catch (CommunicationObjectFaultedException e3)
            {
                Console.WriteLine(e3.Message);
            }
        }
    }
}
