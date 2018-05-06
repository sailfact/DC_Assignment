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
            openfile();
            ConnectDB();
            string username, password, err;
            int users = dataController.GetNumUsers(out err);
            int heros = dataController.GetNumHeroes(out err);
            int bosses = dataController.GetNumBosses(out err);
        

            Console.WriteLine("Users = {0}, heros = {1}, bosses = {2}", users, heros, bosses);

            for (int i = 0; i < dataController.GetNumUsers(out err); i++)
            {
                List<string> list = dataController.GetFriendsByID(i, out err);
                list.ForEach(delegate (String name)
                {
                    Console.WriteLine(name);
                }); 

            }

            for (int i = 0; i < heros; i ++)
            {
                string name = dataController.GetHeroNameByID(i, out err);
                dataController.GetHeroStatsByID(i, out int def, out int hp, out int moveNum, out err);
                Console.WriteLine(name.ToString());
                Console.WriteLine("def = {0}, hp = {1}, moveNum = {2}", def, hp, moveNum);
                for (int j = 0; j < moveNum; j++)
                {
                    dataController.GetMovesByIDAndIndex(i, j, out int val, out string desc, out char type, out char targ, out err);
                    Console.WriteLine("value = {0}\n{1}\ntype = {2}\ntarg = {3}", val, desc, type, targ);
                }
            }

            Console.ReadKey();
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

        public static void openfile()
        {
            FileStream fs = new FileStream("C:/MMORPG_DB/highly_secure_user_file.txt", FileMode.Open, FileAccess.Read);
        }
    }
}
