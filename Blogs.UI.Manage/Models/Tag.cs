using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.UI.Manage.Models
{
  public  class Tag
    {
        public int tagID { get; set; }
        public int blogID { get; set; }
        [Required]
        [Display(Name = "所属分类")]
        public int categoryID { get; set; }

        [Display(Name = "次序")]
        public int tagOrder { get; set; }

        [Display(Name = "标识名")]
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
