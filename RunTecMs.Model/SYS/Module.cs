using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RunTecMs.Model.SYS
{
    [Serializable]
   public class Module
    {
       [Display(Name="模块ID")]
       public int ModuleID { get; set; }

       [Display(Name="父级模块ID")]
       public int ParentModuleID { get; set; }

       [Display(Name="模块名称")]
       public string Name { get; set; }

       [Display(Name="模块值")]
       public string Value { get; set; }

       [Display(Name = "后台区分")]
       public int ManagerTypeID { get; set; }

       [Display(Name = "排序")]
       public int OrderValue { get; set; }

       [Display(Name = "路径")]
       public string Path { get; set; }

       [Display(Name = "描述")]
       public string Description { get; set; }

       [Display(Name = "是否启用")]
       public bool Enabled { get; set; }

       [Display(Name = "可用按钮")]
       public string AvailableOpIDs { get; set; }

       [Display(Name = "超管标识")]
       public bool IsSYSDBA { get; set; }
    }


    public class ModuleInfo : Module
    {
        /// <summary>
        /// 父级名称
        /// </summary>
        public string ParentName { get; set; }
    }
    
}
