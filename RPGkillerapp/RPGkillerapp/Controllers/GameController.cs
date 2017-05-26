using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassLibrary;
using Microsoft.Ajax.Utilities;
using RPGkillerapp.Models;

namespace RPGkillerapp.Controllers
{

    public class GameController : Controller
    {
        public static GameText GameText;
        public static Player CurrentPlayer;
        public static Room CurrenRoom;
        public static Enemy CurrentEnemy;



        public ActionResult GameScreen()
        {
            GameText = new GameText();
            PlayerRepo repo = new PlayerRepo(new PlayerQuery());
            int id = int.Parse(Request.QueryString["playerid"]);
            CurrentPlayer = repo.GetPlayer(id);
            CurrentPlayer.UsedMagic = repo.EquipedMagic(CurrentPlayer.Id);
            CurrentPlayer.CurrentMagic = repo.PlayerMagic(CurrentPlayer.Id);
            GameText.AddText(CurrentPlayer.Name);
            CurrentEnemy = null;
            return View();
        }

        public List<string> Text()
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

        public ActionResult EquipItem()
        {
            PlayerRepo repo = new PlayerRepo(new PlayerQuery());
            int itemid = int.Parse(Request.QueryString["ItemId"]);
            string itemtype = Request.QueryString["ItemType"];
            repo.EquipItem(itemid, CurrentPlayer.Id, itemtype);
            CurrentPlayer = repo.GetPlayer(CurrentPlayer.Id);
            return View("Inventory");
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
                    int enemyspawn = g_repo.EnemyDefeated(CurrentEnemy.Id, CurrentPlayer.Id);
                    if (enemyspawn != 0 && new Random().Next(1, 100) > 55)
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

        [HttpPost]
        public ActionResult ReturnToGamescreen()
        {
            return View("GameScreen");
        }

        public ActionResult Magic()
        {
            CurrentPlayer.CurrentMagic = new PlayerRepo(new PlayerQuery()).PlayerMagic(CurrentPlayer.Id);
            return View();
        }

        public ActionResult Nextroom()
        {
            GameRepo repo = new GameRepo(new GameQuery());
            if (CurrentEnemy == null)
            {
                CurrenRoom = null;
                int roomlevel = new Random().Next(1, CurrentPlayer.Level);
                CurrenRoom = repo.Nextroom(roomlevel);

                if (CurrenRoom.Id == 0)
                {
                    while (CurrenRoom.Id == 0)
                    {
                        roomlevel--;
                        CurrenRoom = repo.Nextroom(roomlevel);

                    }
                }
                GameText.AddText(CurrentPlayer.Name + " entered a " + CurrenRoom.Name);
                if (CurrenRoom.Random >= CurrenRoom.EnemyChance)
                {
                    CurrentEnemy = repo.Enemy(roomlevel);
                    GameText.AddText("a " + CurrentEnemy.Name + " appeared in the " + CurrenRoom.Name);
                }
            }
            else
            {
                GameText.AddText("You cannot leave while a enemy is still alive");
            }
            return View("GameScreen");
        }

        public ActionResult Inventory()
        {
            return View();
        }

        public ActionResult UseMagic()
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
            return View("GameScreen");
        }

        public ActionResult EquipMagic()
        {
            CurrentPlayer.UsedMagic = int.Parse(Request.QueryString["MagicId"]);
            new PlayerRepo(new PlayerQuery()).EquipItem(CurrentPlayer.UsedMagic, CurrentPlayer.Id, "Magic");
            return View("Magic");
        }

        public ActionResult Attack()
        {
            if (CurrenRoom != null)
            {
                if (CurrentEnemy != null)
                {
                    CurrentPlayer.Health -= CurrentEnemy.Attack;
                    CurrentEnemy.Health -= CurrentPlayer.Attack;

                    GameText.AddText(CurrentPlayer.Name + " Attacked " + CurrentEnemy.Name + " for " +
                                     CurrentPlayer.Attack + " damage");
                    GameText.AddText(CurrentEnemy.Name + " Attacked " + CurrentPlayer.Name + " for " +
                                     CurrentEnemy.Attack + " damage");
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
            return View("GameScreen");
        }

    }

}
