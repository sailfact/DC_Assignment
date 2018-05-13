using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using DistributedGameData;
using System.IO;

namespace test
{
    class Program
    {
        public static IDGDataController dataController;

        public static void Main(string[] args)
        {
            ConnectDB();
            string username = "Ross";
            string password = "password";
            User user;
            string err = null;

            for (int i = 0; i < dataController.GetNumUsers(out err); i ++)
            {
                dataController.GetUsernamePassword(i, out string un, out string pw, out err);
                Console.WriteLine("Username : {0}, Password : {1}", un, pw);
            }

            if (VerifyUser(username, password, out user))
                Console.WriteLine(user.UserName);
            else
                Console.WriteLine("fail");


            Console.ReadKey();
        }

        public static bool VerifyUser(string username, string password, out User user)
        {
            string errMsg = null;
            user = null;
            for (int i = 0; i < dataController.GetNumUsers(out errMsg); i++)
            {
                if (dataController.GetUsernamePassword(i, out string un, out string pw, out errMsg))
                {
                    if (username == un && password == pw)
                    {
                        FriendList list = new FriendList(dataController.GetFriendsByID(i, out errMsg));
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

        public static void ConnectDB()
        {
            ChannelFactory<IDGDataController> channelFactory;

            NetTcpBinding tcpBinding = new NetTcpBinding();
            string url = "net.tcp://localhost:50001/DGData";
            // incease default message size quota
            tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;
            tcpBinding.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;

            // bind channel to url
            channelFactory = new ChannelFactory<IDGDataController>(tcpBinding, url);   // bind url to channel factory

            dataController = channelFactory.CreateChannel();
        }
    }
}
