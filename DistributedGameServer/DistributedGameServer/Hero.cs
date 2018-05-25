using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistributedGameServer
{
    [DataContract]
    public class Hero
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int HeroID { get; set; }

        [DataMember]
        public string HeroName { get; set; }

        [DataMember]
        public int MaxHealthPoints { get; set; }

        [DataMember]
        public int HealthPoints { get; set; }

        [DataMember]
        public int Defence { get; set; }

        [DataMember]
        public List<Ability> Abilities { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="hp"></param>
        /// <param name="def"></param>
        /// <param name="abilities"></param>
        public Hero(int id, string name, int hp, int def, List<Ability> abilities)
        {
            this.HeroID = id;
            this.HeroName = name;
            this.MaxHealthPoints = hp;
            this.HealthPoints = hp;
            this.Defence = def;
            this.Abilities = abilities;
        }

        public void TakeDamage(int damage)
        {
            int dmg = damage - Defence;
            if (dmg < 0)
                dmg = 0;

            HealthPoints -= (dmg);
            if (HealthPoints < 0)
            {
                HealthPoints = 0;
            }
        }

        public void Heal(int health)
        {
            HealthPoints += health;
            if (HealthPoints > MaxHealthPoints)
            {
                HealthPoints = MaxHealthPoints;
            }
        }

        public void MaxHeal()
        {
            HealthPoints = MaxHealthPoints;
        }
    }
}