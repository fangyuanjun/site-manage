using Blogs.Entity;
using Blogs.Entity.Util;
using Blogs.IDAL;
using FYJ.Common;
using FYJ.Data;
using FYJ.Data.Entity;
using FYJ.Framework.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Blogs.DAL
{
    public class DALBlog : IDALBlog
    {
        public IDbHelper DbInstance
        {
            get
            {
                return IocHelper<IDbHelperCreate>.Instance.GetDbInstance("Blogs");
            }
        }

        public blog_tb_blog GetEntity(string id)
        {
            EntityHelper<blog_tb_blog> eq = new EntityHelper<blog_tb_blog>("blog_tb_blog", "blogID", null);
            return eq.GetEntity(id, DbInstance);
        }

        /// <summary>
        /// 获取数据库第一条博客
        /// </summary>
        /// <returns></returns>
        /// fangyj 2015-6-22
        public blog_tb_blog GetFirstEntity()
        {
            string sql = "select top 1 * from blog_tb_blog where blogIsDisabled=0";
            DataTable dt = DbInstance.GetDataTable(sql);
            return FYJ.Common.ObjectHelper.DataTableToSingleModel<blog_tb_blog>(dt);
        }

        /// <summary>
        /// 获取用户的一个博客  (第一个)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>2015-07-25</returns>
        public blog_tb_blog GetSingleBlogByUserID(string userID)
        {
            string sql = "select top 1 * from blog_tb_blog where userID=@userID";
            DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@userID", userID));
            return FYJ.Common.ObjectHelper.DataTableToSingleModel<blog_tb_blog>(dt);
        }

        public DataSet GetBlogSitemap(string blogID)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("@blogID", blogID);
            DataSet ds = this.DbInstance.RunProcedure("blog_proc_sitemap", dic);

            return ds;
        }


        public DataSet GetBlogRss(string blogID)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("@blogID", blogID);
            DataSet ds = this.DbInstance.RunProcedure("blog_proc_rss", dic);

            return ds;
        }

        /// <summary>
        /// 获取博客母板页数据
        /// </summary>
        /// <param name="blogID"></param>
        /// <returns></returns>
        public DataSet GetBlogDataSet(string blogID)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("@blogID", blogID);
            DataSet ds = this.DbInstance.RunProcedure("blog_proc_main", dic);

            return ds;
        }

        /// <summary>
        /// 获取博客主页数据
        /// </summary>
        /// <param name="blogID"></param>
        /// <returns></returns>
        public DataSet GetBlogIndexDataSet(string blogID)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("@blogID", blogID);
            DataSet ds = this.DbInstance.RunProcedure("blog_proc_index", dic);

            return ds;
        }

        public DataTable GetMainArticlePic(string blogID)
        {
            string sql =
                @"
                select top 10 
                articleID,
                articlePic,
                articleTitle
                from blog_tb_article
                where blog_tb_article.blogID=@blogID
                and blog_tb_article.articleIsDisabled=0
                and blog_tb_article.articleIsHidden=0
                and blog_tb_article.articleIsDelete=0
                and articlePic is not null and articlePic<>'' order by articleDatetime desc
                ";
            DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@blogID", blogID));

            return dt;
        }



        public int Save(blog_tb_blog entity)
        {
            EntityHelper<blog_tb_blog> eq = new EntityHelper<blog_tb_blog>("blog_tb_blog", "blogID", null);
            string sql = "select 1 from blog_tb_blog where blogID=@blogID";
            if (DbInstance.Exists(sql, DbInstance.CreateParameter("blogID", entity.blogID)))
            {
                entity.UPDATE_DATE = DateTime.Now;
                return eq.Update(entity, DbInstance);
            }
            else
            {
                entity.blogID = new Random().Next(10000000, 99999999);
                entity.blogAddDatetime = DateTime.Now;
                entity.ADD_DATE = DateTime.Now;
                entity.UPDATE_DATE = DateTime.Now;
                return eq.Insert(entity, DbInstance);
            }
        }


        public DataSet GetSitemaphtml(string blogID)
        {
            string sql = "select menuUrl, menuDisplay,menuTarget,menuOrder from blog_tb_menu where blogID=@blogID and menuIsDisabled=0 	order by menuOrder DESC;";
            sql += @"  
            select 
            articleID,
            articleTitle,
            articleDatetime 
            from blog_tb_article 
            where blog_tb_article.blogID=@blogID
            and blog_tb_article.articleIsDisabled=0
            and blog_tb_article.articleIsHidden=0
            and blog_tb_article.articleIsDelete=0
            order by articleDatetime desc
                ";
            DataSet ds = DbInstance.GetDataSet(sql, blogID);
            return ds;
        }

        public List<string> CheckID(string userID)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("@userID", userID);
            DataSet ds = this.DbInstance.RunProcedure("blog_proc_AllID", dic);
            DataTable dt = ds.Tables[0];
            List<string> list = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(dr["id"].ToString());
            }

            return list;
        }


        public blog_tb_blog GetSingleBlogByDomain(string domain)
        {
            string sql = "select top 1 * from blog_tb_blog where replace(blogDomain,'www.','')=@blogDomain";
            DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@blogDomain", domain.Replace("www.", "")));
            if (dt.Rows.Count > 0)
            {
                return FYJ.Common.ObjectHelper.DataTableToSingleModel<blog_tb_blog>(dt);
            }

            sql = "select blogID from blog_tb_category where categoryDomain=@categoryDomain";
            dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@categoryDomain", domain));
            if (dt.Rows.Count > 0)
            {
                return GetEntity(dt.Rows[0]["blogID"].ToString());
            }

            sql = "select * from blog_tb_blog where blogName=@blogName";
            dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@blogName", domain.Replace(".devblog.cn", "")));
            if (dt.Rows.Count > 0)
            {
                return FYJ.Common.ObjectHelper.DataTableToSingleModel<blog_tb_blog>(dt);
            }

            throw new BlogNotFindException();
        }


        public List<blog_tb_SiteCategory> QuerySiteCategory()
        {
            string sql = "select * from blog_tb_SiteCategory where  IsDisabled=0 order by OrderWeight desc";
            DataTable dt = DbInstance.GetDataTable(sql);
            return ObjectHelper.DataTableToModel<blog_tb_SiteCategory>(dt);
        }


        //public int AddVisit(Entity.blog_tb_Visit entity)
        //{
        //    Uri u = new Uri(entity.visitUrl);
        //    string domain = u.Host.TrimEnd('.');
        //    if (domain.StartsWith("www."))
        //    {
        //        domain = domain.Substring(4);
        //    }

        //    string sql = "";
        //    db_commonEntities ctx = new db_commonEntities();
        //    if (ctx.blog_tb_IpAddress.Select(x => x.IP == entity.visitIP).Count() == 0)
        //    {
        //        blog_tb_IpAddress address = new blog_tb_IpAddress();
        //        address.id = Guid.NewGuid().ToString("N");
        //        address.IP = entity.visitIP;
        //        address.City = entity.City;
        //        address.Contry = entity.County;
        //        address.ADD_DATE = DateTime.Now;
        //        address.UPDATE_DATE = DateTime.Now;

        //        AddIPAddress(address);
        //    }

        //    blog_tb_VisitCount countEntity = ctx.blog_tb_VisitCount.SingleOrDefault(x => x.SiteID == entity.siteID && x.CountDate == DateTime.Now);
        //    bool isAddCount = (countEntity == null);
        //    if (countEntity == null)
        //    {
        //        countEntity = new blog_tb_VisitCount();
        //        countEntity.Domain = domain;
        //        countEntity.SiteID = entity.siteID;
        //        countEntity.ID = Guid.NewGuid().ToString("N");
        //        countEntity.ADD_DATE = DateTime.Now;
        //        countEntity.CountDate = DateTime.Now;
        //    }

        //    countEntity.PV += 1;

        //    if (entity.SessionID != null)
        //    {
        //        sql = "select * from blog_tb_Visit where siteID=@siteID and SessionID=@SessionID and DATEDIFF(DAY,ADD_DATE,getdate())=0";
        //        if (!DbInstance.Exists(sql, DbInstance.CreateParameter("@siteID", entity.siteID), DbInstance.CreateParameter("@SessionID", entity.SessionID)))
        //        {
        //            countEntity.UV += 1;
        //        }
        //    }

        //    sql = "select * from blog_tb_Visit where siteID=@siteID and visitIP=@visitIP and DATEDIFF(DAY,ADD_DATE,getdate())=0";
        //    if (!DbInstance.Exists(sql, DbInstance.CreateParameter("@siteID", entity.siteID), DbInstance.CreateParameter("@visitIP", entity.visitIP)))
        //    {
        //        countEntity.IP += 1;
        //    }

        //    countEntity.UPDATE_DATE = DateTime.Now;

        //    if (isAddCount)
        //    {
        //        AddCount(countEntity);
        //    }
        //    else
        //    {
        //        UpdateCount(countEntity);
        //    }

        //    FYJ.Data.Entity.EntityHelper<blog_tb_Visit>.Insert(entity, "blog_tb_Visit", "visitID", true, DbInstance);
        //    return 1;
        //}

        public int SaveBlogCount(string blogID)
        {
            string sql = "select  * from blog_tb_blog_count where blogID=@blogID";
            if (DbInstance.Exists(sql, DbInstance.CreateParameter("@blogID", blogID)))
            {
                sql = "update blog_tb_blog_count set PV=PV+1,UPDATE_DATE=@UPDATE_DATE where blogID=@blogID";
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("blogID", blogID);
                dic.Add("UPDATE_DATE", DateTime.Now);
                DbInstance.ExecuteSql(sql, dic);
            }
            else
            {
                blog_tb_blog_count entity = new blog_tb_blog_count();
                entity.ID = Guid.NewGuid().ToString("N");
                entity.BlogID = Convert.ToInt32(blogID);
                entity.PV = 1;
                entity.AddPV = 0;
                entity.ADD_DATE = DateTime.Now;
                entity.UPDATE_DATE = DateTime.Now;

                FYJ.Data.Entity.EntityHelper<blog_tb_blog_count>.Insert(entity, "blog_tb_blog_count", "ID", true, DbInstance);
            }

            return 1;
        }

        public int GetTotalPV(string blogID)
        {
            string sql = "select  * from blog_tb_blog_count where blogID=@blogID";
            if (DbInstance.Exists(sql, DbInstance.CreateParameter("@blogID", blogID)))
            {
                sql = "select PV+AddPV as TotalPV from blog_tb_blog_count where blogID=@blogID";
                return DbInstance.GetInt(sql, DbInstance.CreateParameter("@blogID", blogID));
            }

            return 0;
        }

        public bool IsExistsBlogName(string name)
        {
            string sql = "select top 1 * from blog_tb_blog where blogName=@blogName";
            bool b = DbInstance.Exists(sql, DbInstance.CreateParameter("@blogName", name));
            return b;
        }
    }
}

