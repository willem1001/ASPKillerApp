using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Player : Statistics
    {
        public string Classes { get; set; }
        public int Experience { get; set; }
        public int UsedMagic { get; set; }
        public List<Magic> CurrentMagic { get; set; }
        public int Gold { get; set; }

        public Player(string classes, int experience, int gold, int id, int health, int mana, int healthregen, int manaregen, int attack, int defence, int dodgechance, int critchance, string name, int level, int maxhealth, int maxmana)
        {
            this.Classes = classes;
            this.Experience = experience;
            this.Gold = gold;
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

        public Player(string classes, string name, int id)
        {
            this.Classes = classes;
            this.Name = name;
            this.Id = id;
        }
    }
}
