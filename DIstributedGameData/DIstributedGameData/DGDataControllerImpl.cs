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
        /// 'DGDLLWrapper.InitBD to initialise the DataBase
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
        public string GetBossNameByID(int id, out string errMsg)
        {
            errMsg = null;
            try
            {
                return m_gameDB.GetBossNameByID(id);
            }
            catch (DllNotFoundException)
            {
                errMsg = "ERROR : in DLL function 'DGDLLWrapper.GetBossNameByID'";
                Console.WriteLine(errMsg);
                return null;
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
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool GetBossStatsByID(int id, out int def, out int hp, out int damage, out char targetPref, out string errMsg)
        {
            errMsg = null;
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
                errMsg = "ERROR : in DLL function 'DGDLLWrapper.GetBossStatsByID'";
                Console.WriteLine(errMsg);
                return false;
            }
            return true;
        }

        /// <summary>
        /// GetGriendsByID
        /// gets a list of friends for a given user id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public List<string> GetFriendsByID(int id, out string errMsg)
        {
            errMsg = null;
            try
            {
                return m_gameDB.GetFriendsByID(id);
            }
            catch (DllNotFoundException)
            {
                errMsg = "ERROR : in DLL function 'DGDLLWrapper.GetFriendsByID'";
                Console.WriteLine(errMsg);
                return null;
            }
        }

        /// <summary>
        /// GetHeroNameByID
        /// returns the name of a Hero for the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public string GetHeroNameByID(int id, out string errMsg)
        {
            errMsg = null;
            try
            {
                return m_gameDB.GetHeroNameByID(id);
            }
            catch (DllNotFoundException)
            {
                errMsg = "ERROR : in DLL function 'DGDLLWrapper.GetHeroNameByID'";
                Console.WriteLine(errMsg);
                return null;
            }
        }

        /// <summary>
        /// GetHeroStatsByID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="def"></param>
        /// <param name="hp"></param>
        /// <param name="moveNum"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool GetHeroStatsByID(int id, out int def, out int hp, out int moveNum, out string errMsg)
        {
            errMsg = null;
            def = 0;
            hp = 0;
            moveNum = 0;
            try
            {
                m_gameDB.GetHeroStatsByID(id, out def, out hp, out moveNum);
            }
            catch (ArgumentOutOfRangeException)
            {
                errMsg = "ERROR : Invalid Arguments";
                Console.WriteLine(errMsg);
                return false;
            }
            catch (DllNotFoundException)
            {
                errMsg = "ERROR : in DLL function 'DGDLLWrapper.GetHeroStatsByID'";
                Console.WriteLine(errMsg);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <param name="description"></param>
        /// <param name="type"></param>
        /// <param name="target"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool GetMovesByIDAndIndex(int id, int index, out int value, out string description, out char type, out char target, out string errMsg)
        {
            errMsg = null;
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
                errMsg = "ERROR : Invalid Argument";
                Console.WriteLine(errMsg);
                return false;
            }
            catch (DllNotFoundException)
            {
                errMsg = "ERROR : in DLL function 'DGDLLWrapper.GetMovesByIDAndIndex'";
                Console.WriteLine(errMsg);
                return false;
            }

            return true;
        }

        /// <summary>
        /// GetNumBosses
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int GetNumBosses(out string errMsg)
        {
            errMsg = null;
            try
            {
                return m_gameDB.GetNumBosses();
            }
            catch (DllNotFoundException)
            {
                errMsg = "ERROR : in DLL function 'DGDLLWrapper.GetNumBosses'";
                Console.WriteLine(errMsg);
            }

            return -1;
        }

        /// <summary>
        /// GetNumHeroes
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int GetNumHeroes(out string errMsg)
        {
            errMsg = null;
            try
            {
                return m_gameDB.GetNumHeroes();
            }
            catch (DllNotFoundException)
            {
                errMsg = "ERROR : in DLL function 'DGDLLWrapper.GetNumHeroes'";
                Console.WriteLine(errMsg);
            }

            return -1;
        }

        /// <summary>
        /// GetNumUsers
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int GetNumUsers(out string errMsg)
        {
            errMsg = null;
            try
            {
                return m_gameDB.GetNumUsers();
            }
            catch (DllNotFoundException)
            {
                errMsg = "ERROR : in DLL function 'DGDLLWrapper.GetNumUsers'";
                Console.WriteLine(errMsg);
            }

            return -1;
        }

        /// <summary>
        /// GetUsernamePassword
        /// returns the given username and id for the given id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="passwd"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool GetUsernamePassword(int id, out string username, out string passwd, out string errMsg)
        {
            errMsg = null;
            username = null;
            passwd = null;
            try
            {
                m_gameDB.GetUsernamePassword(id, out username, out passwd);
            }
            catch (ArgumentOutOfRangeException e1)
            {
                errMsg = e1.Message;
                Console.WriteLine(errMsg);
                return false;
            }
            catch (DllNotFoundException e2)
            {
                errMsg = e2.Message;
                Console.WriteLine(errMsg);
                return false;
            }

            return true;
        }
    }
}