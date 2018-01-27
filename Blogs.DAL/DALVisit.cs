using Blogs.Entity;
using Blogs.IDAL;
using FYJ.Framework.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.DAL
{
    public class DALVisit : IDALVisit
    {

        protected FYJ.Data.IDbHelper DbInstance
        {
            get
            {
                return IocHelper<IDbHelperCreate>.Instance.GetDbInstance("Visit");
            }
        }


        public int AddCount(Entity.blog_tb_VisitCount entity)
        {
            return FYJ.Data.Entity.EntityHelper<blog_tb_VisitCount>.Insert(entity, "blog_tb_VisitCount", "ID", true, DbInstance);
        }

        public int UpdateCount(blog_tb_VisitCount entity)
        {
            return FYJ.Data.Entity.EntityHelper<blog_tb_VisitCount>.Update(entity, "blog_tb_VisitCount", DbInstance, "ID");
        }

        public int AddVisit(Entity.blog_tb_Visit entity)
        {
            Uri u = new Uri(entity.visitUrl);
            string domain = u.Host.TrimEnd('.');
            if (domain.StartsWith("www."))
            {
                domain = domain.Substring(4);
            }

            string sql = "";

            if (!DbInstance.Exists("select * from blog_tb_IpAddress where IP=@IP", DbInstance.CreateParameter("@IP", entity.visitIP)))
            {
                blog_tb_IpAddress address = new blog_tb_IpAddress();
                address.id = Guid.NewGuid().ToString("N");
                address.IP = entity.visitIP;
                address.City = entity.City;
                address.Contry = entity.County;
                address.ADD_DATE = DateTime.Now;
                address.UPDATE_DATE = DateTime.Now;

                AddIPAddress(address);
            }

            blog_tb_VisitCount countEntity = null;
            if (String.IsNullOrEmpty(entity.siteID))
            {
                sql = "select * from blog_tb_VisitCount where (SiteID is null or SiteID='') and CountDate=@CountDate";
                DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@CountDate", DateTime.Now.Date));
                countEntity = FYJ.Common.ObjectHelper.DataTableToSingleModel<blog_tb_VisitCount>(dt);
            }
            else
            {
                sql = "select * from blog_tb_VisitCount where SiteID=@SiteID and CountDate=@CountDate";
                DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@SiteID", entity.siteID), DbInstance.CreateParameter("@CountDate", DateTime.Now.Date));
                countEntity = FYJ.Common.ObjectHelper.DataTableToSingleModel<blog_tb_VisitCount>(dt);
            }

            bool isAddCount = (countEntity == null);
            if (countEntity == null)
            {
                countEntity = new blog_tb_VisitCount();
                countEntity.Domain = domain;
                countEntity.SiteID = entity.siteID;
                countEntity.ID = Guid.NewGuid().ToString("N");
                countEntity.ADD_DATE = DateTime.Now;
                countEntity.CountDate = DateTime.Now;
            }

            countEntity.PV += 1;

            if (String.IsNullOrEmpty(entity.siteID))
            {
                if (entity.SessionID != null)
                {
                    sql = "select * from blog_tb_Visit where siteID is null or siteID='' and SessionID=@SessionID and DATEDIFF(DAY,ADD_DATE,getdate())=0";
                    if (!DbInstance.Exists(sql, DbInstance.CreateParameter("@SessionID", entity.SessionID)))
                    {
                        countEntity.UV += 1;
                    }
                }

                sql = "select * from blog_tb_Visit where siteID is null or siteID='' and visitIP=@visitIP and DATEDIFF(DAY,ADD_DATE,getdate())=0";
                if (!DbInstance.Exists(sql, DbInstance.CreateParameter("@visitIP", entity.visitIP)))
                {
                    countEntity.IP += 1;
                }
            }
            else
            {
                if (entity.SessionID != null)
                {
                    sql = "select * from blog_tb_Visit where siteID=@siteID and SessionID=@SessionID and DATEDIFF(DAY,ADD_DATE,getdate())=0";
                    if (!DbInstance.Exists(sql, DbInstance.CreateParameter("@siteID", entity.siteID), DbInstance.CreateParameter("@SessionID", entity.SessionID)))
                    {
                        countEntity.UV += 1;
                    }
                }

                sql = "select * from blog_tb_Visit where siteID=@siteID and visitIP=@visitIP and DATEDIFF(DAY,ADD_DATE,getdate())=0";
                if (!DbInstance.Exists(sql, DbInstance.CreateParameter("@siteID", entity.siteID), DbInstance.CreateParameter("@visitIP", entity.visitIP)))
                {
                    countEntity.IP += 1;
                }
            }



            countEntity.UPDATE_DATE = DateTime.Now;

            if (isAddCount)
            {
                AddCount(countEntity);
            }
            else
            {
                UpdateCount(countEntity);
            }

            FYJ.Data.Entity.EntityHelper<blog_tb_Visit>.Insert(entity, "blog_tb_Visit", "visitID", true, DbInstance);
            return 1;
        }

        public int AddIPAddress(blog_tb_IpAddress entity)
        {
            return FYJ.Data.Entity.EntityHelper<blog_tb_IpAddress>.Insert(entity, "blog_tb_IpAddress", "ID", true, DbInstance);
        }


    }
}
