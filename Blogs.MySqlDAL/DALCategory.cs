using Blogs.IDAL;
using FYJ;
using FYJ.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Blogs.Entity;

namespace Blogs.DAL
{
    public class DALCategory : FYJ.Framework.Core.DAL.DALAbstract<Blogs.Entity.blog_tb_category>,IDALCategory
    {
        protected override string PrimaryKey
        {
            get { return "categoryID"; }
        }

        public override int Delete(string id)
        {
            string str = "";
            foreach (string s in id.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string t = s.Trim('\'');
                str += "'" + t + "',";
            }
            str = str.TrimEnd(',');

            if (DbInstance.Exists("SELECT 1 from blog_tb_category where categoryIsSystem=1 and  categoryID in (" + str + ")"))
            {
                throw new CustomException("选择的分类有系统分类存在，不能删除");
            }

            if (DbInstance.Exists("SELECT 1 from blog_tb_article where  categoryID in (" + str + ")"))
            {
                throw new CustomException("选择的分类有文章存在，如需删除请先删除对应文章");
            }

            if (DbInstance.Exists("SELECT 1 from blog_tb_category   where parentID in (" + str + ")"))
            {
                throw new CustomException("选择的分类有子分类，如需删除请先删除子类");
            }
            return base.Delete(id);
        }


        public List<blog_tb_category> GetList(string blogID, string parentID)
        {
            string sql = @"select table1.*,(select count(1) from blog_tb_category where parentID=table1.categoryID ) as rowcount,ifnull(ArticleCount,0) ArticleCount from blog_tb_category as table1 
                left join (
select COUNT(*) as ArticleCount,categoryID from blog_tb_article group by categoryID) as table2 on table2.categoryID=table1.categoryID
                where  blogID=@blogID ";
            List<IDbDataParameter> paras = new List<IDbDataParameter>();
            if (!String.IsNullOrEmpty(parentID))
            {
                sql += " and parentID=@parentID";
                paras.Add(DbInstance.CreateParameter("@parentID", parentID));
            }
            sql += "order by categoryOrderWeight desc";
            paras.Add(DbInstance.CreateParameter("@blogID", blogID));

            DataTable dt = DbInstance.GetDataTable(sql, paras);

            List<blog_tb_category> list = ObjectHelper.DataTableToModel<blog_tb_category>(dt);

            return list;
        }
    }
}

