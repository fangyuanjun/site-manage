using Blogs.Entity;
using FYJ.Framework.Core.IBLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Blogs.IBLL
{
    public interface IBLLBoard : IBLLBase<Blogs.Entity.blog_tb_Board>
    {
        /// <summary>
        /// 查询留言板
        /// </summary>
        /// <param name="state">-1 则查询所有状态</param>
        /// <returns></returns>
        IList<blog_tb_Board> Query(int state);
    }
}
