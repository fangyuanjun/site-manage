
using System;
using System.Collections.Generic;
using System.Data;
using Blogs.Entity;

namespace Blogs.IDAL
{
    public interface IDALCategory : FYJ.Framework.Core.IDAL.IDALBase<blog_tb_category>
    {
        List<blog_tb_category> GetList(string blogID, string parentID);
    }
}
