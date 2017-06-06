using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
   public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Level { get; set; }
        public int GoldValue { get; set; }
        public int GoldCost { get; set; }
        public int ItemAmount { get; set; }
        public int Attack { get; set; }
        public int Defence { get; set; }
        public int Dodge { get; set; }
        public int Bonushealth { get; set; }
        public int CritChance { get; set; }

        public Item(int id, string name, string type, int level, int goldvalue, int goldcost, int itemamount, int attack, int defence, int dodge, int bonushealth, int crit)
        {
            this.Id = id;
            this.Name = name;
            this.Type = type;
            this.Level = level;
            this.GoldValue = goldvalue;
            this.GoldCost = goldcost;
            this.ItemAmount = itemamount;
            this.Attack = attack;
            this.Defence = defence;
            this.Dodge = dodge;
            this.Bonushealth = bonushealth;
            this.CritChance = crit;
        }

        public Item(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
