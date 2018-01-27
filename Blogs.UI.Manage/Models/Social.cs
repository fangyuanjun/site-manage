using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.UI.Manage.Models
{
    public class Social
    {
        [Required]
        public string socialID { get; set; }
        [Required]
        [Display(Name = "用户ID")]
        public string userID { get; set; }
        [Required]
        [Display(Name = "显示名")]
        public string display { get; set; }
        [Display(Name = "QQ号")]
        public string QQ { get; set; }
        [Display(Name = "QQ群号")]
        public string qqQun { get; set; }
        [Display(Name = "QQ链接地址")]
        public string qqLink { get; set; }
        [Display(Name = "微博号")]
        public string weibo { get; set; }
        [Display(Name = "微博收听地址")]
        public string weiboLink { get; set; }
        [Display(Name = "邮箱")]
        public string email { get; set; }
        [Display(Name = "电话")]
        public string tel { get; set; }
        [Display(Name = "地址")]
        public string address { get; set; }
        [Display(Name = "统计代码")]
        public string tongji { get; set; }
        [Display(Name = "腾讯微博号")]
        public string qqWeibo { get; set; }
        [Display(Name = "腾讯微博收听地址")]
        public string qqWeiboLink { get; set; }
        [Display(Name = "微信号")]
        public string weixin { get; set; }

        [Required]
        [Display(Name = "添加时间")]
        public System.DateTime ADD_DATE { get; set; }
        [Required]
        [Display(Name = "修改时间")]
        public System.DateTime UPDATE_DATE { get; set; }
    }
}
