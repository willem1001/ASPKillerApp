using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassLibrary;

namespace RPGkillerapp.Models
{
    public class Current
    {
        public Player CurrentPlayer { get; set; }
        public Room CurrentRoom { get; set; }
        public Enemy CurrentEnemy { get; set; }
        public GameText GameText { get; set; }
        public bool Trader { get; set; }
        public bool Gold { get; set; } = true;

        public void SetPlayer(int id)
        {
            GameText = new GameText();
            PlayerRepo repo = new PlayerRepo(new PlayerQuery());

            CurrentPlayer = repo.GetPlayer(id);
            CurrentPlayer.UsedMagic = repo.EquipedMagic(CurrentPlayer.Id);
            CurrentPlayer.CurrentMagic = repo.PlayerMagic(CurrentPlayer.Id);
            GameText.AddText(CurrentPlayer.Name);
            CurrentEnemy = null;
        }

        public List<string> StoryText()
        {
            return GameText.OldText;
        }

        public List<Item> InventoryItems()
        {
            return new PlayerRepo(new PlayerQuery()).PlayerInventory(CurrentPlayer.Id);
        }

        public List<int> PlayerEquipment()
        {
            return new PlayerRepo(new PlayerQuery()).PlayerEquipment(CurrentPlayer.Id);
        }

        public void EquipMagic()
        {
            CurrentPlayer.CurrentMagic = new PlayerRepo(new PlayerQuery()).PlayerMagic(CurrentPlayer.Id);
        }

        public void EquipMagic(int magicid)
        {
            new PlayerRepo(new PlayerQuery()).EquipItem(magicid, CurrentPlayer.Id, "Magic");
        }

        public List<Item> TraderItems()
        {
            return new GameRepo(new GameQuery()).TraderItems(1, Trader);
        }

        public void Sellitem(int itemid)
        {
            new GameRepo(new GameQuery()).Sellitem(1, CurrentPlayer.Id, itemid);
            SetPlayer(CurrentPlayer.Id);
            Gold = true;
        }

        public void Buyitem(int itemid, int itemcost)
        {
            if (CurrentPlayer.Gold >= itemcost)
            {
                new GameRepo(new GameQuery()).Buyitem(1, CurrentPlayer.Id, itemid);
                Gold = true;
            }
            else
            {
                Gold = false;
            }
            SetPlayer(CurrentPlayer.Id);
        }

        public void EquipItem(int itemid, string itemtype)
        {
            PlayerRepo repo = new PlayerRepo(new PlayerQuery());
            repo.EquipItem(itemid, CurrentPlayer.Id, itemtype);
            CurrentPlayer = repo.GetPlayer(CurrentPlayer.Id);
        }

        public void Endturn()
        {
            PlayerRepo repo = new PlayerRepo(new PlayerQuery());
            GameRepo g_repo = new GameRepo(new GameQuery());
            CurrentPlayer.Mana += CurrentPlayer.ManaRegen;
            CurrentPlayer.Health += CurrentPlayer.HealthRegen;
            repo.UpdatePlayer(CurrentPlayer);
            CurrentPlayer = repo.GetPlayer(CurrentPlayer.Id);
            CurrentPlayer.UsedMagic = repo.EquipedMagic(CurrentPlayer.Id);
            CurrentPlayer.CurrentMagic = repo.PlayerMagic(CurrentPlayer.Id);
            if (CurrentEnemy != null)
            {
                if (CurrentEnemy.Health <= 0)
                {
                    GameText.AddText(CurrentPlayer.Name + " Defeated " + CurrentEnemy.Name + " And gained " +
                                     CurrentEnemy.ExperienceDrop + " experience");

                    List<int> items = new List<int>();
                    Item item = g_repo.GetItem(CurrentPlayer.Level);

                    GameText.AddText(CurrentEnemy.Name + " Dropped a " + item.Name);

                    items.Add(item.Id);
                    items.Add(200);
                    g_repo.SetItem(items, CurrentPlayer.Id);

                    int enemyspawn = g_repo.EnemyDefeated(CurrentEnemy.Id, CurrentPlayer.Id);
                    if (enemyspawn != 0 && new Random().Next(1, 100) > 80)
                    {
                        CurrentEnemy = g_repo.EnemybyId(enemyspawn);
                        GameText.AddText("A " + CurrentEnemy.Name + "Jumps at you!");
                    }
                    else
                    {
                        CurrentEnemy = null;
                    }
                }
            }
        }

        public void Nextroom()
        {
            Trader = false;
            GameRepo repo = new GameRepo(new GameQuery());
            if (CurrentEnemy == null)
            {
                CurrentRoom = null;
                int roomlevel = new Random().Next(1, CurrentPlayer.Level);
                CurrentRoom = repo.Nextroom(roomlevel);

                if (CurrentRoom.Id == 0)
                {
                    while (CurrentRoom.Id == 0)
                    {
                        roomlevel--;
                        CurrentRoom = repo.Nextroom(roomlevel);

                    }
                }
                GameText.AddText(CurrentPlayer.Name + " entered a " + CurrentRoom.Name);
                if (CurrentRoom.Random <= CurrentRoom.EnemyChance && CurrentRoom.Random >= CurrentRoom.TraderChance &&
                    CurrentRoom.Random >= CurrentRoom.EventChance)
                {
                    CurrentEnemy = repo.Enemy(roomlevel);
                    GameText.AddText("a " + CurrentEnemy.Name + " appeared in the " + CurrentRoom.Name);
                }
                if (CurrentRoom.Random >= CurrentRoom.EnemyChance && CurrentRoom.Random <= CurrentRoom.TraderChance &&
                    CurrentRoom.Random >= CurrentRoom.EventChance)
                {
                    GameText.AddText("You meet a trader while traveling");
                    Trader = true;
                }
            }
            else
            {
                GameText.AddText("You cannot leave while a enemy is still alive");
            }
        }


        public void UseMagic()
        {
            if (CurrentPlayer.UsedMagic != 0)
            {
                foreach (Magic magic in CurrentPlayer.CurrentMagic)
                {
                    if (magic.Id == CurrentPlayer.UsedMagic)
                    {
                        if (CurrentPlayer.Mana > magic.Manacost)
                        {
                            if (magic.Damage == 0)
                            {
                                CurrentPlayer.Health += magic.HealthRestore;
                                CurrentPlayer.Mana -= magic.Manacost;
                                GameText.AddText(CurrentPlayer.Name + " healed for " + magic.HealthRestore + "health");
                            }
                            else if (CurrentEnemy != null)
                            {
                                CurrentPlayer.Health += magic.HealthRestore - CurrentEnemy.Attack;
                                CurrentPlayer.Mana -= magic.Manacost;
                                CurrentEnemy.Health -= magic.Damage;
                                if (magic.Damage > 0)
                                {
                                    GameText.AddText(CurrentPlayer.Name + " did " + magic.Damage + " to " +
                                                     CurrentEnemy.Name + "with " + magic.Name);
                                }
                                if (magic.HealthRestore > 0)
                                {
                                    GameText.AddText(CurrentPlayer.Name + " healed for " + magic.HealthRestore +
                                                     "health");
                                }

                                GameText.AddText(CurrentEnemy.Name + " Attacked " + CurrentPlayer.Name + " for " +
                                                 CurrentEnemy.Attack + " damage");
                            }
                            else
                            {
                                GameText.AddText("There is nothing to use magic on");
                            }
                        }
                        else
                        {
                            GameText.AddText("Not enough mana to use this spell");
                        }
                    }
                }
            }
            else
            {
                GameText.AddText("No magic equipped yet");
            }
            Endturn();
        }

        public void Attack()
        {
            if (CurrentRoom != null)
            {
                if (CurrentEnemy != null)
                {
                    Random random = new Random();
                    int enemyattack =
                        Convert.ToInt32(Math.Round(Convert.ToDouble(CurrentEnemy.Attack - (CurrentPlayer.Defence / 2)),
                            0));
                    if (enemyattack < 0)
                    {
                        enemyattack = 0;
                    }

                    CurrentPlayer.Health -= enemyattack;
                    int rand = random.Next(1, 101);
                    int playerattack = CurrentPlayer.Attack;
                    if (CurrentPlayer.CritChance > rand)
                    {
                        playerattack = (playerattack * 2);
                        GameText.AddText("Critical hit!");
                        GameText.AddText(CurrentPlayer.Name + " Attacked " + CurrentEnemy.Name + " for " + playerattack +
                                         " damage");
                    }
                    else
                    {
                        GameText.AddText(CurrentPlayer.Name + " Attacked " + CurrentEnemy.Name + " for " + playerattack +
                                         " damage");
                    }
                    rand = random.Next(1, 101);
                    if (CurrentPlayer.DodgeChance > rand)
                    {
                        GameText.AddText(CurrentEnemy.Name + " Attacked " + CurrentPlayer.Name + " but missed");
                    }
                    else
                    {
                        CurrentEnemy.Health -= CurrentPlayer.Attack;
                        GameText.AddText(CurrentEnemy.Name + " Attacked " + CurrentPlayer.Name + " for " + enemyattack +
                                         " damage");
                    }
                }
                else
                {
                    GameText.AddText("There is nothing to attack here");
                }
            }
            else
            {
                GameText.AddText("There is nothing to attack here");
            }
            Endturn();
        }

    }
}

