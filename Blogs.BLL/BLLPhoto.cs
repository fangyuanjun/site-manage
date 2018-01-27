using Blogs.Entity;
using Blogs.IDAL;
using FYJ.Framework.Core.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blogs.BLL
{
    public class BLLPhoto : FYJ.Framework.Core.BLL.BLLAbstract<Blogs.Entity.blog_tb_Photo, IDALPhoto>, IBLL.IBLLPhoto
    {

        public IList<blog_tb_Photo> GetPhotoBySha1(string sha1, string userID)
        {
            return Dal.GetPhotoBySha1(sha1, userID);
        }


        public int Add(blog_tb_Photo entity, blog_tb_Exif exif)
        {
            return Dal.Add(entity, exif);
        }


        public IList<blog_tb_Photo> Query(string albumID)
        {
            return Dal.Query(albumID);
        }
    }
}
