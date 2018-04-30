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
        public string UserName { get; set; }
    }

    [DataContract]
    public class FriendList
    {
        [DataMember]
        public List<Friend> Friends { get; set; }

        [DataMember]
        public int Count { get; set; }

        public FriendList()
        {
            Friends = new List<Friend>();
            Count= -1;
        }

        public FriendList(List<Friend> friends)
        {
            this.Friends = friends;
            Count = 0;
        }

        public Friend getFriend(int idx)
        {
            return Friends[idx];
        }
    }
}
