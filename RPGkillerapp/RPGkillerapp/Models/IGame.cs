using ClassLibrary;

namespace RPGkillerapp.Models
{
    public interface IGame
    {
        Room NextRoom(int playerlevel);
    }
}