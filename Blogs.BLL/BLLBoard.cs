using Blogs.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blogs.BLL
{
    public class BLLBoard : FYJ.Framework.Core.BLL.BLLAbstract<Blogs.Entity.blog_tb_Board,IDAL.IDALBoard>, IBLL.IBLLBoard
    {

        public IList<blog_tb_Board> Query(int state)
        {
            return Dal.Query(state);
        }
    }
}
