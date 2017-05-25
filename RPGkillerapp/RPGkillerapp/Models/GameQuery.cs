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

        public Enemy Enemy(int playerlevel)
        {
            Enemy enemy = new Enemy();

            string query =
                "select Top 1 Enemy.[Name], [Statistics].Attack, [Statistics].Defence, [Statistics].Health, [Statistics].HealthRegen, [Statistics].Mana, [Statistics].ManaRegen, [Statistics].[Level], [Statistics].CritChance, [Statistics].DodgeChance, [Statistics].MaxHealth, [Statistics].MaxMana, Enemy.ExperienceDrop, Enemy.Id from Enemy " +
                "inner join [Statistics] on Enemy.StatisticsId = [Statistics].Id " +
                "where [Statistics].[Level] = @playerlevel " +
                "Order By NEWID()";

            SqlCommand cmd = new SqlCommand(query, Database.Connect());
            cmd.Parameters.AddWithValue("@playerlevel", playerlevel);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    enemy.Name = reader.GetString(0);
                    enemy.Attack = reader.GetInt32(1);
                    enemy.Defence = reader.GetInt32(2);
                    enemy.Health = reader.GetInt32(3);
                    enemy.HealthRegen = reader.GetInt32(4);
                    enemy.Mana = reader.GetInt32(5);
                    enemy.ManaRegen = reader.GetInt32(6);
                    enemy.Level = reader.GetInt32(7);
                    enemy.CritChance = reader.GetInt32(8);
                    enemy.DodgeChance = reader.GetInt32(9);
                    enemy.MaxHealth = reader.GetInt32(10);
                    enemy.MaxMana = reader.GetInt32(11);
                    enemy.ExperienceDrop = reader.GetInt32(12);
                    enemy.Id = reader.GetInt32(13);
                }
                return enemy;
            }
        }

        public Enemy EnemybyId(int enemyId)
        {
            Enemy enemy = new Enemy();

            string query =
                "select Top 1 Enemy.[Name], [Statistics].Attack, [Statistics].Defence, [Statistics].Health, [Statistics].HealthRegen, [Statistics].Mana, [Statistics].ManaRegen, [Statistics].[Level], [Statistics].CritChance, [Statistics].DodgeChance, [Statistics].MaxHealth, [Statistics].MaxMana, Enemy.ExperienceDrop, Enemy.Id from Enemy " +
                "inner join [Statistics] on Enemy.StatisticsId = [Statistics].Id " +
                "where Enemy.Id = @EnemyId " +
                "Order By NEWID()";

            SqlCommand cmd = new SqlCommand(query, Database.Connect());
            cmd.Parameters.AddWithValue("@EnemyId", enemyId);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    enemy.Name = reader.GetString(0);
                    enemy.Attack = reader.GetInt32(1);
                    enemy.Defence = reader.GetInt32(2);
                    enemy.Health = reader.GetInt32(3);
                    enemy.HealthRegen = reader.GetInt32(4);
                    enemy.Mana = reader.GetInt32(5);
                    enemy.ManaRegen = reader.GetInt32(6);
                    enemy.Level = reader.GetInt32(7);
                    enemy.CritChance = reader.GetInt32(8);
                    enemy.DodgeChance = reader.GetInt32(9);
                    enemy.MaxHealth = reader.GetInt32(10);
                    enemy.MaxMana = reader.GetInt32(11);
                    enemy.ExperienceDrop = reader.GetInt32(12);
                    enemy.Id = reader.GetInt32(13);
                }
                return enemy;
            }
        }

        public int EnemyDefeated(int enemyid, int playerid)
        {
            string query = "exec EnemyDefeated @enemyid = @EnemyId, @playerid = @PlayerId";

            SqlCommand cmd = new SqlCommand(query, Database.Connect());
            cmd.Parameters.AddWithValue("@PlayerId", playerid);
            cmd.Parameters.AddWithValue("@EnemyId", enemyid);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    return Convert.ToInt32(reader["id"]);
                }
                return 0;


            }
        }
    }
}