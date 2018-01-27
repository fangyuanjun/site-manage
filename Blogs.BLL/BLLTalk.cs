using Blogs.Entity;
using Blogs.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blogs.BLL
{
    public class BLLTalk : FYJ.Framework.Core.BLL.BLLAbstract<Blogs.Entity.blog_tb_Talk, IDALTalk>, IBLL.IBLLTalk
    {

        public string ReadTemp(string userID)
        {
            return Dal.ReadTemp(userID);
        }

        public int SaveTemp(string userID, string content)
        {
            return Dal.SaveTemp(userID,content);
        }


        public List<blog_tb_Talk> Query(string userID)
        {
            return Dal.Query(userID);
        }
    }
}
