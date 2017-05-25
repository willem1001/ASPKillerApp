using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ClassLibrary;

namespace RPGkillerapp.Models
{
    public class GameQuery : IGame
    {
        public Room NextRoom(int playerlevel)
        {
            string query = "select top 1 * from Room " +
                           "where Roomlevel between @playerlevel - 1 and @playerlevel + 1 " +
                           "order by NEWID()";

            SqlCommand cmd = new SqlCommand(query, Database.Connect());
            cmd.Parameters.AddWithValue("@playerlevel", playerlevel);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                Room room = new Room();
                while (reader.Read())
                {
                    room.Name = Convert.ToString(reader["Naam"]);
                    room.Id = Convert.ToInt32(reader["Id"]);
                    room.Type = Convert.ToString(reader["Type"]);
                    room.Random = new Random().Next(1, 100);
                    room.EnemyChance = Convert.ToInt32(reader["EnemyChance"]);
                    room.EventChance = Convert.ToInt32(reader["EventChance"]);
                    room.TraderChance = Convert.ToInt32(reader["TraderChance"]);
                }
                return room;
            }
        }
    }
}