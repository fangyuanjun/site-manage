
using Blogs.Entity;
using FYJ.Framework.Core.IBLL;
using System;
using System.Collections.Generic;
using System.Data;



namespace Blogs.IBLL
{
    public interface IBLLTag : IBLLBase<Blogs.Entity.blog_tb_tag>
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
