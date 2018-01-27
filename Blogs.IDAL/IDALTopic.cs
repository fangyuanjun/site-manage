
using System.Collections.Generic;
using Blogs.Entity;

namespace Blogs.IDAL
{
    public interface IDALTopic : FYJ.Framework.Core.IDAL.IDALBase<Blogs.Entity.blog_tb_topic>
    {
        IEnumerable<blog_tb_topic> GetTopic(string blogID);
    }
}
