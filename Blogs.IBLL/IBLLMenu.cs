
using Blogs.Entity;
using FYJ.Framework.Core.IBLL;
using System;
using System.Collections.Generic;
using System.Data;



namespace Blogs.IBLL
{
    public interface IBLLMenu : IBLLBase<blog_tb_menu>
    {

        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <param name="blogID"></param>
        /// <returns></returns>
        List<blog_tb_menu> GetAllList(string blogID);

        List<blog_tb_menu> GetTreeData(string blogID);
    }
}
