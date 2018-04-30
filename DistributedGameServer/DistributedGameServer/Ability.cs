using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistributedGameServer
{
    [DataContract]
    class Ability
    {
        [DataMember]
        public int AbilityID { get; set; }

        [DataMember]
        public string AbilityName { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int Value { get; set; }

        [DataMember]
        public char Type { get; set; }

        [DataMember]
        public char Target { get; set; }

        public Ability(int id, string name, string desc, int value, char type, char target)
        {
            Random rnd = new Random();
            this.AbilityID = id;
            this.AbilityName = name;
            this.Description = desc;
            this.Value = rnd.Next(value / 4, value + 1);
            this.Type = type;
            this.Target = target;
        }
    }
}
