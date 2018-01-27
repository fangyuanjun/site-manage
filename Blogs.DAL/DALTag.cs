using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Blogs.IDAL;
using FYJ.Data.Entity;
using Blogs.Entity;

namespace Blogs.DAL
{
    public class DALTag : FYJ.Framework.Core.DAL.DALAbstract<Blogs.Entity.blog_tb_tag>, IDALTag
    {
        protected override string PrimaryKey
        {
            get { return "tagID"; }
        }

        /// <summary>
        /// 新增或修改标签
        /// </summary>
        /// <param name="blogID">博客ID</param>
        /// <param name="articleID">文章ID</param>
        /// <param name="tagDisplay">标签名</param>
        /// <returns></returns>
        public int UpdateTag(string blogID, string articleID, string tagDisplay)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("@blogID", blogID);
            dic.Add("@articleID", articleID);
            dic.Add("@str", tagDisplay);
            int result = this.DbInstance.ExecuteProcedure("blog_proc_articleTag", dic);

            return result;
        }


        public IEnumerable<Entity.blog_tb_tag> GetTags(string blogID)
        {
            string sql = "select * from blog_tb_tag where blogID=@blogID";
            DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@blogID", blogID));
            return FYJ.Common.ObjectHelper.DataTableToModel<blog_tb_tag>(dt);
        }
    }
}

