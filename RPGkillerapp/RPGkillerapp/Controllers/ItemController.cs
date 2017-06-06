using RPGkillerapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using ClassLibrary;


namespace RPGkillerapp.Controllers
{
    public class ItemController : ApiController
    {

        public List<Item> Items()
        {
            return new GameRepo(new GameQuery()).Allitems();

        }

        public IEnumerable<Item> GetAllItems()
        {
            return Items();
        }

        public IHttpActionResult GetItem(int id)
        {
            var item = Items().FirstOrDefault(p => p.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }
    }
}
