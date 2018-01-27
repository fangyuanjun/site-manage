using Blogs.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.UI.Manage.ViewModel
{
   public class ArticlePreviewViewModel
    {
        public blog_tb_article Article { get; set; }

        public string Content { get; set; }
    }
}
