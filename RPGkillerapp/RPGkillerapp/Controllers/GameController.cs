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
        public static Current Current;

        public ActionResult GameScreen()
        {
            int id = int.Parse(Request.QueryString["playerid"]);
            Current = new Current();
            Current.SetPlayer(id);
            return View();
        }

        public ActionResult EquipItem()
        {
            int itemid = int.Parse(Request.QueryString["ItemId"]);
            string itemtype = Request.QueryString["ItemType"];
            Current.EquipItem(itemid, itemtype);
            return View("Inventory");
        }

        [HttpPost]
        public ActionResult ReturnToGamescreen()
        {
            return View("GameScreen");
        }

        public ActionResult Magic()
        {
            Current.EquipMagic();
            return View();
        }

        public ActionResult Nextroom()
        {
            Current.Nextroom();
            return View("GameScreen");
        }

        public ActionResult Inventory()
        {
            return View();
        }

        public ActionResult UseMagic()
        {
            Current.UseMagic();
            return View("GameScreen");
        }

        public ActionResult EquipMagic()
        {
            int usedMagic = int.Parse(Request.QueryString["MagicId"]);
            Current.EquipMagic(usedMagic);
            return View("Magic");
        }

        public ActionResult Attack()
        {
            Current.Attack();
            return View("GameScreen");
        }

        public ActionResult Trader()
        {
            return View();
        }

        public ActionResult Sellitem()
        {
            int itemid = int.Parse(Request.QueryString["ItemId"]);
            Current.Sellitem(itemid);
            return View("Trader");
        }

        public ActionResult Buyitem()
        {
            int itemid = int.Parse(Request.QueryString["ItemId"]);
            int itemcost = int.Parse(Request.QueryString["ItemCost"]);
            Current.Buyitem(itemid, itemcost);
            return View("Trader");
        }
    }
}
