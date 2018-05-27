using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistributedGameServer
{
    /// <summary>
    /// Boss
    /// stores information for a Boss
    /// </summary>
    [DataContract]
    public class Boss
    {
        [DataMember]
        public int BossID { get; set; }

        [DataMember]
        public string BossName { get; set; }

        [DataMember]
        public int MaxHealthPoints { get; set; }

        [DataMember]
        public int HealthPoints { get; set; }

        [DataMember]
        public int Defence { get; set; }

        [DataMember]
        public int Damage { get; set; }

        [DataMember]
        public char TargetStrategy { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="hp"></param>
        /// <param name="def"></param>
        /// <param name="damage"></param>
        /// <param name="strat"></param>
        public Boss(int id, string name, int hp, int def, int damage, char strat)
        {
            Random rnd = new Random();
            this.BossID = id;
            this.BossName = name;
            this.MaxHealthPoints = hp;
            this.HealthPoints = hp;
            this.Damage = damage;
            this.TargetStrategy = strat;
        }

        /// <summary>
        /// Attack 
        /// property returns random damage value
        /// </summary>
        public int Attack
        {
            get
            {
                Random random = new Random();
                return random.Next(Damage / 2, Damage + 1);
            }
        }

        /// <summary>
        /// TakeDamage
        /// reduces Boss health by given amount
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(int damage)
        {
            if (Defence < damage)
                HealthPoints -= (damage - Defence);
            if (HealthPoints < 0)
                HealthPoints = 0;
        }
    }
}
