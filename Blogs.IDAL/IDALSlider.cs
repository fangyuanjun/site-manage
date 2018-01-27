using Blogs.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blogs.IDAL
{
    public interface IDALSlider : FYJ.Framework.Core.IDAL.IDALBase<Blogs.Entity.blog_tb_slider>
    {
        List<blog_tb_slider> Query(string blogID);
    }
}
