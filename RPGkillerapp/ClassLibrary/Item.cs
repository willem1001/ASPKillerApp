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
    }
}
