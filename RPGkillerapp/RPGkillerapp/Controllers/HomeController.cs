using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassLibrary;
using RPGkillerapp.Models;

namespace RPGkillerapp.Controllers
{
    public class HomeController : Controller
    {
        //maak een karaker
        public ActionResult CreatePlayer()
        {
            return View();
        }

        //selecteer karakter
        public ActionResult PlayerSelect()
        {          
            return View();
        }

        //haal alle karakters op
        public List<Player> Allplayers()
        {
            return new PlayerRepo(new PlayerQuery()).GetAllPlayers();
        }

        //maakt het nieuwe karakter aan
        [HttpPost]
        public ActionResult NewPlayer(NewPlayer model)
        {
            new PlayerRepo(new PlayerQuery()).NewPlayer(model);
            return View("PlayerSelect");
        }
    }
}