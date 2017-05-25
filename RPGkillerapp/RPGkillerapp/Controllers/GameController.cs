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
        

        
        public ActionResult GameScreen()
        {
            GameText = new GameText();
            string name = Request.QueryString["name"];
            int id = Convert.ToInt32(Request.QueryString["Id"]);

            CurrentPlayer = new PlayerRepo(new PlayerQuery()).GetPlayer(id);

            GameText.AddText(name);
            return View();
        }

        public List<string> Text()
        {
            return GameText.OldText;
        }


        [HttpPost]
        public ActionResult Nextroom()
        {

         Room room = new GameRepo(new GameQuery()).Nextroom(CurrentPlayer.Id);
            GameText.AddText(room.Name);
            return View("GameScreen");
        }

    }
}