using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace DistributedGameData
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = null;
            NetTcpBinding tcpBinding = new NetTcpBinding();
            string url = "net.tcp://localhost:50001/DGData";
            DGDataControllerImpl dGData = new DGDataControllerImpl();

            try
            {
                // incease default message size quota
                tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;
                tcpBinding.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;

                host = new ServiceHost(dGData);     // host the implementing class
                host.AddServiceEndpoint(typeof(IDGDataController), tcpBinding, url);    // access via the interface class

                host.Open();    // enter listening state ready for client requests
                Console.WriteLine("Press Enter to Exit");
                Console.ReadLine();     // block waiting for client requests
            }
            catch (FaultException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ObjectDisposedException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (TimeoutException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (CommunicationObjectFaultedException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (host != null)
                    host.Close();
                Environment.Exit(0);
            }
        }
    }
}
