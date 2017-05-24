using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RPGkillerapp.Models
{
    public static class ClassSelect
    {
        private static readonly List<string> Classes = new List<string>()
        {
            "Warrior",
            "Tank",
            "Rogue"
        };


        public static IEnumerable<SelectListItem> ClassesDropdown
        {
            get { return new SelectList(Classes); }
        }
    }
}