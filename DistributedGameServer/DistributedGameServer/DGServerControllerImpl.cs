using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DistributedGamePortal;
using DistributedGameData;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;

namespace DistributedGameServer
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
                     UseSynchronizationContext = false)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     UseSynchronizationContext = false)]
    class DGServerControllerImpl : IDGServerController
    {
        private delegate bool GameOperation();
        private IDGPortalController m_portal;
        private IDGDataController m_database;
        private List<User> m_users;
        private List<Hero> m_heroes;
        private Dictionary<Guid, Hero> m_players;
        private Dictionary<Guid, IDGServerControllerCallback> m_clients;
        private Boss m_boss;
        private Server m_serverInfo;
        /// <summary>
        /// Constructor
        /// </summary>
        public DGServerControllerImpl()
        {
            m_players = new Dictionary<Guid, Hero>();
            m_clients = new Dictionary<Guid, IDGServerControllerCallback>();
            ConnectToPortal();
            ConnectToDB();
            m_boss = SelectBoss();
            m_heroes = GetHeroes();
        }

        ~DGServerControllerImpl()
        {
            if (m_portal != null)
            {
                m_portal.Unsubscribe(m_serverInfo);
            }
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
                m_serverInfo = m_portal.Subscribe();
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
            int index = random.Next(m_database.GetNumBosses(out err) - 1);
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

            for (int i = 0; i < m_database.GetNumHeroes(out err); ++i)
            {
                m_database.GetHeroStatsByID(i, out int def, out int hp, out int moveNum, out err);
                name = m_database.GetHeroNameByID(i, out err);
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
        public void SelectHero(Guid id, Hero hero)
        {
            if (m_players.Count >= 12)
            {
                m_players.Add(id, hero);
            }

            if (m_players.Count == 5)
            {
                GameOperation gameDel = Game;
                AsyncCallback callback = this.GameOnComplete;

                gameDel.BeginInvoke(callback, null);
                Console.WriteLine("Begining Game");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="boss"></param>
        /// <param name="players"></param>
        public void GetGameStats(out Boss boss, out Dictionary<Guid, Hero> players)
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
            return m_serverInfo.Url;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Hero> GetHeroList()
        {
            return m_heroes;
        }

        private bool Game()
        {
            char strategy = m_boss.TargetStrategy;
            Random rnd = new Random();
            int index, value, abilityIdx, targIdx;
            char target, type;
            while (m_boss.HealthPoints != 0 && AreAlive())
            {
                // boss turn
                if (strategy == 'R') // attack random
                {
                    index = rnd.Next(m_players.Count);
                    m_players.ElementAt(index).Value.TakeDamage(m_boss.Attack());
                }
                else if (strategy == 'H')   // attack highest damage
                {   // for now attack first hero
                    m_players.ElementAt(0).Value.TakeDamage(m_boss.Attack());
                }

                // player turns
                foreach (var client in m_clients)
                {
                    if (m_players[client.Key].HealthPoints > 0)
                    {
                        do
                        {
                            client.Value.TakeTurn(m_players[client.Key], out abilityIdx, out targIdx);
                        }
                        while (abilityIdx > -1 && targIdx > -1 && abilityIdx < m_players[client.Key].Abilities.Count && targIdx > m_players.Count);
                        value = m_players[client.Key].UseAbility(abilityIdx, out type, out target);

                        if (type == 'H')
                        {
                            if (target == 'M')
                                HealMulti(value);
                            else if (target == 'S')
                                HealHero(targIdx, value);
                        }
                        else if (type == 'D')
                        {
                            m_boss.TakeDamage(value);
                        }
                    }
                }    
            }
            return true;
        }

        /// <summary>
        /// AreAlive
        /// </summary>
        /// <returns></returns>
        private bool AreAlive()
        {
            foreach (var player in m_players)
            {
                if (player.Value.HealthPoints != 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// HealMulti
        /// </summary>
        /// <param name="value"></param>
        private void HealMulti(int value)
        {
            foreach (var player in m_players)
            {
                player.Value.Heal(value);
            }
        }

        /// <summary>
        /// HealHero
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        private void HealHero(int index, int value)
        {
            m_players.ElementAt(index).Value.Heal(value);
        }

        /// <summary>
        /// Subscribe
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Guid Subscribe()
        {
            IDGServerControllerCallback callback = OperationContext.Current.GetCallbackChannel<IDGServerControllerCallback>();
            Guid clientID = Guid.NewGuid();

            if (callback != null)
            {
                m_clients.Add(clientID, callback);
            }

            return clientID;
        }

        /// <summary>
        /// Unsubscribe
        /// </summary>
        /// <param name="id"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Unsubscribe(Guid id)
        {
            if (m_clients.ContainsKey(id))
            {
                m_clients.Remove(id);
            }

            if (m_players.ContainsKey(id))
            {
                m_players.Remove(id);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        private void GameOnComplete(IAsyncResult result)
        {
            bool iAddResult;
            GameOperation del;
            AsyncResult asyncObj = (AsyncResult)result;
            
            if (asyncObj.EndInvokeCalled == false)
            { 
                del = (GameOperation)asyncObj.AsyncDelegate;
                iAddResult = del.EndInvoke(asyncObj); 
            }
            asyncObj.AsyncWaitHandle.Close();

            foreach (var client in m_clients)
            {
                client.Value.NotifyGameEnded();
            }

            if (m_players.Count >= 5)
            {
                GameOperation game = Game;
                AsyncCallback callback = GameOnComplete;

                game.BeginInvoke(callback, null);
                Console.WriteLine("Begining Game");
            }
        }
    }
}
