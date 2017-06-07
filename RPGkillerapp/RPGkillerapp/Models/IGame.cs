using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using ClassLibrary;

namespace RPGkillerapp.Models
{
    public interface IGame
    {
        Room NextRoom(int playerlevel);
        Enemy Enemy(int playerlevel);
        int EnemyDefeated(int enemyid, int playerid);
        Enemy EnemybyId(int enemyId);
        Item Getitem(int playerlevel);
        void Setitem(List<int> itemid, int playerid);
        List<Item> TraderItems(int traderid, bool update);
        void Sellitem(int traderid, int playerid, int itemid);
        void Buyitem(int traderid, int playerid, int itemid);
        List<Item> Allitems();
        int ItemAmount(int playerid, int itemid);
    }
}