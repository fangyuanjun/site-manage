using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.UI.Manage
{
  public  class TagViewModel
    {
        public int tagID { get; set; }
        public int blogID { get; set; }
        [Required]
        [Display(Name = "所属分类")]
        public int categoryID { get; set; }
        public int tagOrder { get; set; }
        public string tagName { get; set; }

        [Required]
        [Display(Name = "标签名称")]
        public string tagDisplay { get; set; }
        [Required]
        [Display(Name = "添加时间")]
        public System.DateTime ADD_DATE { get; set; }
        [Required]
        [Display(Name = "修改时间")]
        public System.DateTime UPDATE_DATE { get; set; }

        /// <summary>
        /// 拥有的文章总数
        /// </summary>
        public int ArticleCount { get; set; }

        public string Url { get; set; }
    }
}
