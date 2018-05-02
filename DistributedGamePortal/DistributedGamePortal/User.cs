using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistributedGamePortal
{
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
        public List<User> FriendList { get; set; }

        public User(int id, string username, string passwd,  List<User> friendList)
        {
            this.UserID = id;
            this.UserName = UserName;
            this.Password = passwd;
            this.FriendList = friendList;
        }
    }
}
