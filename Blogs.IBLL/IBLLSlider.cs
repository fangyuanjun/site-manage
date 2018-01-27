using Blogs.Entity;
using FYJ.Framework.Core.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blogs.IBLL
{
    public interface IBLLSlider : IBLLBase<Blogs.Entity.blog_tb_slider>
    {
        List<blog_tb_slider> Query(string blogID);

    }
}
