
using System;
using System.Collections.Generic;
using System.Data;
using Blogs.Entity;

namespace Blogs.IDAL
{
    public interface IDALLink : FYJ.Framework.Core.IDAL.IDALBase<blog_tb_link>
    {
        ICollection<blog_tb_link> Query(string blogID);
    }
}
