
using Blogs.Entity;
using FYJ.Framework.Core.IBLL;
using System;
using System.Collections.Generic;
using System.Data;



namespace Blogs.IBLL
{
    public interface IBLLLink : IBLLBase<Blogs.Entity.blog_tb_link>
    {
        ICollection<blog_tb_link> Query(string blogID);
    }
}
