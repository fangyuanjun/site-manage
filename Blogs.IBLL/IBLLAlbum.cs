using Blogs.Entity;
using FYJ;
using FYJ.Framework.Core;
using FYJ.Framework.Core.IBLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Blogs.IBLL
{
    public interface IBLLAlbum : IBLLBase<Blogs.Entity.blog_tb_Album>
    {
        IList<SelectModel> QueryAlbumSelect(string userID);

        IList<blog_tb_Album> QueryAlbum(ref GridPager pager);

        /// <summary>
        /// 获取封面图片
        /// </summary>
        /// <param name="albumID"></param>
        /// <returns></returns>
        List<string> GetCoverPhotos(string albumID);
    }
}
