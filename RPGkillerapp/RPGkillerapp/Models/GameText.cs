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

        public GameText()
        {
            if (OldText == null)
            {
                OldText = new List<string>();
                OldText.Add("Hoi");
            }
        }

        public void clear()
        {
            OldText.Clear();
        }


        public void AddText(string add)
        {
            List<string> text = new List<string>();
            text = OldText;
            text.Add(add);
            OldText = text;
            
        }


    }
}