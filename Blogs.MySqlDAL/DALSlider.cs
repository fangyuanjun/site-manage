using Blogs.Entity;
using Blogs.IDAL;
using FYJ;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Blogs.DAL
{
    public class DALSlider : FYJ.Framework.Core.DAL.DALAbstract<Blogs.Entity.blog_tb_slider>, IDALSlider
    {
        protected override string PrimaryKey
        {
            get { return "ID"; }
        }

        public List<Entity.blog_tb_slider> Query(string blogID)
        {
            string sql = "select   *  from blog_tb_slider where BlogID=@BlogID   order by OrderWeight DESC limit 0,5";
            DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@BlogID", blogID));
            return ObjectHelper.DataTableToModel<blog_tb_slider>(dt);
        }
    }
}
