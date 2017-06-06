using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
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
            Database.CloseConnection();
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
                    Player player = new Player(Convert.ToString(reader["Klasse"]), Convert.ToString(reader["Name"]), Convert.ToInt32(reader["Id"]));
                    players.Add(player);
                }
                Database.CloseConnection();
                return players;
            }
        }

        public Player GetPlayer(int id)
        {
            Player player = null;

            string query =
                "select Player.Id, Player.[Name], Player.InventoryId, Player.RoomId, SUM(Weapon.Attack + [Statistics].Attack) as Attack, SUM(Armour.Defence + [Statistics].Defence + Shield.Defence) as Defence, sum([Statistics].MaxHealth + Armour.BonusHealth) as MaxHealth, [Statistics].HealthRegen, [Statistics].Mana, [Statistics].ManaRegen, [Statistics].[Level], [Statistics].Experience, [Statistics].CritChance, sum(Shield.DodgeChance + [Statistics].DodgeChance) as dodgechance, [Statistics].Health, [Statistics].MaxMana, Klasse, Gold from Player, [Statistics] ,Armour , Weapon, Shield " +
                "where Player.Id = @playerId " +
                "and[Statistics].Id = Player.StatisticsId " +
                "and Armour.Id = EquippedArmorId " +
                "and Weapon.Id = EquippedWeaponId " +
                "and Shield.Id = EquippedShieldId " +
                "Group by Player.Id, Player.[Name], Player.InventoryId, Player.RoomId, [Statistics].HealthRegen, [Statistics].Mana, [Statistics].ManaRegen, [Statistics].[Level], [Statistics].Experience, [Statistics].CritChance, [Statistics].Health, [Statistics].MaxMana, Klasse, Gold " +
                "Having sum([Statistics].MaxHealth + Armour.BonusHealth) >= Health";
            SqlCommand cmd = new SqlCommand(query, Database.Connect());
            cmd.Parameters.AddWithValue("@playerId", id);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    player = new Player(reader.GetString(16), reader.GetInt32(11), reader.GetInt32(17), reader.GetInt32(0), reader.GetInt32(14), reader.GetInt32(8), reader.GetInt32(7), reader.GetInt32(9), reader.GetInt32(4), reader.GetInt32(5), reader.GetInt32(13), reader.GetInt32(12), reader.GetString(1), reader.GetInt32(10), reader.GetInt32(6), reader.GetInt32(15));
                }
                Database.CloseConnection();
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
                "exec playerinventory @playerid = @Playerid";

            SqlCommand cmd = new SqlCommand(query, Database.Connect());
            cmd.Parameters.AddWithValue("@Playerid", playerid);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Item item = new Item(Convert.ToInt32(reader["Id"]), Convert.ToString(reader["Name"]), Convert.ToString(reader["Type"]), Convert.ToInt32(reader["Level"]), Convert.ToInt32(reader["Gold"]), Convert.ToInt32(reader["GoldCost"]), Convert.ToInt32(reader["ItemCount"]), Convert.ToInt32(reader["Attack"]), Convert.ToInt32(reader["Defence"]), Convert.ToInt32(reader["Dodge"]), Convert.ToInt32(reader["Health"]), Convert.ToInt32(reader["CritChance"]));
                    items.Add(item);
                }
            }
            Database.CloseConnection();
            return items;
        }

        public int EquipedMagic(int playerid)
        {
            string query = "Select UsedMagic from Player " +
                           "where Player.Id = @playerid";

            int magicid = 0;
            SqlCommand cmd = new SqlCommand(query, Database.Connect());
            cmd.Parameters.AddWithValue("@playerid", playerid);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        magicid = Convert.ToInt32(reader["UsedMagic"]);
                    }
                    else
                    {
                        magicid = 0;
                    }
                }
            }
            Database.CloseConnection();
            return magicid;
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
            Database.CloseConnection();
            return equipment;
        }

        public void EquipItem(int itemid, int playerid, string type)
        {
            string query = "";
            if (type == "Consumable")
            {
                query = "update [Statistics] " +
                        "set Health += isnull((" +
                        "select HealthRestore from Consumable " +
                        "inner join item on Item.Id = Consumable.Id " +
                        "where Item.Id = @itemid), 0) " +
                        ", Mana += isnull(( " +
                        "select ManaRestore from Consumable " +
                        "inner join item on Item.Id = Consumable.Id " +
                        "where Item.Id = @itemid), 0) " +


                        "where[Statistics].Id = (select Player.StatisticsId from Player where Player.Id = @playerid) " +

                        "update ItemInventory " +
                        "set ItemCount = ItemCount - 1 " +
                        "where ItemId = " +
                        "(select item.Id from Item " +
                        "where Item.Id = @itemid)" +
                        "and ItemInventory.InventoryId = (select InventoryId from Player where Player.Id = @playerid)";

            }
            else if (type == "Armor")
            {
                query = "update Player " +
                        "set EquippedArmorId = " +
                        "(select Armour.Id from Armour " +
                        "inner join Item on Item.Id = Armour.Id " +
                        "where Item.Id = @itemid) " +
                        "where Player.Id = @playerid";
            }
            else if (type == "Weapon")
            {
                query = "update Player " +
                        "set EquippedWeaponId = " +
                        "(select Weapon.Id from Weapon " +
                        "inner join Item on Item.Id = Weapon.Id " +
                        "where Item.Id = @itemid) " +
                        "where Player.Id = @playerid";
            }
            else if (type == "Shield")
            {
                query = "update Player " +
                        "set EquippedShieldId = " +
                        "(select Shield.Id from Shield " +
                        "inner join Item on Item.Id = Shield.Id " +
                        "where Item.Id = @itemid) " +
                        "where Player.Id = @playerid";

            }
            else if (type == "Magic")
            {
                query = "update Player " +
                        "set usedmagic = @itemid " +
                        "where Player.Id = @playerid";
            }
            SqlCommand cmd = new SqlCommand(query, Database.Connect());
            cmd.Parameters.AddWithValue("@itemid", itemid);
            cmd.Parameters.AddWithValue("@playerid", playerid);
            cmd.ExecuteNonQuery();
            Database.CloseConnection();
        }

        public List<Magic> PlayerMagic(int playerid)
        {

            List<Magic> magiclist = new List<Magic>();

            string query =
                "select Magic.Id, Magic.[Name], Magic.Damage, Magic.HealthRestore, Magic.ManaCost from Magic " +
                "inner join PlayerMagic on PlayerMagic.MagicId = Magic.Id " +
                "inner join Player on Player.id = PlayerMagic.PlayerId " +
                "where Player.Id = @playerid";

            SqlCommand cmd = new SqlCommand(query, Database.Connect());
            cmd.Parameters.AddWithValue("@playerid", playerid);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Magic magic = new Magic();
                    magic.Id = Convert.ToInt32(reader["Id"]);
                    magic.Name = Convert.ToString(reader["Name"]);
                    magic.Damage = Convert.ToInt32(reader["Damage"]);
                    magic.HealthRestore = Convert.ToInt32(reader["HealthRestore"]);
                    magic.Manacost = Convert.ToInt32(reader["ManaCost"]);

                    magiclist.Add(magic);
                }
            }
            Database.CloseConnection();
            return magiclist;
        }
    }
}