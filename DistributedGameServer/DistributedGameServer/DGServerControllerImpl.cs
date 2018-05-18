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
    class DGServerControllerImpl : IDGServerController
    {
        private IDGPortalController m_portal;
        private IDGDataController m_database;
        private List<User> m_users;
        private List<Hero> m_heroes;
        private Dictionary<User, Hero> m_players;
        private Boss m_boss;
        private int m_count;
        /// <summary>
        /// Constructor
        /// </summary>
        public DGServerControllerImpl()
        {
            ConnectToPortal();
            m_users = new List<User>();
            m_count = -1;
            ConnectToDB();
            m_boss = SelectBoss();
            m_heroes = GetHeroes();
            m_players = new Dictionary<User, Hero>();
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
            ++m_count;
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
                Console.WriteLine(e4.Message);
                Environment.Exit(1);
            }
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
        /// GetHeroes
        /// gets heroes from the database
        /// </summary>
        /// <returns></returns>
        public List<Hero> GetHeroes()
        {
            List<Hero> heroes = new List<Hero>();
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
                heroes.Add(new Hero(i, name, hp, def, abilities));
            }

            return heroes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hero"></param>
        /// <param name="user"></param>
        public void SelectHero(Hero hero, User user)
        {
            m_players.Add(user, hero);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="boss"></param>
        /// <param name="players"></param>
        public void GetGameStats(out Boss boss, out Dictionary<User, Hero> players)
        {
            boss = m_boss;
            players = m_players;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetServerUrl()
        {
            return m_portal.GetServerInfo().Url;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Hero> GetHeroList()
        {
            return m_heroes;
        }
    }
}
