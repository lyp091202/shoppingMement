using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunTecMs.Model.SYS
{


    public class TreeModule
    {
 
        public string id { get; set; }

        public string text { get; set; }

        public string state { get; set; }


    }

    /// <summary>
    /// 树模型
    /// </summary>
    public class TreeNode
    {

        public int id { get; set; }

        public string text { get; set; }

        public string state { get; set; }

        public bool Checked { get; set; }

        public string iconCls { get; set; }

        public List<TreeNode> children { get; set; }  //集合属性，可以保存子节点

    }

}
