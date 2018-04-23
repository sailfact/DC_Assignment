using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DIstributedGameData
{
    class DGDLLWrapper
    {
        [DllImport("DistributedGameDLL.dll")]
        public static extern int InitDB();        [DllImport("DistributedGameDLL.dll")]        public static extern void GetUsernamePassword(int id, out string username, out string passwd);        [DllImport("DistributedGameDLL.dll")]        public static extern List<string> GetFriendsByID(int id);

        [DllImport("DistributedGameDLL.dll")]
        public static extern int GetNumHeroes();

        [DllImport("DistributedGameDLL.dll")]
        public static extern int GetNumBosses();        [DllImport("DistributedGameDLL.dll")]        public static extern int GetNumUsers();        [DllImport("DistributedGameDLL.dll")]        public static extern string GetHeroNameByID(int id);        [DllImport("DistributedGameDLL.dll")]        public static extern string GetBossNameByID(int id);

        [DllImport("DistributedGameDLL.dll")]
        public static extern void GetHeroStatsByID(int id, out int def, out int hp, out int moveNum);

        [DllImport("DistributedGameDLL.dll")]
        public static extern void GetBossStatsByID(int id, out int def, out int hp, out int damage, out char targetPref);        [DllImport("DistributedGameDLL.dll")]        public static extern void GetMovesByIDAndIndex(int id, int index, out int value, out string description, out char type, out char target);
    }
}
