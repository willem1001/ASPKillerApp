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
                           "where Roomlevel = @playerlevel " +
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
                Database.CloseConnection();
                return room;
            }
        }

        public Enemy Enemy(int playerlevel)
        {

            Enemy enemy = null;

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
                    enemy = new Enemy(0, reader.GetInt32(12), reader.GetInt32(13), reader.GetInt32(3), reader.GetInt32(5), reader.GetInt32(4), reader.GetInt32(6), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(9), reader.GetInt32(8), reader.GetString(0), reader.GetInt32(7), reader.GetInt32(10), reader.GetInt32(11));
                }
                Database.CloseConnection();
                return enemy;
            }
        }

        public Enemy EnemybyId(int enemyId)
        {
            Enemy enemy = null;

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
                    enemy = new Enemy(0, reader.GetInt32(12), reader.GetInt32(13), reader.GetInt32(3), reader.GetInt32(5), reader.GetInt32(4), reader.GetInt32(6), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(9), reader.GetInt32(8), reader.GetString(0), reader.GetInt32(7), reader.GetInt32(10), reader.GetInt32(11));
                }
                Database.CloseConnection();
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
                Database.CloseConnection();
                return 0;
            }
        }

        public Item Getitem(int playerlevel)
        {
            string query = "exec GetItem @level = @playerlevel";
            int currentplayerlevel = playerlevel;

            Item item = null;
            SqlCommand cmd = new SqlCommand(query, Database.Connect());
            cmd.Parameters.AddWithValue("@playerlevel", currentplayerlevel);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    item = new Item(Convert.ToInt32(reader["id"]), Convert.ToString(reader["name"]));
                }
            }
            Database.CloseConnection();
            while (item == null)
            {
                currentplayerlevel--;
                query = "exec GetItem @level = @newplayerlevel";
                cmd = new SqlCommand(query, Database.Connect());
                cmd.Parameters.AddWithValue("@newplayerlevel", currentplayerlevel);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        item = new Item(Convert.ToInt32(reader["id"]), Convert.ToString(reader["name"]));
                    }
                    Database.CloseConnection();
                }
            }
            Database.CloseConnection();
            return item;
        }

        public void Setitem(List<int> itemId, int playerid)
        {

            string query = "exec FoundItem @playerid = @Playerid, @itemid = @Itemid";
            foreach (var currentitem in itemId)
            {
                SqlCommand cmd = new SqlCommand(query, Database.Connect());
                cmd.Parameters.AddWithValue("@Playerid", playerid);
                cmd.Parameters.AddWithValue("@Itemid", currentitem);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Item> TraderItems(int traderid, bool update)
        {
            if (update)
            {
                string query = "exec RefreshTrader @traderid= @TraderId";
                List<Item> traderItems = new List<Item>();
                SqlCommand cmd = new SqlCommand(query, Database.Connect());
                cmd.Parameters.AddWithValue("@TraderId", traderid);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Item item = new Item(Convert.ToInt32(reader["Id"]), Convert.ToString(reader["Name"]), Convert.ToString(reader["Type"]), Convert.ToInt32(reader["Level"]), Convert.ToInt32(reader["Gold"]), Convert.ToInt32(reader["GoldCost"]),  Convert.ToInt32(reader["ItemCount"]), Convert.ToInt32(reader["Attack"]), Convert.ToInt32(reader["Defence"]), Convert.ToInt32(reader["Dodge"]), Convert.ToInt32(reader["Health"]), Convert.ToInt32(reader["CritChance"]));
                        traderItems.Add(item);
                    }
                }
                return traderItems;

            }
            else
            {
                string query = "exec TraderItems @traderid = @TraderId";
                List<Item> traderItems = new List<Item>();
                SqlCommand cmd = new SqlCommand(query, Database.Connect());
                cmd.Parameters.AddWithValue("@TraderId", traderid);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Item item = new Item(Convert.ToInt32(reader["Id"]), Convert.ToString(reader["Name"]), Convert.ToString(reader["Type"]), Convert.ToInt32(reader["Level"]), Convert.ToInt32(reader["Gold"]), Convert.ToInt32(reader["GoldCost"]), Convert.ToInt32(reader["ItemCount"]), Convert.ToInt32(reader["Attack"]), Convert.ToInt32(reader["Defence"]), Convert.ToInt32(reader["Dodge"]), Convert.ToInt32(reader["Health"]), Convert.ToInt32(reader["CritChance"]));
                        traderItems.Add(item);
                    }
                }
                return traderItems;
            }
        }

        public void Sellitem(int traderid, int playerid, int itemid)
        {
            string query = "Exec Sellitem @traderid = @TraderId, @playerid = @PlayerId, @itemid = @ItemId";
            SqlCommand cmd = new SqlCommand(query, Database.Connect());
            cmd.Parameters.AddWithValue("@TraderId", traderid);
            cmd.Parameters.AddWithValue("@PlayerId", playerid);
            cmd.Parameters.AddWithValue("@ItemId", itemid);
            cmd.ExecuteNonQuery();
        }

        public void Buyitem(int traderid, int playerid, int itemid)
        {
            string query = "Exec Buyitem @traderid = @TraderId, @playerid = @PlayerId, @itemid = @ItemId";
            SqlCommand cmd = new SqlCommand(query, Database.Connect());
            cmd.Parameters.AddWithValue("@TraderId", traderid);
            cmd.Parameters.AddWithValue("@PlayerId", playerid);
            cmd.Parameters.AddWithValue("@ItemId", itemid);
            cmd.ExecuteNonQuery();
        }

    }
}