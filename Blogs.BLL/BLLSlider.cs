using Blogs.Entity;
using Blogs.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blogs.BLL
{
    public class BLLSlider : FYJ.Framework.Core.BLL.BLLAbstract<Blogs.Entity.blog_tb_slider, IDALSlider>, IBLL.IBLLSlider
    {

        public List<blog_tb_slider> Query(string blogID)
        {
            return Dal.Query(blogID);
        }
    }
}
