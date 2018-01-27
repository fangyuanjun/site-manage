using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.UI.Manage.Models
{
    public class Blog
    {
        public int blogID { get; set; }
        [Required]
        [Display(Name = "英文名称")]
        public string blogName { get; set; }
        public string userID { get; set; }
        [Required]
        [Display(Name = "标题")]
        public string blogTitle { get; set; }

         [Display(Name = "副标题")]
        public string blogSubTitle { get; set; }

        [Required]
        [Display(Name = "Logo")]
        public string blogLogo { get; set; }

        [Display(Name = "绑定域名")]
        public string blogDomain { get; set; }

        [Required]
        [Display(Name = "端口")]
        public int Port { get; set; }

        [Required]
        [Display(Name = "公告")]
        public string blogNotice { get; set; }

        [Display(Name = "关键字")]
        public string blogKeywords { get; set; }

        [Display(Name = "描述")]
        public string blogDescription { get; set; }
        [Required]
        [Display(Name = "创建时间")]
        public System.DateTime blogAddDatetime { get; set; }
        [Display(Name = "主题")]
        public string themeID { get; set; }
        [Required]
        [Display(Name = "是否禁用")]
        public bool blogIsDisabled { get; set; }
        [Required]
        [Display(Name = "次序")]
        public int blogOrder { get; set; }

        [Required]
        [Display(Name = "添加时间")]
        public System.DateTime ADD_DATE { get; set; }
        [Required]
        [Display(Name = "修改时间")]
        public System.DateTime UPDATE_DATE { get; set; }

        [Display(Name = "社交信息")]
        public string socialID { get; set; }
        [Display(Name = "统计代码")]
        public string tongji { get; set; }
        [Display(Name = "备案信息")]
        public string beian { get; set; }
        public string Url { get; set; }


        ///<summary>
        ///关于我
        ///</summary>
        [Display(Name = "关于我")]
        public String AboutMe { get; set; }

        ///<summary>
        ///QQ
        ///</summary>
        [Display(Name = "QQ")]
        public String QQ { get; set; }

        ///<summary>
        ///QQ群
        ///</summary>
        [Display(Name = "QQ群")]
        public String QQGroup { get; set; }

        ///<summary>
        ///QQ链接
        ///</summary>
       [Display(Name = "QQ链接")]
        public String QQLink { get; set; }

        ///<summary>
        ///新浪微博
        ///</summary>
        [Display(Name = "新浪微博")]
        public String Weibo { get; set; }

        ///<summary>
        ///新浪微博链接
        ///</summary>
        [Display(Name = "新浪微博链接")]
        public String WeiboLink { get; set; }

        ///<summary>
        ///邮箱
        ///</summary>
        [Display(Name = "邮箱")]
        public String Email { get; set; }

        ///<summary>
        ///电话
        ///</summary>
        [Display(Name = "电话")]
        public String Tel { get; set; }

        ///<summary>
        ///地址
        ///</summary>
        [Display(Name = "地址")]
        public String Address { get; set; }

        ///<summary>
        ///腾讯微博
        ///</summary>
        [Display(Name = "腾讯微博")]
        public String QQWeibo { get; set; }

        ///<summary>
        ///腾讯微博链接
        ///</summary>
        [Display(Name = "腾讯微博链接")]
        public String QQWeiboLink { get; set; }

        ///<summary>
        ///微信
        ///</summary>
        [Display(Name = "微信")]
        public String Weixin { get; set; }

        ///<summary>
        ///是否强制使用HTTPS
        ///</summary>
        [Display(Name = "强制HTTPS")]
        public bool IsMustSSL { get; set; }

        /// <summary>
        /// 是否关闭评论
        /// </summary>
        [Display(Name = "关闭评论")]
        public bool IsCloseComment { get; set; }

        /// <summary>
        /// 是否关闭留言板
        /// </summary>
        [Display(Name = "关闭留言")]
        public bool IsCloseBoard { get; set; }
    }
}
