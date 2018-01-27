using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.UI.Manage
{
   public class LinkViewModel
    {
        public string linkID { get; set; }
        public int blogID { get; set; }
        [Required]
        [Display(Name = "链接名称")]
        public string linkName { get; set; }
        [Required]
        [Display(Name = "链接地址")]
        public string linkUrl { get; set; }

        [Display(Name = "图片")]
        public string linkPic { get; set; }
        [Required]
        [Display(Name = "次序")]
        public int linkOrder { get; set; }
        [Required]
        [Display(Name = "是否禁用")]
        public bool linkIsDisabled { get; set; }
        [Required]
        [Display(Name = "添加时间")]
        public System.DateTime ADD_DATE { get; set; }
        [Required]
        [Display(Name = "修改时间")]
        public System.DateTime UPDATE_DATE { get; set; }
    }
}
