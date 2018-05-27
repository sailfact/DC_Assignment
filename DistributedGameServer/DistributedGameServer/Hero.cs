using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistributedGameServer
{
    /// <summary>
    /// Hero
    /// strores all to information for heros
    /// </summary>
    [DataContract]
    public class Hero
    {
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
        /// Constructor
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
        
        /// <summary>
        /// TakeDamage
        /// damages hero for given value 
        /// if it's less that 0 set health to 0
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(int damage)
        {
            if (Defence < damage)
                HealthPoints -= (damage - Defence);

            if (HealthPoints < 0)
                HealthPoints = 0;
        }

        /// <summary>
        /// Heal
        /// heals hero for the given value
        /// if its larger than the max set it to the max
        /// </summary>
        /// <param name="health"></param>
        public void Heal(int health)
        {
            HealthPoints += health;
            if (HealthPoints > MaxHealthPoints)
            {
                HealthPoints = MaxHealthPoints;
            }
        }

        /// <summary>
        /// MaxHeal
        /// heals hero to max health
        /// </summary>
        public void MaxHeal()
        {
            HealthPoints = MaxHealthPoints;
        }
    }
}