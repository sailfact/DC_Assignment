using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    [DataContract]
    public class FriendList
    {
        [DataMember]
        public List<string> Friends { get; set; }

        public FriendList(List<string> friends)
        {
            this.Friends = friends;
        }

        public FriendList()
        {
            this.Friends = new List<string>();
        }

        public void AddFriend(string newFriend)
        {
            Friends.Add(newFriend);
        }
    }
}
