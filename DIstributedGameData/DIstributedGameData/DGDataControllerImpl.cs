using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIstributedGameData
{
    class DGDataControllerImpl : IDGDataController
    {
        /// <summary>
        /// DGDataControllerImpl
        /// Constructer for Data Server Object calls
        /// 'DGDLLWrapper.InitBD to initialise the DataBase
        /// </summary>
        public DGDataControllerImpl()
        {
            try
            {
                DGDLLWrapper.InitDB();
            }
            catch (DllNotFoundException)
            {
                Console.WriteLine("ERROR : failed to initialise DataBase");
                Environment.Exit(1);
            }
        }

        public string GetBossNameByID(int id, out string errMsg)
        {
            throw new NotImplementedException();
        }

        public bool GetBossStatsByID(int id, out int def, out int hp, out int damage, out char targetPref, out string errMsg)
        {
            throw new NotImplementedException();
        }

        public List<string> GetFriendsByID(int id, out string errMsg)
        {
            throw new NotImplementedException();
        }

        public string GetHeroNameByID(int id, out string errMsg)
        {
            throw new NotImplementedException();
        }

        public bool GetHeroStatsByID(int id, out int def, out int hp, out int moveNum, out string errMsg)
        {
            throw new NotImplementedException();
        }

        public bool GetMovesByIDAndIndex(int id, int index, out int value, out string description, out char type, out char target, out string errMsg)
        {
            throw new NotImplementedException();
        }

        public int GetNumBosses(out string errMsg)
        {
            throw new NotImplementedException();
        }

        public int GetNumHeroes(out string errMsg)
        {
            throw new NotImplementedException();
        }

        public int GetNumUsers(out string errMsg)
        {
            throw new NotImplementedException();
        }

        public bool GetUsernamePassword(int id, out string username, out string passwd, out string errMsg)
        {
            throw new NotImplementedException();
        }
    }
}
