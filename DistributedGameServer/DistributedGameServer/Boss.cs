using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistributedGameServer
{
    [DataContract]
    class Boss
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

        public int Attack(out char strategy)
        {
            Random random = new Random();
            strategy = TargetStrategy;
            return random.Next(Damage / 2, Damage + 1);
        }

        public void TakeDamage(int damage)
        {
            int newHealth = HealthPoints - (Damage - Defence);
            HealthPoints = newHealth < 0 ? 0 : newHealth; 
        }
    }
}
