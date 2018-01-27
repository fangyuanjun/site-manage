
using Blogs.IDAL;
using FYJ.Framework.Core.BLL;
using System;
using System.Collections.Generic;
using System.Data;



namespace Blogs.BLL
{
    public class BLLLink : BLLAbstract<Blogs.Entity.blog_tb_link, IDALLink>, IBLL.IBLLLink
    {
        public ICollection<Entity.blog_tb_link> Query(string blogID)
        {
            return Dal.Query(blogID);
        }

    }
}
