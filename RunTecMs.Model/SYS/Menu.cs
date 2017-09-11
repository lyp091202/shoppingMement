using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunTecMs.Model.SYS
{
   public class Menu
    {
       public int MenuID { get; set; }
       public string MenuName { get; set; }
       public string MenuUrl { get; set; }
    }

   public class JsonTree
   {
       public int id { get; set; }
       public string text { get; set; }
       public string state { get; set; }
       public Dictionary<string, string> attributes { get; set; }
       public object children { get; set; }
   }
}
