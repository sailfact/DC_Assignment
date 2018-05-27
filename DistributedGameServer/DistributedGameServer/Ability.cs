using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistributedGameServer
{
    /// <summary>
    /// Ability
    /// stroes information for hero abilities
    /// </summary>
    [DataContract]
    public class Ability
    {
        [DataMember]
        public int AbilityID { get; set; }

        [DataMember]
        public string AbilityName { get; set; }

        [DataMember]
        public int Value { get; set; }

        [DataMember]
        public char Type { get; set; }

        [DataMember]
        public char Target { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="desc"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="target"></param>
        public Ability(int id, string name, int value, char type, char target)
        {
            Random rnd = new Random();
            this.AbilityID = id;
            this.AbilityName = name;
            this.Value = rnd.Next(value / 2, value + 1);
            this.Type = type;
            this.Target = target;
        }
    }
}