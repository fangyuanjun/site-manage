
using Blogs.Entity;
using Blogs.IDAL;
using System;
using System.Collections.Generic;
using System.Data;



namespace Blogs.BLL
{
    public class BLLTag : FYJ.Framework.Core.BLL.BLLAbstract<Blogs.Entity.blog_tb_tag, IDALTag>, IBLL.IBLLTag
    {

        /// <summary>
        /// 新增或修改标签
        /// </summary>
        /// <param name="blogID">博客ID</param>
        /// <param name="articleID">文章ID</param>
        /// <param name="tagDisplay">标签名</param>
        /// <returns></returns>
        public int UpdateTag(string blogID, string articleID, string tagDisplay)
        {
            return Dal.UpdateTag(blogID, articleID, tagDisplay);
        }


        public IEnumerable<blog_tb_tag> GetTags(string blogID)
        {
            return Dal.GetTags(blogID);
        }
    }
}
