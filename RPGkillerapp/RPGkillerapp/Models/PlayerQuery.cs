using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ClassLibrary;

namespace RPGkillerapp.Models
{
    public class PlayerQuery:IPlayer
    {
        public void NewPlayer(string name, string classes)
        {
            string query = "exec NewCaracter @naam = @Name, @klasse = @Classes";

            SqlCommand cmd = new SqlCommand(query, Database.Connect());
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Classes", classes);
            cmd.ExecuteNonQuery();
        }

        public List<Player> GetAllPlayers()
        {
            string query = "Select Player.[Name], Player.Id, Player.Klasse From Player";
            
            
            List<Player> players = new List<Player>();

            SqlCommand cmd = new SqlCommand(query, Database.Connect());
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Player player = new Player();
                    player.Classes = Convert.ToString(reader["Klasse"]);
                    player.Id = Convert.ToInt32(reader["Id"]);
                    player.Name = Convert.ToString(reader["Name"]);

                    players.Add(player);
                }

                return players;
            }

        }
    }
}