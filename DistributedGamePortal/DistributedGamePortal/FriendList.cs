using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistributedGamePortal
{
    public enum Status { Online, Offline}

    [DataContract]
    public class Friend
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Status OnlineStatus { get; set; }

        public Friend(string name, Status status)
        {
            this.Name = name;
            this.OnlineStatus = status;
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
