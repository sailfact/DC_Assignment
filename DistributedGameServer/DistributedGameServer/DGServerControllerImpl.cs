using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DistributedGamePortal;
using DistributedGameData;

namespace DistributedGameServer
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, 
                     UseSynchronizationContext = false)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                     ConcurrencyMode = ConcurrencyMode.Single,
                     UseSynchronizationContext = false)]
    class DGServerControllerImpl : IDGServerController, IDGPortalControllerCallback
    {
        private IDGPortalController m_portal;
        private IDGDataController m_database;
        private List<User> m_users;
        private List<Hero> m_heros;
        private Dictionary<User, Hero> m_gameHeros;
        private Boss m_boss;
        private int m_usercount;
        private int m_serverID;
        /// <summary>
        /// Constructor
        /// </summary>
        public DGServerControllerImpl()
        {
            ConnectToPortal();
            m_serverID = m_portal.GetServerID();
            m_users = new List<User>();
            m_heros = GetHeros();
            m_gameHeros = new Dictionary<User, Hero>();
            m_boss = null;
            m_usercount = -1;
            ConnectToDB();
        }

        /// <summary>
        /// AddUser 
        /// Callback function for IDGPortalContollerImpl
        /// adds user to user list
        /// </summary>
        /// <param name="newUser"></param>
        public void AddUser(User newUser)
        {
            m_users.Add(newUser);
            ++m_usercount;
        }
        /// <summary>
        /// 
        /// </summary>
        public void ConnectToDB()
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
        public void ConnectToPortal()
        {
            DuplexChannelFactory<IDGPortalController> channelFactory;

            NetTcpBinding tcpBinding = new NetTcpBinding();
            string url = "net.tcp://localhost:50002/DGPortal";
            try
            {
                // incease default message size quota
                tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;
                tcpBinding.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;

                // bind channel to url
                channelFactory = new DuplexChannelFactory<IDGPortalController>(new InstanceContext(this), tcpBinding, url);   // bind url to channel factory

                m_portal = channelFactory.CreateChannel();  
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
        public void SetUpGame()
        {
            m_boss = SelectBoss();
            
        }

        /// <summary>
        /// SelectBoss
        /// gets a random boss from the server and 
        /// assigns it to m_boss
        /// </summary>
        /// <returns>Boss</returns>
        public Boss SelectBoss()
        {
            Random random = new Random();
            string err = null;
            int index = random.Next(m_database.GetNumBosses(out err)-1);
            string name = m_database.GetBossNameByID(index, out err);
            m_database.GetBossStatsByID(index, out int def, out int hp, out int damage, out char targPref, out err);
            return new Boss(index, name, hp, def, damage, targPref);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Hero> GetHeros()
        {
            List<Hero> heros = new List<Hero>();
            string err = null;
            List<Ability> abilities = new List<Ability>();
            int val;
            string name, desc;
            char type, targ;

            for(int i = 0; i < m_database.GetNumHeroes(out err); ++i)
            {
                m_database.GetHeroStatsByID(i, out int def, out int hp, out int moveNum, out err);
                name  = m_database.GetHeroNameByID(i, out err);
                m_database.GetMovesByIDAndIndex(i, 0, out val, out desc, out type, out targ, out err);
                abilities.Add(new Ability(0, "Move1", desc, val, type, targ));
                m_database.GetMovesByIDAndIndex(i, 1, out val, out desc, out type, out targ, out err);
                abilities.Add(new Ability(1, "Move2", desc, val, type, targ));
                heros.Add(new Hero(i, name, hp, def, abilities));
            }

            return heros;
        }
        /// <summary>
        /// SelectHero
        /// </summary>
        /// <param name="hero"></param>
        public void SelectHero(Hero hero, int userID)
        {
            m_gameHeros.Add(m_users[userID], hero);
        }
        /// <summary>
        /// DisplayStats
        /// </summary>
        /// <param name="boss"></param>
        /// <param name="heroes"></param>
        public void GetGameStats(out Boss boss, out Dictionary<User, Hero> heros)
        {
            boss = m_boss;
            heros = m_gameHeros;
        }
    }
}
