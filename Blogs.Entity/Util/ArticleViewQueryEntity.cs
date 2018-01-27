using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Entity
{
    public class ArticleViewQueryEntity
    {
        public string BlogID { get; set; }

        public string SiteCategoryID { get; set; }

        public string CategoryID { get; set; }

        public string TopicID { get; set; }

        public string ArticleIsOriginal { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string ArticleTitle { get; set; }

    }
}
