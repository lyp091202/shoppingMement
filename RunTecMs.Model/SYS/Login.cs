using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RunTecMs.Model.SYS
{
    public class Login
    {
        [Display(Name = "用户名")]
        [Required(ErrorMessage = "请输入用户名")]
        [RegularExpression(@"[A-Za-z0-9]{5,20}", ErrorMessage = "输入的用户名不正确，请重新输入。")]
        public string UserName { get; set; }

        [Display(Name = "密码")]
        [Required(ErrorMessage = "请输入密码")]
        [RegularExpression(@"[A-Za-z0-9]{5,20}", ErrorMessage = "输入的密码不正确，长度在6-20位之间，请重新输入。")]
        public string Pwd { get; set; }

        [Display(Name = "验证码")]
        [Required(ErrorMessage = "请输入验证码")]
        [RegularExpression(@"[0-9]{4}", ErrorMessage = "输入的验证码不正确，请重新输入。")]
        public string ValidCode { get; set; }
    }
}
