using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blogs.Entity
{
   public class blog_attachment
    {
        public string fileUrl { get; set; }

        public string fileThumbUrl { get; set; }

        public string fileName { get; set; }

        public long fileSize { get; set; }
    }
}
