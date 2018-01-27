using Blogs.Entity;
using Blogs.IDAL;
using FYJ;
using FYJ.Common;
using FYJ.Data.Util;
using FYJ.Framework.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.DAL
{
    public class DALArticleView : IDALArticleView
    {

        public FYJ.Data.IDbHelper DbInstance
        {
            get
            {
                return IocFactory<FYJ.Data.IDbFactory>.Instance.GetDbInstance("Blogs");
            }
        }

        public IEnumerable<blog_view_article> GetList(ref GridPager pager)
        {
            int count = 0;
            List<IDataParameter> plist = null;
            if (pager.Parameters != null && pager.Parameters.Count > 0)
            {
                plist = new List<IDataParameter>();
                foreach (var v in pager.Parameters)
                {
                    plist.Add(DbInstance.CreateParameter(v.Key, v.Value));
                }
            }
            DataTable dt = DbInstance.GetDataTable(out count, "blog_view_article", pager.OrderColumn + " " + pager.Order, plist, "*", pager.Where, pager.CurrentPage, pager.PageSize);
            pager.TotalRows = count;
            List<blog_view_article> list = ObjectHelper.DataTableToModel<blog_view_article>(dt);

            return list;
        }



        public IEnumerable<blog_view_article> Query(ArticleViewQueryEntity queryEntity, ref GridPager pager)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string where = " 1=1";

            where += " and blogID=@blogID";
            dic.Add("@blogID", queryEntity.BlogID);

            if (!String.IsNullOrWhiteSpace(queryEntity.SiteCategoryID))
            {
                where += " and siteCategoryID=@siteCategoryID";
                dic.Add("@siteCategoryID", queryEntity.SiteCategoryID);
            }
            if (!String.IsNullOrWhiteSpace(queryEntity.CategoryID))
            {
                where += " and categoryID=@categoryID";
                dic.Add("@categoryID", queryEntity.CategoryID);
            }
            if (!String.IsNullOrWhiteSpace(queryEntity.TopicID))
            {
                where += " and topicID=@topicID";
                dic.Add("@topicID", queryEntity.StartDate);
            }
            if (!String.IsNullOrWhiteSpace(queryEntity.ArticleIsOriginal))
            {
                bool b = (Int32.Parse(queryEntity.ArticleIsOriginal) == 1);
                where += " and articleIsOriginal=@articleIsOriginal";
                dic.Add("@articleIsOriginal", b);
            }
            if (!String.IsNullOrWhiteSpace(queryEntity.StartDate))
            {
                where += " and articleDatetim>=@StartDate";
                dic.Add("@StartDate", Convert.ToDateTime(queryEntity.StartDate).Date);
            }
            if (!String.IsNullOrWhiteSpace(queryEntity.EndDate))
            {
                where += " and articleDatetim<=@EndDate";
                dic.Add("@EndDate", Convert.ToDateTime(queryEntity.EndDate).Date);
            }
            if (!String.IsNullOrWhiteSpace(queryEntity.ArticleTitle))
            {
                where += " and LOWER(articleTitle)  like '%@articleTitle%'";
                dic.Add("@articleTitle", queryEntity.ArticleTitle.Trim().ToLower());
            }

            pager.Where = where;
            pager.Parameters = dic;
            IEnumerable<Blogs.Entity.blog_view_article> list = GetList(ref pager);

            return list;
        }
    }
}
