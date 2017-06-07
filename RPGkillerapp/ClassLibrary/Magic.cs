using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
   public class Magic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Damage { get; set; }
        public int HealthRestore { get; set; }
        public int Manacost { get; set; }
        //public bool Equiped { get; set; }

        public Magic(int id, string name, int damage, int healthrestore, int manacost)
        {
            this.Id = id;
            this.Name = name;
            this.Damage = damage;
            this.HealthRestore = healthrestore;
            this.Manacost = manacost;
        }
    }
}
