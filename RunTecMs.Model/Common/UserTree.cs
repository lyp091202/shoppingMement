using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunTecMs.Model.Common
{
    /// <summary>
    /// 用户树结构用
    /// </summary>
    public class UserTree
    {
        public string text { get; set; }
        public int id { get; set; }
        public List<UserTree> children { get; set; }
    }
}
