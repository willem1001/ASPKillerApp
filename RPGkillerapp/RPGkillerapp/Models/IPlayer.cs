using System.Collections.Generic;
using ClassLibrary;

namespace RPGkillerapp.Models
{
    public interface IPlayer
    {
       void NewPlayer(string name, string classes);
        List<Player> GetAllPlayers();
    }
}