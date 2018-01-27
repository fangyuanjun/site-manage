using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.UI.Manage.Models
{
    public class Category
    {
        [Required]
        public int categoryID { get; set; }

        [Display(Name = "父分类")]
        public int parentID { get; set; }
        public int blogID { get; set; }

        [Display(Name = "英文名称")]
        public string categoryName { get; set; }
        [Required]
        [Display(Name = "显示名称")]
        public string categoryDisplay { get; set; }
        [Required]
        [Display(Name = "排序权重")]
        public int categoryOrderWeight { get; set; }

        [Display(Name = "图片")]
        public string categoryPic { get; set; }

        [Display(Name = "关键字")]
        public string categoryKeywords { get; set; }

        [Display(Name = "描述")]
        public string categoryDescription { get; set; }

        [Display(Name = "域名")]
        public string categoryDomain { get; set; }
        [Required]
        [Display(Name = "是否禁用")]
        public bool categoryIsDisabled { get; set; }
        public string themID { get; set; }
        [Required]
        [Display(Name = "添加时间")]
        public System.DateTime ADD_DATE { get; set; }
        [Required]
        [Display(Name = "修改时间")]
        public System.DateTime UPDATE_DATE { get; set; }

        public int cc
        {
            get;
            set;
        }

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

        public Category[] children
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

        public int ID
        {
            get { return this.categoryID; }
        }

        /// <summary>
        /// 拥有的文章总数
        /// </summary>
        public int ArticleCount { get; set; }

        public string Url { get; set; }
    }
}
