using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ClassLibrary;

namespace RPGkillerapp.Models
{
    public class PlayerQuery : IPlayer
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

        public Player GetPlayer(int id)
        {
            Player player = new Player();

            string query =
                "select Player.Id, Player.[Name], Player.InventoryId, Player.RoomId, SUM(Weapon.Attack + [Statistics].Attack) as Attack, SUM(Armour.Defence + [Statistics].Defence + Shield.Defence) as Defence, sum([Statistics].MaxHealth + Armour.BonusHealth) as MaxHealth, [Statistics].HealthRegen, [Statistics].Mana, [Statistics].ManaRegen, [Statistics].[Level], [Statistics].Experience, [Statistics].CritChance, sum(Shield.DodgeChance + [Statistics].DodgeChance) as dodgechance, [Statistics].Health, [Statistics].MaxMana, Klasse from Player, [Statistics] ,Armour , Weapon, Shield " +
                "where Player.Id = @playerId " +
                "and[Statistics].Id = Player.StatisticsId " +
                "and Armour.Id = EquippedArmorId " +
                "and Weapon.Id = EquippedWeaponId " +
                "and Shield.Id = EquippedShieldId " +
                "Group by Player.Id, Player.[Name], Player.InventoryId, Player.RoomId, [Statistics].HealthRegen, [Statistics].Mana, [Statistics].ManaRegen, [Statistics].[Level], [Statistics].Experience, [Statistics].CritChance, [Statistics].Health, [Statistics].MaxMana, Klasse " +
                "Having sum([Statistics].MaxHealth + Armour.BonusHealth) >= Health";
            SqlCommand cmd = new SqlCommand(query, Database.Connect());
            cmd.Parameters.AddWithValue("@playerId", id);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    player.Id = reader.GetInt32(0);
                    player.Name = reader.GetString(1);
                    // inventory 2
                    // room 3
                    player.Attack = reader.GetInt32(4);
                    player.Defence = reader.GetInt32(5);
                    player.MaxHealth = reader.GetInt32(6);
                    player.HealthRegen = reader.GetInt32(7);
                    player.Mana = reader.GetInt32(8);
                    player.ManaRegen = reader.GetInt32(9);
                    player.Level = reader.GetInt32(10);
                    player.Experience = reader.GetInt32(11);
                    player.CritChance = reader.GetInt32(12);
                    player.DodgeChance = reader.GetInt32(13);
                    player.Health = reader.GetInt32(14);
                    player.MaxMana = reader.GetInt32(15);
                    player.Classes = reader.GetString(16);
                }
                return player;
            }
        }

        public void UpdatePlayer(Player player)
        {
            string query = "update [Statistics] " +
                           "set Health = @playerhealth, Mana = @playermana " +
                           "where [Statistics].Id = (select [Statistics].Id from Player " +
                           "inner join [Statistics] on [Statistics].Id = Player.StatisticsId " +
                           "where Player.Id = @playerid)";

            SqlCommand cmd = new SqlCommand(query, Database.Connect());
            cmd.Parameters.AddWithValue("@playerid", player.Id);
            cmd.Parameters.AddWithValue("@playerhealth", player.Health);
            cmd.Parameters.AddWithValue("@playermana", player.Mana);
            cmd.ExecuteNonQuery();
            Database.CloseConnection();
        }

        public List<Item> PlayerInventory(int playerid)
        {
            List<Item> items = new List<Item>();
            string query =
                "select Item.Id, Item.Gold, Item.[Name], Item.[Level], Item.[Type], ItemInventory.ItemCount from Player, Item " +
                "inner join ItemInventory on Item.Id = ItemInventory.ItemId " +
                "where ItemInventory.InventoryId = Player.InventoryId " +
                "and Player.Id = @playerid";

            SqlCommand cmd = new SqlCommand(query, Database.Connect());
            cmd.Parameters.AddWithValue("@playerid", playerid);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Item item = new Item();
                    item.Id = Convert.ToInt32(reader["Id"]);
                    item.Name = Convert.ToString(reader["Name"]);
                    item.Type = Convert.ToString(reader["Type"]);
                    item.GoldValue = Convert.ToInt32(reader["Gold"]);
                    item.Level = Convert.ToInt32(reader["Level"]);
                    item.ItemAmount = Convert.ToInt32(reader["ItemCount"]);

                    items.Add(item);
                }
            }
            return items;
        }

        public List<int> PlayerEquipment(int playerid)
        {
            List<int> equipment = new List<int>();

            string query = "Select Item.Id from item, Player " +
                           "where Item.Id = Player.EquippedArmorId " +
                           "or Item.Id = Player.EquippedShieldId " +
                           "or Item.Id = Player.EquippedWeaponId " +
                           "group by item.Id, Player.Id " +
                           "having Player.Id = @playerid";

            SqlCommand cmd = new SqlCommand(query, Database.Connect());
            cmd.Parameters.AddWithValue("@playerid", playerid);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    equipment.Add(Convert.ToInt32(reader["Id"]));
                }
            }
            return equipment;
        }
    }
}