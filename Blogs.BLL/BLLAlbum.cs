using Blogs.Entity;
using Blogs.IDAL;
using FYJ;
using FYJ.Framework.Core;
using FYJ.Framework.Core.BLL;
using FYJ.Framework.Core.IBLL;
using FYJ.Framework.Core.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blogs.BLL
{
    public class BLLAlbum : BLLAbstract<blog_tb_Album, IDALAlbum>, IBLL.IBLLAlbum
    {
        public IList<SelectModel> QueryAlbumSelect(string userID)
        {
            return Dal.QueryAlbumSelect(userID);
        }

        public IList<blog_tb_Album> QueryAlbum(ref GridPager pager)
        {
            return Dal.QueryAlbum(ref pager);
        }


        public List<string> GetCoverPhotos(string albumID)
        {
            return Dal.GetCoverPhotos(albumID);
        }

       
    }
}
