
using System;
using System.Collections.Generic;
using System.Data;
using Blogs.Entity;
using FYJ.Framework.Core.IBLL;


namespace Blogs.IBLL
{
    public interface IBLLCategory : IBLLBase<blog_tb_category>
    {
        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <param name="blogID"></param>
        /// <returns></returns>
        List<blog_tb_category> GetAllList(string blogID);

        List<blog_tb_category> GetTreeData(string blogID);
    }
}
