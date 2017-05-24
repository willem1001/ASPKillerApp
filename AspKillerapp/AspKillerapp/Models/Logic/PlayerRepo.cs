using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspKillerapp.Models.Logic
{
    public class PlayerRepo
    {
        public IPlayer Context;

        public PlayerRepo(IPlayer context)
        {
            Context = context;
        }

        public void NewPlayer(string name, string classes)
        {
            Context.NewPlayer(name, classes);
        }
    }
}