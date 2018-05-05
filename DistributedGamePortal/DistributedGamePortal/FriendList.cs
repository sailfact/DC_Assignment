using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistributedGamePortal
{
    [DataContract]
    public class Friend
    {
        [DataMember]
        public int UserID { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Servername { get; set; }

        public Friend(int id, string username, string servername)
        {
            this.UserID = id;
            this.Username = username;
            this.Servername = servername;
        }

    }

    [DataContract]
    public class FriendList
    {
        [DataMember]
        public List<Friend> Friends { get; set; }

        public FriendList(List<Friend> friends)
        {
            this.Friends = friends;
        }

        public FriendList()
        {
            this.Friends = new List<Friend>();
        }

        public void AddFriend(Friend newFriend)
        {
            Friends.Add(newFriend);
        }
    }
}
