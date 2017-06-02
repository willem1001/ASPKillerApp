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


        public List<Player> Players()
        {
            return new PlayerRepo(new PlayerQuery()).GetAllPlayers();
        } 

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PlayerSelect()
        {          
            return View();
        }

        public List<Player> Allplayers()
        {
            return new PlayerRepo(new PlayerQuery()).GetAllPlayers();
        }

        [HttpPost]
        public ActionResult NewPlayer(Player model)
        {
            new PlayerRepo(new PlayerQuery()).NewPlayer(model);
            return View("PlayerSelect");
        }
    }
}