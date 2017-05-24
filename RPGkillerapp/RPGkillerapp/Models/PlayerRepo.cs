using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassLibrary;

namespace RPGkillerapp.Models
{
    public class PlayerRepo
    {
        public IPlayer Context;


        public PlayerRepo(IPlayer context)
        {
            Context = context;
        }

        public void NewPlayer(Player model)
        {
            Context.NewPlayer(model.Name, model.Classes);
        }

        public List<Player> GetAllPlayers()
        {
            return Context.GetAllPlayers();
        }
    }
}