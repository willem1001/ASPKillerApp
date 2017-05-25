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
            int id = int.Parse(Request.QueryString["playerid"]);

            CurrentPlayer = new PlayerRepo(new PlayerQuery()).GetPlayer(id);

            GameText.AddText(CurrentPlayer.Name);
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
            int itemid = int.Parse(Request.QueryString["ItemId"]);
            string itemtype = Request.QueryString["ItemType"];
            new PlayerRepo(new PlayerQuery()).EquipItem(itemid, CurrentPlayer.Id, itemtype);
            CurrentPlayer = new PlayerRepo(new PlayerQuery()).GetPlayer(CurrentPlayer.Id);
            return View("Inventory");

        }


        [HttpPost]
        public ActionResult ReturnToGamescreen()
        {
            return View("GameScreen");
        }


        public ActionResult Nextroom()
        {
            if (CurrentEnemy == null)
            {
                CurrenRoom = new GameRepo(new GameQuery()).Nextroom(CurrentPlayer.Level);
                GameText.AddText(CurrentPlayer.Name + " entered a " + CurrenRoom.Name);
                if (CurrenRoom.Random >= CurrenRoom.EnemyChance)
                {
                    CurrentEnemy = new GameRepo(new GameQuery()).Enemy(CurrentPlayer.Level);
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

        public ActionResult Attack()
        {
            if (CurrenRoom != null)
            {
                if (CurrentEnemy != null)
                {
                    CurrentPlayer.Health = CurrentPlayer.Health - CurrentEnemy.Attack;
                    CurrentEnemy.Health = CurrentEnemy.Health - CurrentPlayer.Attack;

                    GameText.AddText(CurrentPlayer.Name + " Attacked " + CurrentEnemy.Name + " for " +
                                     CurrentPlayer.Attack + " damage");
                    GameText.AddText(CurrentEnemy.Name + " Attacked " + CurrentPlayer.Name + " for " +
                                     CurrentEnemy.Attack + " damage");

                    new PlayerRepo(new PlayerQuery()).UpdatePlayer(CurrentPlayer);

                    if (CurrentEnemy.Health <= 0)
                    {
                        
                        GameText.AddText(CurrentPlayer.Name + " Defeated " + CurrentEnemy.Name + " And gained " + CurrentEnemy.ExperienceDrop + " experience");
                        int enemyspawn = new GameRepo(new GameQuery()).EnemyDefeated(CurrentEnemy.Id, CurrentPlayer.Id);
                        if (enemyspawn != 0 && new Random().Next(1,100) > 55)
                        {
                            CurrentEnemy = new GameRepo(new GameQuery()).EnemybyId(enemyspawn);
                            GameText.AddText("A " + CurrentEnemy.Name + "Jumps at you!");
                        }
                        else
                        {
                            CurrentEnemy = null;
                        }
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

            return View("GameScreen");
        }

    }
    
}
