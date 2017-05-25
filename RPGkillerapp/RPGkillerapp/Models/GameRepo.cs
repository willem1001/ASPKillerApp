using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassLibrary;

namespace RPGkillerapp.Models
{
    public class GameRepo
    {
        public IGame Context;

        public GameRepo(IGame context)
        {
            Context = context;
        }
        public Room Nextroom(int playerlevel)
        {
            return Context.NextRoom(playerlevel);
        }
    }
}