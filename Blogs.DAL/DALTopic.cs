

using Blogs.Entity;
using Blogs.IDAL;
using FYJ.Data.Entity;
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

        public System.Collections.Generic.IEnumerable<Entity.blog_tb_topic> GetTopic(string blogID)
        {
            string sql = "select * from blog_tb_topic where blogID=@blogID";
            DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@blogID", blogID));
            return FYJ.Common.ObjectHelper.DataTableToModel<blog_tb_topic>(dt);
        }
    }
}

