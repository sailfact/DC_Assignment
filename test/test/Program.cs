using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using DistributedGameData;
using DistributedGamePortal;
using System.IO;

namespace test
{
    class Program
    {
        public static IDGDataController dataController;

        public static void Main(string[] args)
        {
            ConnectDB();
            Dictionary<int, Player> dict = new Dictionary<int, Player>();
            dict.Add(1, new Player("ross", 100));
            dict.Add(2, new Player("alex", 200));
            dict.Add(3, new Player("liam", 300));
            dict.Add(4, new Player("charlie", 400));

            foreach (var item in dict)
            {
                item.Value.TakeDamage(10);
            }

            foreach (var item in dict)
            {
                Console.WriteLine("key = {0}, name = {1}, health = {2}", item.Key, item.Value.Name, item.Value.Health);
            }
            

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

    public class Player
    {
        private string name;
        private int health;

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                this.name = value;
            }
        }
            
        public int Health
        {
            get
            {
                return health;
            }

            set
            {
                this.health = value;
            }
        }

        public Player(string name, int health)
        {
            this.name = name;
            this.health = health;
        }

        

        public void TakeDamage(int dam)
        {
            health -= dam;
        }
    
    }
}
