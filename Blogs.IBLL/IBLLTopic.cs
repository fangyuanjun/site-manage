using Blogs.Entity;
using FYJ.Framework.Core.IBLL;
using System;
using System.Collections.Generic;
using System.Data;

namespace Blogs.IBLL
{
    public interface IBLLTopic : IBLLBase<Blogs.Entity.blog_tb_topic>
    {
        IEnumerable<blog_tb_topic> GetTopic(string blogID);
    }
}
