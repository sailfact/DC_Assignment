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
        public int Value
        {
            get
            {
                Random rnd = new Random();
                return rnd.Next(Value / 2, Value + 1);
            }

            set => Value = value;
        }

        [DataMember]
        public char Type { get; set; }

        [DataMember]
        public char Target { get; set; }

        public Ability(int id, string name, string desc, int value, char type, char target)
        {
            this.AbilityID = id;
            this.AbilityName = name;
            this.Description = desc;
            this.Value = value;
            this.Type = type;
            this.Target = target;
        }
    }
}