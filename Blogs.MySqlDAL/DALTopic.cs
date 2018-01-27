

using Blogs.Entity;
using Blogs.IDAL;
using FYJ;
using FYJ.Data.Entity;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Blogs.DAL
{
    public class DALTopic : FYJ.Framework.Core.DAL.DALAbstract<Blogs.Entity.blog_tb_topic>, IDALTopic
    {
        protected override string PrimaryKey
        {
            get { return "topicID"; }
        }

        public IEnumerable<Entity.blog_tb_topic> GetTopic(string blogID)
        {
            string sql = "select blog_tb_topic.*,ifnull(ArticleCount,0) ArticleCount from blog_tb_topic   LEFT JOIN (select count(*) ArticleCount ,topicID from blog_tb_article GROUP BY topicID) table1 on table1.topicID=blog_tb_topic.topicID where blogID=@blogID";
            DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@blogID", blogID));
            return ObjectHelper.DataTableToModel<blog_tb_topic>(dt);
        }
    }
}

