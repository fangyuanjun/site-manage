using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.UI.Manage.Models
{
   public class Menu
    {
        public string menuID { get; set; }
        public int blogID { get; set; }

        [Display(Name = "父菜单")]
        public string parentID { get; set; }

        public string menuClass { get; set; }
        public string menuName { get; set; }

        [Required]
        [Display(Name = "显示名")]
        public string menuDisplay { get; set; }

        [Required]
        [Display(Name = "链接地址")]
        public string menuUrl { get; set; }
        public string menuPic { get; set; }

        [Required]
        [Display(Name = "排序权重")]
        public int menuOrder { get; set; }
        [Required]
        [Display(Name = "是否禁用")]
        public bool menuIsDisabled { get; set; }

        [Display(Name = "打开方式")]
        public string menuTarget { get; set; }
        [Required]
        [Display(Name = "添加时间")]
        public System.DateTime ADD_DATE { get; set; }
        [Required]
        [Display(Name = "修改时间")]
        public System.DateTime UPDATE_DATE { get; set; }


        public string state
        {
            get;
            set;
        }

        public string iconCls
        {
            get;
            set;
        }

        public Menu[] children
        {
            get;
            set;
        }

        public int ChildCount
        {
            get;
            set;
        }

        public String ADD_DATE2
        {
            get { return this.ADD_DATE.ToString(); }
        }

        public String UPDATE_DATE2
        {
            get { return this.UPDATE_DATE.ToString(); }
        }

        public string ID
        {
            get { return this.menuID; }
        }
    }
}
