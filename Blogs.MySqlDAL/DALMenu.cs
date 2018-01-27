
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using FYJ.Data.Entity;
using FYJ;
using Blogs.Entity;

namespace Blogs.DAL
{
    public class DALMenu : FYJ.Framework.Core.DAL.DALAbstract<blog_tb_menu>, Blogs.IDAL.IDALMenu
    {
        protected override string PrimaryKey
        {
            get { return "menuID"; }
        }

        public List<blog_tb_menu> GetList(string blogID, string parentID)
        {
            string sql = @"select *,(select count(1) from blog_tb_menu where parentID=table1.menuID ) as rowcount from blog_tb_menu as table1 where  blogID=@blogID ";
            List<IDbDataParameter> paras = new List<IDbDataParameter>();
            if (!String.IsNullOrEmpty(parentID))
            {
                sql += " and parentID=@parentID";
                paras.Add(DbInstance.CreateParameter("@parentID", parentID));
            }
            sql += " order by menuOrder desc";
            paras.Add(DbInstance.CreateParameter("@blogID", blogID));

            DataTable dt = DbInstance.GetDataTable(sql, paras);

            List<blog_tb_menu> list = ObjectHelper.DataTableToModel<blog_tb_menu>(dt);

            return list;
        }
    }
}

