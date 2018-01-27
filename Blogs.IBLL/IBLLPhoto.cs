using Blogs.Entity;
using FYJ.Framework.Core.IBLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Blogs.IBLL
{
    public interface IBLLPhoto : IBLLBase<Blogs.Entity.blog_tb_Photo>
    {
        IList<blog_tb_Photo> GetPhotoBySha1(string sha1, string userID);

        int Add(blog_tb_Photo entity, blog_tb_Exif exif);

        IList<blog_tb_Photo> Query(string albumID);
    }
}
