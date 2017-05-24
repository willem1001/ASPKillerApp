using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RPGkillerapp.Models
{
    public class GameText
    {
        public List<string> OldText { get; set; }

        public List<string> AddText()
        {
            List<string> text = new List<string>();
            text.Add("Hoi");
            text.Add("Doei");
            return text;
        }

        public IEnumerable<SelectListItem> GetText
        {
            get { return new SelectList(AddText()); }
        }

        

    }
}