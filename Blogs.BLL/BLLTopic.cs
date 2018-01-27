
using Blogs.Entity;
using Blogs.IDAL;
using System;
using System.Collections.Generic;
using System.Data;



namespace Blogs.BLL
{
    public class BLLTopic : FYJ.Framework.Core.BLL.BLLAbstract<Blogs.Entity.blog_tb_topic, IDALTopic>, IBLL.IBLLTopic
    {
        public IEnumerable<blog_tb_topic> GetTopic(string blogID)
        {
            return Dal.GetTopic(blogID);
        }
    }
}
