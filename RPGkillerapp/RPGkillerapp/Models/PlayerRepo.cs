﻿using System;
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

        public Player GetPlayer(int id)
        {
            return Context.GetPlayer(id);
        }

        public void UpdatePlayer(Player player)
        {
            Context.UpdatePlayer(player);
        }

        public List<Item> PlayerInventory(int playerid)
        {
           return Context.PlayerInventory(playerid);
        }

        public List<int> PlayerEquipment(int playerid)
        {
            return Context.PlayerEquipment(playerid);
        }

    }
}