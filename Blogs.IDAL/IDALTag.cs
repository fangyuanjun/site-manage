
using System;
using System.Collections.Generic;
using System.Data;
using Blogs.Entity;


namespace Blogs.IDAL
{
    public interface IDALTag : FYJ.Framework.Core.IDAL.IDALBase<Blogs.Entity.blog_tb_tag>
    {
       /// <summary>
       /// 新增或修改标签
       /// </summary>
       /// <param name="blogID">博客ID</param>
       /// <param name="articleID">文章ID</param>
       /// <param name="tagDisplay">标签名</param>
       /// <returns></returns>
       int UpdateTag(string blogID,string articleID,string tagDisplay);

        /// <summary>
        /// 获取博客标签
        /// </summary>
        /// <param name="blogID"></param>
        /// <returns></returns>
        /// fangyj 2015-5-8
       IEnumerable<blog_tb_tag> GetTags(string blogID);
    }
}
