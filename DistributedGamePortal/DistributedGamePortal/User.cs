using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistributedGamePortal
{
    /// <summary>
    /// User
    /// stores information about the users
    /// </summary>
    [DataContract]
    public class User
    {
        [DataMember]
        public int UserID { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Password { get; set; }
        
        [DataMember]
        public List<string> FriendList { get; set; }

        public User(int id, string username, string passwd,  List<string> friendList)
        {
            this.UserID = id;
            this.UserName = username;
            this.Password = passwd;
            this.FriendList = friendList;
        }
    }
}
