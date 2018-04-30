using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistributedGameServer
{
    [DataContract]
    class Hero
    {
        [DataMember]
        public int HeroID { get; set; }

        [DataMember]
        public string HeroName { get; set; }

        [DataMember]
        public int MaxHealthPoints { get; }

        [DataMember]
        public int HealthPoints
        {
            get => HealthPoints;
            set => HealthPoints = value > MaxHealthPoints ? MaxHealthPoints : value;
        }

        [DataMember]
        public int Defence { get; set; }

        [DataMember]
        public List<Ability> Abilities { get; set; }

        public Hero(int id, string name, int hp, int def, List<Ability> abilities)
        {
            this.HeroID = id;
            this.HeroName = name;
            this.MaxHealthPoints = hp;
            this.HealthPoints = hp;
            this.Defence = def;
            this.Abilities = abilities;
        }
    }
}
