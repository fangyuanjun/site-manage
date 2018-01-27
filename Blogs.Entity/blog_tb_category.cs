using FYJ;
using System;
using System.Collections.Generic;

namespace Blogs.Entity
{
    public class blog_tb_category : FYJ.Entity.EntityBase
    {
        public int categoryID { get; set; }
        public int parentID { get; set; }
        public int blogID { get; set; }
        public string categoryName { get; set; }
        public string categoryDisplay { get; set; }
        public int categoryOrderWeight { get; set; }
        public string categoryPic { get; set; }
        public string categoryKeywords { get; set; }
        public string categoryDescription { get; set; }
        public string categoryDomain { get; set; }
        public bool categoryIsDisabled { get; set; }
        public bool categoryIsSystem { get; set; }
        public string themeID { get; set; }
        public System.DateTime ADD_DATE { get; set; }
        public System.DateTime UPDATE_DATE { get; set; }

        [Ignore]
        public int rowcount { get; set; }

        [Ignore]
        public blog_tb_category[] children
        {
            get;
            set;
        }


        [Ignore]
        public string TextWithTreeSpace { get; set; }

        /// <summary>
        /// 层次深度  0开始
        /// </summary>
        [Ignore]
        public int treeDeep { get; set; }

        [Ignore]
        public string id { get { return categoryID + ""; } }

        /// <summary>
        /// 文章数
        /// </summary>
        [Ignore]
        public int ArticleCount { get; set; }
    }
}
