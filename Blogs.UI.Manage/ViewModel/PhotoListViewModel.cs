using Blogs.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blogs.UI.Manage
{
    public class PhotoListViewModel
    {
        public string Title { get; set; }

        public IList<blog_tb_Photo> PhotoCollection { get; set; }
    }
}