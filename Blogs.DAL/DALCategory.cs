using Blogs.IDAL;
using FYJ.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace Blogs.DAL
{
    public class DALCategory : FYJ.Framework.Core.DAL.DALAbstract<Blogs.Entity.blog_tb_category>,IDALCategory
    {
        protected override string PrimaryKey
        {
            get { return "categoryID"; }
        }


        int treeDeepIndex = 1;
        private String DeepSpace()
        {
            String str = "";
            for (int i = 0; i < treeDeepIndex; i++)
            {
                str += "　";//全角空格
            }

            return str;
        }

        private void AddcategoryDisplayTree(String pid, DataTable newTable, DataTable dt)
        {
            DataRow[] drs = dt.Select("parentID='" + pid + "'");
            foreach (DataRow dr in drs)
            {
                DataRow newRow = newTable.NewRow();
                newRow["categoryID"] = dr["categoryID"].ToString();
                newRow["categoryDisplay"] = DeepSpace() + dr["categoryDisplay"].ToString();
                newTable.Rows.Add(newRow); //一定要在下句的前面
                if (Convert.ToInt32(dr["cc"]) > 0)
                {
                    treeDeepIndex++;
                    AddcategoryDisplayTree(dr["categoryID"].ToString(), newTable, dt);
                }
               
            }
        }

        public DataTable GetcategorysTreeTable(string blogID)
        {
            treeDeepIndex = 1;
            string sql = "select categoryID,categoryDisplay,parentID,(select count(1) from blog_tb_category where parentID=table1.categoryID ) as cc from blog_tb_category as table1 where  blogID=@blogID order by categoryOrderWeight desc";
            DataTable dt = DbInstance.GetDataTable(sql,DbInstance.CreateParameter("@blogID",blogID));
            DataTable newTable = dt.Clone();
            foreach (DataRow dr in dt.Rows)
            {
                DataRow newRow = newTable.NewRow();
                newRow["categoryID"] = dr["categoryID"].ToString();
                newRow["categoryDisplay"] = dr["categoryDisplay"].ToString();
                if (dr["parentID"].ToString() == "" || dr["parentID"].ToString() == "0")
                {
                    newTable.Rows.Add(newRow);//一定要在下句的前面
                    if (Convert.ToInt32(dr["cc"]) > 0)
                    {
                        AddcategoryDisplayTree(dr["categoryID"].ToString(), newTable, dt);
                    }
                }
            }

            return newTable;
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

            if (DbInstance.Exists("SELECT 1 from blog_view_article where  categoryID in (" + str + ")"))
            {
                throw new CustomException("选择的分类有文章存在，如需删除请先删除对应文章");
            }

            if (DbInstance.Exists("SELECT 1 from blog_tb_category   where parentID in (" + str + ")"))
            {
                throw new CustomException("选择的分类有子分类，如需删除请先删除子类");
            }
            return base.Delete(id);
        }


        public DataTable GetCategoryTable(string blogID)
        {
            String sql = @"
select blog_tb_category.categoryID,categoryDisplay,categoryDomain,categoryOrderWeight,categoryIsDisabled,blog_tb_category.parentID,categoryName,isnull(cc2,0) cc2
,isnull(cc,0) cc,blog_tb_category.ADD_DATE,blog_tb_category.UPDATE_DATE 
from blog_tb_category  inner join blog_tb_blog on blog_tb_category.blogID=blog_tb_blog.blogID
left join (
select COUNT(*) as cc2,parentID from blog_tb_category group by parentID )as table1 on table1.parentID=blog_tb_category.categoryID
left join (
select COUNT(*) as cc,categoryID from blog_view_article group by categoryID) as table2 on table2.categoryID=blog_tb_category.categoryID
";
            sql += " where blog_tb_category.blogID='" + blogID + "'";
            DataTable dt = DbInstance.GetDataTable(sql);

            return dt;
        }
    }
}

