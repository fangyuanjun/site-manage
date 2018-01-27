using Blogs.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Blogs.IDAL
{
    public interface IDALBoard : FYJ.Framework.Core.IDAL.IDALBase<Blogs.Entity.blog_tb_Board>
    {
        /// <summary>
        /// 查询留言板
        /// </summary>
        /// <param name="state">-1 则查询所有状态</param>
        /// <returns></returns>
        IList<blog_tb_Board> Query(int state);
    }
}
