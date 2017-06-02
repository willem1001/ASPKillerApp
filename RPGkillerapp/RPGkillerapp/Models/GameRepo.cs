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

        public Enemy Enemy(int playerlevel)
        {
            return Context.Enemy(playerlevel);
        }

        public int EnemyDefeated(int enemyid, int playerid)
        {
           return Context.EnemyDefeated(enemyid, playerid);
        }

        public Enemy EnemybyId(int enemyId)
        {
            return Context.EnemybyId(enemyId);
        }

        public Item GetItem(int playerlevel)
        {
            return Context.Getitem(playerlevel);
        }

        public void SetItem(List<int> itemid, int playerid)
        {
            Context.Setitem(itemid, playerid);
        }
    }
}