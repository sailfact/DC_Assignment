using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistributedGamePortal
{
    /// <summary>
    /// Server
    /// stores information about a server
    /// </summary>
    [DataContract]
    public class Server
    {
        [DataMember]
        public int ServerID { get; set; }

        [DataMember]
        public string Url { get; set; }

        [DataMember]
        public string Name { get; set; }

        public Server(int id, string url, string name)
        {
            this.ServerID = id;
            this.Url = url;
            this.Name = name;
        }
    }
}
