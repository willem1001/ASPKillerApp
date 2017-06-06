using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Enemy : Statistics
    {
        public int AbilityChance { get; set; }
        public int ExperienceDrop { get; set; }

        public Enemy(int abilitiychance, int experiencedrop, int id, int health, int mana, int healthregen, int manaregen, int attack, int defence, int dodgechance, int critchance, string name, int level, int maxhealth, int maxmana)
        {
            this.AbilityChance = abilitiychance;
            this.ExperienceDrop = experiencedrop;
            this.Id = id;
            this.Health = health;
            this.Mana = mana;
            this.HealthRegen = healthregen;
            this.ManaRegen = manaregen;
            this.Attack = attack;
            this.Defence = defence;
            this.DodgeChance = dodgechance;
            this.CritChance = critchance;
            this.Name = name;
            this.Level = level;
            this.MaxHealth = maxhealth;
            this.MaxMana = maxmana;
        }

    }
}
