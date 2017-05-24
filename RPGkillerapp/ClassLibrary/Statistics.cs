using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public abstract class Statistics
    {
        public int Id { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int HealthRegen { get; set; }
        public int ManaRegen { get; set; }
        public int Attack { get; set; }
        public int Defence { get; set; }
        public int DodgeChance { get; set; }
        public int CritChance { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int MaxHealth { get; set; }
        public int MaxMana { get; set; }
    }
}
