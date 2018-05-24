using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DistributedGameDatabase;

namespace DistributedGameData
{
    /// <summary>
    /// DGDataController
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, 
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     UseSynchronizationContext = false)]
    class DGDataControllerImpl : IDGDataController
    {
        DistributedGameDB m_gameDB;
        /// <summary>
        /// DGDataControllerImpl
        /// Constructer for Data Server Object calls
        /// DGDLLWrapper.InitBD to initialise the DataBase
        /// </summary>
        public DGDataControllerImpl()
        {
            try
            {
                Console.WriteLine("Client Connected");
                m_gameDB = new DistributedGameDB();
                m_gameDB.InitDB();
            }
            catch (DllNotFoundException e1)
            {
                Console.WriteLine(e1.Message);
            }
            catch (DirectoryNotFoundException e2)
            {
                Console.WriteLine(e2.Message);
            }
            catch (FileNotFoundException e3)
            {
                Console.WriteLine(e3.Message);
            }
        }

        /// <summary>
        /// GetBossNameByID
        /// takes an ID and returns the corresponding
        /// Name of a Boss
        /// or null if error occurs
        /// </summary>
        /// <param name="id"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public string GetBossNameByID(int id)
        {
            try
            {
                return m_gameDB.GetBossNameByID(id);
            }
            catch (DllNotFoundException)
            {
                throw new FaultException<DataServerFault>(new DataServerFault("DGDLLWrapper.GetBossNameByID", "DllNotFoundException"));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new FaultException<DataServerFault>(new DataServerFault("DGDLLWrapper.GetBossNameByID", "ArgumentOutOfRangeException"));
            }
        }

        /// <summary>
        /// GetBossStatsByID
        /// takes an ID for a boss and returns the stats via reference
        /// returns true if successful or false if error
        /// returns The 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="def"></param>
        /// <param name="hp"></param>
        /// <param name="damage"></param>
        /// <param name="targetPref"></param>
        /// <returns></returns>
        public void GetBossStatsByID(int id, out int def, out int hp, out int damage, out char targetPref)
        {
            def = 0;
            hp = 0;
            damage = 0;
            targetPref = '0';
            try
            {
                m_gameDB.GetBossStatsByID(id, out def, out hp, out damage, out targetPref);
            }
            catch(DllNotFoundException)
            {
                throw new FaultException<DataServerFault>(new DataServerFault("DGDLLWrapper.GetBossStatsByID", "DllNotFoundException"));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new FaultException<DataServerFault>(new DataServerFault("DGDLLWrapper.GetBossStatsByID", "ArgumentOutOfRangeException"));
            }
        }

        /// <summary>
        /// GetGriendsByID
        /// gets a list of friends for a given user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<string> GetFriendsByID(int id)
        {
            try
            {
                return m_gameDB.GetFriendsByID(id);
            }
            catch (DllNotFoundException)
            {
                throw new FaultException<DataServerFault>(new DataServerFault("DGDLLWrapper.GetFriendsByID", "DllNotFoundException"));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new FaultException<DataServerFault>(new DataServerFault("DGDLLWrapper.GetFriendsByID", "ArgumentOutOfRangeException"));
            }
        }

        /// <summary>
        /// GetHeroNameByID
        /// returns the name of a Hero for the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetHeroNameByID(int id)
        {
            try
            {
                return m_gameDB.GetHeroNameByID(id);
            }
            catch (DllNotFoundException)
            {
                throw new FaultException<DataServerFault>(new DataServerFault("DGDLLWrapper.GetHeroNameByID", "DllNotFoundException"));
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new FaultException<DataServerFault>(new DataServerFault("DGDLLWrapper.GetHeroNameByID", "ArgumentOutOfRangeException"));
            }
        }

        /// <summary>
        /// GetHeroStatsByID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="def"></param>
        /// <param name="hp"></param>
        /// <param name="moveNum"></param>
        /// <returns></returns>
        public void GetHeroStatsByID(int id, out int def, out int hp, out int moveNum)
        {
            def = 0;
            hp = 0;
            moveNum = 0;
            try
            {
                m_gameDB.GetHeroStatsByID(id, out def, out hp, out moveNum);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new FaultException<DataServerFault>(new DataServerFault("DGDLLWrapper.GetHeroStatsByID", "ArgumentOutOfRangeException"));
            }
            catch (DllNotFoundException)
            {
                throw new FaultException<DataServerFault>(new DataServerFault("DGDLLWrapper.GetHeroStatsByID", "DllNotFoundException"));
            }
        }

        /// <summary>
        /// GetMovesByIDAndIndex
        /// </summary>
        /// <param name="id"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <param name="description"></param>
        /// <param name="type"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public void GetMovesByIDAndIndex(int id, int index, out int value, out string description, out char type, out char target)
        {
            value = 0;
            description = null;
            type = '0';
            target = '0';
            try
            {
                m_gameDB.GetMovesByIDAndIndex(id, index, out value, out description, out type, out target);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new FaultException<DataServerFault>(new DataServerFault("DGDLLWrapper.GetMovesByIDAndIndex", "ArgumentOutOfRangeException"));
            }
            catch (DllNotFoundException)
            {
                throw new FaultException<DataServerFault>(new DataServerFault("DGDLLWrapper.GetMovesByIDAndIndex", "DllNotFoundException"));
            }
        }

        /// <summary>
        /// GetNumBosses
        /// </summary>
        /// <returns></returns>
        public int GetNumBosses()
        {
            try
            {
                return m_gameDB.GetNumBosses();
            }
            catch (DllNotFoundException)
            {
                throw new FaultException<DataServerFault>(new DataServerFault("DGDLLWrapper.GetNumBosses", "DllNotFoundException"));
            }
        }

        /// <summary>
        /// GetNumHeroes
        /// </summary>
        /// <returns></returns>
        public int GetNumHeroes()
        {
            try
            {
                return m_gameDB.GetNumHeroes();
            }
            catch (DllNotFoundException)
            {
                throw new FaultException<DataServerFault>(new DataServerFault("DGDLLWrapper.GetNumHeroes", "DllNotFoundException"));
            }
        }

        /// <summary>
        /// GetNumUsers
        /// </summary>
        /// <returns></returns>
        public int GetNumUsers()
        {
            try
            {
                return m_gameDB.GetNumUsers();
            }
            catch (DllNotFoundException)
            {
                throw new FaultException<DataServerFault>(new DataServerFault("DGDLLWrapper.GetNumUsers", "DllNotFoundException"));
            }
        }

        /// <summary>
        /// GetUsernamePassword
        /// returns the given username and id for the given id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="passwd"></param>
        /// <returns></returns>
        public void GetUsernamePassword(int id, out string username, out string passwd)
        {
            username = null;
            passwd = null;
            try
            {
                m_gameDB.GetUsernamePassword(id, out username, out passwd);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new FaultException<DataServerFault>(new DataServerFault("DGDLLWrapper.GetUsernamePassword", "ArgumentOutOfRangeException"));
            }
            catch (DllNotFoundException)
            {
                throw new FaultException<DataServerFault>(new DataServerFault("DGDLLWrapper.GetUsernamePassword", "DllNotFoundException"));
            }
        }
    }
}