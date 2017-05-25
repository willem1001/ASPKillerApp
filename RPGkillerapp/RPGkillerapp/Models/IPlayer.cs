using System.Collections.Generic;
using ClassLibrary;

namespace RPGkillerapp.Models
{
    public interface IPlayer
    {
       void NewPlayer(string name, string classes);
        List<Player> GetAllPlayers();
        Player GetPlayer(int id);
        void UpdatePlayer(Player player);
        List<Item> PlayerInventory(int playerid);
        List<int> PlayerEquipment(int playerid);
        void EquipItem(int itemid, int playerid, string type);
    }
}