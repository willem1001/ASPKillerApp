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
    }
}
