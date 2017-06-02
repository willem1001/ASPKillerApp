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
    }
}