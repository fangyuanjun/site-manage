using Blogs.Entity;
using Blogs.IDAL;
using FYJ;
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
                return IocFactory<FYJ.Data.IDbFactory>.Instance.GetDbInstance("Blogs");
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
            string sql = "select  * from blog_tb_blog where blogIsDisabled=0 limit 0,1";
            DataTable dt = DbInstance.GetDataTable(sql);
            return ObjectHelper.DataTableToSingleModel<blog_tb_blog>(dt);
        }

        /// <summary>
        /// 获取用户的一个博客  (第一个)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>2015-07-25</returns>
        public blog_tb_blog GetSingleBlogByUserID(string userID)
        {
            string sql = "select  * from blog_tb_blog where userID=@userID limit 0,1";
            DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@userID", userID));
            return ObjectHelper.DataTableToSingleModel<blog_tb_blog>(dt);
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


       
        public List<string> CheckID(string userID)
        {
            //Dictionary<string, object> dic = new Dictionary<string, object>();
            //dic.Add("@userID", userID);
            //DataSet ds = this.DbInstance.RunProcedure("blog_proc_AllID", dic);
            //DataTable dt = ds.Tables[0];
            //List<string> list = new List<string>();
            //foreach (DataRow dr in dt.Rows)
            //{
            //    list.Add(dr["id"].ToString());
            //}

            //return list;
            throw new NotImplementedException();
        }


        public blog_tb_blog GetSingleBlogByDomain(string domain,int port)
        {
            string sql = "select  * from blog_tb_blog where blogID =(select blogID from blog_tb_domain where replace(blogDomain,'www.','')=@blogDomain and port=@port limit 0,1) ";
            DataTable dt = DbInstance.GetDataTable(sql
                , DbInstance.CreateParameter("@blogDomain", domain.Replace("www.", ""))
                , DbInstance.CreateParameter("@port", port));
            if (dt.Rows.Count > 0)
            {
                return ObjectHelper.DataTableToSingleModel<blog_tb_blog>(dt);
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
                return ObjectHelper.DataTableToSingleModel<blog_tb_blog>(dt);
            }

            //如果都没查找到取第一条
            sql = "select * from blog_tb_blog limit 0,1";
            dt = DbInstance.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                return ObjectHelper.DataTableToSingleModel<blog_tb_blog>(dt);
            }

            return null;
        }


        public List<blog_tb_SiteCategory> QuerySiteCategory()
        {
            string sql = "select * from blog_tb_SiteCategory where  IsDisabled=0 order by OrderWeight desc";
            DataTable dt = DbInstance.GetDataTable(sql);
            return ObjectHelper.DataTableToModel<blog_tb_SiteCategory>(dt);
        }

        public int SaveBlogCount(string blogID)
        {
            string sql = "select  * from blog_tb_blog_count where blogID=@blogID";
            IDbHelper db = IocFactory<FYJ.Data.IDbFactory>.Instance.GetDbInstance("Blogs-Write");
            if (db.Exists(sql, db.CreateParameter("@blogID", blogID)))
            {
                sql = "update blog_tb_blog_count set PV=IFNULL(PV,0)+1,UPDATE_DATE=@UPDATE_DATE where blogID=@blogID";
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("blogID", blogID);
                dic.Add("UPDATE_DATE", DateTime.Now);
                db.ExecuteSql(sql, dic);
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

                FYJ.Data.Entity.EntityHelper<blog_tb_blog_count>.Insert(entity, "blog_tb_blog_count", "ID", true, db);
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
            string sql = "select  * from blog_tb_blog where blogName=@blogName  limit 0,1";
            bool b = DbInstance.Exists(sql, DbInstance.CreateParameter("@blogName", name));
            return b;
        }

        public bool IsBlogDisabled(string blogID)
        {
            string sql = "select  blogIsDisabled from blog_tb_blog where blogID=@blogID";
            return DbInstance.GetBoolean(sql, DbInstance.CreateParameter("@blogID", blogID));
        }

        public int AddDomain(blog_tb_domain entity)
        {
            IDbHelper db = IocFactory<FYJ.Data.IDbFactory>.Instance.GetDbInstance("Blogs-Write");
            return EntityHelper<blog_tb_domain>.Insert(entity, "blog_tb_domain", "ID", true, db);
        }

        public int UpdateDomain(blog_tb_domain entity)
        {
            IDbHelper db = IocFactory<FYJ.Data.IDbFactory>.Instance.GetDbInstance("Blogs-Write");
            return EntityHelper<blog_tb_domain>.Update(entity, "blog_tb_domain",db, "ID");
        }

        public blog_tb_domain QueryDomain(string blogID, string domain, int port)
        {
            if(domain==null)
            {
                domain = "";
            }
            else
            {
                domain = domain.Trim();
            }

            string sql = "select * from blog_tb_domain where blogID=@blogID and ifnull(domain,'')=@domain and port=@port";
            DataTable dt = DbInstance.GetDataTable(sql,DbInstance.CreateParameter("@blogID",blogID)
                , DbInstance.CreateParameter("@domain", domain)
                , DbInstance.CreateParameter("@port", port));

            return ObjectHelper.DataTableToSingleModel<blog_tb_domain>(dt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
       public blog_tb_domain GetDomainEntity(string ID)
        {
            string sql = "select * from blog_tb_domain where ID=@ID";
            DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@ID", ID));

            return ObjectHelper.DataTableToSingleModel<blog_tb_domain>(dt);
        }

        public int DeleteSingleDomain(string ID)
        {
            string sql = "delete from blog_tb_domain where ID=@ID";
            IDbHelper db = IocFactory<FYJ.Data.IDbFactory>.Instance.GetDbInstance("Blogs-Write");
            return db.ExecuteSql(sql,db.CreateParameter("@ID",ID));
        }

        public List<blog_tb_domain> GetDomainList(string blogID)
        {
            string sql = "select * from blog_tb_domain where blogID=@blogID";
            DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@blogID", blogID));

            return ObjectHelper.DataTableToModel<blog_tb_domain>(dt);
        }
    }
}

