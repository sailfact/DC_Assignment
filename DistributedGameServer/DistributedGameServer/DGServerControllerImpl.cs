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
        private List<Hero> m_heroes;
        private Dictionary<int, Hero> m_players;
        private Dictionary<User, IDGServerControllerCallback> m_clients;
        private Boss m_boss;
        private Server m_serverInfo;
        /// <summary>
        /// Constructor
        /// </summary>
        public DGServerControllerImpl()
        {
            m_players = new Dictionary<int, Hero>();
            m_clients = new Dictionary<User, IDGServerControllerCallback>();
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
        /// ConnectToDB
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
            List<Ability> abilities;
            int val;
            string name, desc;
            char type, targ;

            for (int i = 0; i < m_database.GetNumHeroes(out err); ++i)
            {
                abilities = new List<Ability>();
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
        /// SelectHero
        /// </summary>
        /// <param name="hero"></param>
        /// <param name="user"></param>
        public void SelectHero(User user, Hero hero)
        {
            if (m_players.Count < 12)
            {
                m_players.Add(user.UserID, hero);
            }
            else
            {
                m_clients[user].ServerFull();
            }

            //if (m_players.Count == 5)
            //{
                GameOperation gameDel = Game;
                AsyncCallback callback = this.GameOnComplete;

                gameDel.BeginInvoke(callback, null);
                Console.WriteLine("Begining Game");
            //}
        }

        /// <summary>
        /// GetServerUrl
        /// </summary>
        /// <returns></returns>
        public string GetServerUrl()
        {
            return m_serverInfo.Url;
        }

        /// <summary>
        /// GetHeroList
        /// </summary>
        /// <returns></returns>
        public List<Hero> GetHeroList()
        {
            return m_heroes;
        }

        /// <summary>
        /// Game
        /// </summary>
        /// <returns></returns>
        private bool Game()
        {
            char strategy = m_boss.TargetStrategy;
            Random rnd = new Random();
            int dmg, index = 0, value, abilityIdx, targIdx, highestDmg = m_players.First().Key;
            char target, type;
            string msg = "";
            
            foreach (var client in m_clients)
            {
                client.Value.NotifyGameStart();
            }

            Console.WriteLine("Game Started");
            while (m_boss.HealthPoints > 0 && AreAlive())
            {
                foreach (var client in m_clients)
                {
                    client.Value.NotifyGameStats(m_boss, m_players);
                }
                // boss turn
                Console.WriteLine("Boss Turn");
                if (strategy == 'R') // attack random
                    index = m_players.Keys.ElementAt(rnd.Next(m_players.Keys.Count()));
                else if (strategy == 'H')   // attack highest damage
                    index = highestDmg;

                dmg = m_boss.Attack;
                m_players[index].TakeDamage(dmg);
                msg += m_boss.BossName + " Has dealt " + dmg + " damage to " + m_players[index].HeroName;
                if (m_players[index].HealthPoints == 0)
                    msg += "\n" + m_players[index].HeroName + " Has Perished.";
                foreach (var client in m_clients)
                {
                    client.Value.NotifyMove(msg);
                }
                msg = "";

                // player turns
                foreach (var client in m_clients)
                {
                    if (m_players.ContainsKey(client.Key.UserID) && m_players[client.Key.UserID].HealthPoints > 0)
                    {
                        do
                        {
                            client.Value.TakeTurn(m_players[client.Key.UserID], out abilityIdx, out targIdx);
                        }
                        while (abilityIdx > -1 && targIdx > -1 && abilityIdx < m_players[client.Key.UserID].Abilities.Count && targIdx > m_players.Count);

                        value = m_players[client.Key.UserID].UseAbility(abilityIdx, out type, out target);
                        msg += m_players[client.Key.UserID].HeroName + " used " + m_players[client.Key.UserID].Abilities.ElementAt(abilityIdx).AbilityName;
                        if (type == 'H')
                        {
                            if (target == 'M')
                            {
                                HealMulti(value);
                                msg += "\n" + m_players[client.Key.UserID].HeroName + " Healed everyone  by " + value;
                            }
                            else if (target == 'S')
                            {
                                HealHero(targIdx, value);
                                msg += "\n" + m_players[client.Key.UserID].HeroName + " Healed " + m_players[targIdx].HeroName + " by " + value;
                            }
                        }
                        else if (type == 'D')
                        {
                            m_boss.TakeDamage(value);
                            msg += "\n" + m_players[client.Key.UserID].HeroName + " dealt " + value + " damage to boss";
                            if (m_boss.HealthPoints == 0)
                                msg += "\n" + m_players[client.Key.UserID].HeroName + " has slain " + m_boss.BossName;
                        }
                    }
                    foreach (var cli in m_clients)
                    {
                        cli.Value.NotifyMove(msg);
                    }
                }    
            }

            foreach (var client in m_clients)
            {
                if (!client.Value.NotifyGameEnded())
                    return false;
            }
            return true;
        }

        /// <summary>
        /// AreAlive
        /// </summary>
        /// <returns></returns>
        private bool AreAlive()
        {
            bool alive = false;
            foreach (var player in m_players)
            {
                if (player.Value.HealthPoints > 0)
                {
                    alive = true;
                }
            }

            return alive;
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
        public bool Subscribe(User user)
        {
            IDGServerControllerCallback callback = OperationContext.Current.GetCallbackChannel<IDGServerControllerCallback>();

            if (callback != null)
            {
                m_clients.Add(user, callback);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Unsubscribe
        /// </summary>
        /// <param name="id"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Unsubscribe(User user)
        {
            if (m_clients.ContainsKey(user))
            {
                m_clients.Remove(user);
            }

            if (m_players.ContainsKey(user.UserID))
            {
                m_players.Remove(user.UserID);
            }
        }

        /// <summary>
        /// GameOnComplete
        /// </summary>
        /// <param name="result"></param>
        private void GameOnComplete(IAsyncResult result)
        {
            bool iResult = false;
            GameOperation del;
            AsyncResult asyncObj = (AsyncResult)result;
            
            if (asyncObj.EndInvokeCalled == false)
            { 
                del = (GameOperation)asyncObj.AsyncDelegate;
                iResult = del.EndInvoke(asyncObj); 
            }
            asyncObj.AsyncWaitHandle.Close();

            if (m_players.Count >= 5 && iResult)
            {
                GameOperation game = Game;
                AsyncCallback callback = GameOnComplete;
                foreach (var player in m_players)
                {
                    player.Value.MaxHeal();
                }
                m_boss = SelectBoss();
                game.BeginInvoke(callback, null);
                Console.WriteLine("Begining Game");
            }
        }
    }
}
