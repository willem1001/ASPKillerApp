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


        [HttpPost]
        public ActionResult Nextroom()
        {
            if (CurrenRoom.Enemy == null)
            {
                CurrenRoom = new GameRepo(new GameQuery()).Nextroom(CurrentPlayer.Level);
                GameText.AddText(CurrentPlayer.Name + " entered a " + CurrenRoom.Name);
                if (CurrenRoom.Random >= CurrenRoom.EnemyChance)
                {
                    CurrenRoom.Enemy = new GameRepo(new GameQuery()).Enemy(CurrentPlayer.Level);
                    GameText.AddText("a " + CurrenRoom.Enemy.Name + " appeared in the " + CurrenRoom.Name);
                }
            }
            else
            {
                GameText.AddText("You cannot leave while a enemy is still alive");
            }
            return View("GameScreen");
        }

        public ActionResult Attack()
        {
            if (CurrenRoom != null)
            {
                if (CurrenRoom.Enemy != null)
                {
                    CurrentPlayer.Health = CurrentPlayer.Health - CurrenRoom.Enemy.Attack;
                    CurrenRoom.Enemy.Health = CurrenRoom.Enemy.Health - CurrentPlayer.Attack;

                    GameText.AddText(CurrentPlayer.Name + " Attacked " + CurrenRoom.Enemy.Name + " for " +
                                     CurrentPlayer.Attack + " damage");
                    GameText.AddText(CurrenRoom.Enemy.Name + " Attacked " + CurrentPlayer.Name + " for " +
                                     CurrenRoom.Enemy.Attack + " damage");

                    new PlayerRepo(new PlayerQuery()).UpdatePlayer(CurrentPlayer);

                    if (CurrenRoom.Enemy.Health <= 0)
                    {
                        
                        GameText.AddText(CurrentPlayer.Name + " Defeated " + CurrenRoom.Enemy.Name + " And gained " + CurrenRoom.Enemy.ExperienceDrop + " experience");
                        int enemyspawn = new GameRepo(new GameQuery()).EnemyDefeated(CurrenRoom.Enemy.Id, CurrentPlayer.Id);
                        if (enemyspawn != 0 && new Random().Next(1,100) > 55)
                        {
                            CurrenRoom.Enemy = new GameRepo(new GameQuery()).EnemybyId(enemyspawn);
                            GameText.AddText("A " + CurrenRoom.Enemy.Name + "Jumps at you!");
                        }
                        else
                        {
                            CurrenRoom.Enemy = null;
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
