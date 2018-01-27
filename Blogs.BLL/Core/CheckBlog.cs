using Blogs.Entity;
using FYJ;
using FYJ.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.BLL
{
    public class CheckBlog
    {
        public static void ValidateBlog(Type modeltype, string ids)
        {
            string userID = IocFactory<IBlogInfo>.Instance.CurrentUserID;
            string blogID = IocFactory<IBlogInfo>.Instance.CurrentBlogID;

            if (modeltype == typeof(blog_tb_Album))
            {
                Check("blog_tb_Album","ID",ids,"userID",userID);
            }
           else if (modeltype == typeof(blog_tb_article))
            {
                Check("blog_tb_article", "articleID", ids, "blogID", blogID);
            }
           else if (modeltype == typeof(blog_tb_blog))
            {
                Check("blog_tb_blog", "blogID", ids, "userID", userID);
            }
            else if (modeltype == typeof(blog_tb_Board))
            {
                Check("blog_tb_Board", "ID", ids, "blogID", blogID);
            }
            else if (modeltype == typeof(blog_tb_category))
            {
                Check("blog_tb_category", "categoryID", ids, "blogID", blogID);
            }
            else if (modeltype == typeof(blog_tb_comment))
            {
                
            }
            else if (modeltype == typeof(blog_tb_domain))
            {
                Check("blog_tb_domain", "ID", ids, "blogID", blogID);
            }
            else if (modeltype == typeof(blog_tb_Photo))
            {
                Check("blog_tb_Photo", "ID", ids, "UserID", userID);
            }
            else if (modeltype == typeof(blog_tb_slider))
            {
                Check("blog_tb_slider", "ID", ids, "blogID", blogID);
            }
            else if (modeltype == typeof(blog_tb_tag))
            {
                Check("blog_tb_tag", "tagID", ids, "blogID", blogID);
            }
            else if (modeltype == typeof(blog_tb_Talk))
            {
                Check("blog_tb_Talk", "ID", ids, "userID", userID);
            }
            else if (modeltype == typeof(blog_tb_topic))
            {
                Check("blog_tb_topic", "topicID", ids, "blogID", blogID);
            }
        }

        private static IDbHelper DbInstance
        {
            get
            {
                return IocFactory<FYJ.Data.IDbFactory>.Instance.GetDbInstance("Blogs");
            }
        }

        private static void Check(string tableName, string primaryName, string ids, string checkName, string chekcValue)
        {
            if (String.IsNullOrEmpty(ids)||ids=="0")  //如果是新增
            {
                return;
            }

            string tmp = "";
            foreach (string s in ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string t = s.Trim('\'');
                tmp += "'" + t + "',";
            }
            tmp = tmp.TrimEnd(',');

            string sql = "select " + checkName + "  from " + tableName + " where " + primaryName + "  in (" + tmp + ")";

            DataTable dt = DbInstance.GetDataTable(sql);
            if (dt.Rows.Count == 0)
            {
                if(tableName.Equals("blog_tb_article",StringComparison.CurrentCultureIgnoreCase))  //因为文章新增的时候就有ID
                {
                    return;
                }

                throw new CustomException("id不存在");
            }

            foreach (DataRow dr in dt.Rows)
            {
                //只要有一项的userID不等于参数userID 则认证失败
                if (dr[0].ToString() != chekcValue)
                {
                    throw new CustomException("无权限");
                }
            }

        }
    }
}
