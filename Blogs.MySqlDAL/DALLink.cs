using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using Blogs.IDAL;
using Blogs.Entity;
using FYJ;

namespace Blogs.DAL
{
    public class DALLink : FYJ.Framework.Core.DAL.DALAbstract<Blogs.Entity.blog_tb_link>, IDALLink
    {
        protected override string PrimaryKey
        {
            get { return "linkID"; }
        }

        public ICollection<Entity.blog_tb_link> Query(string blogID)
        {
            string sql = "select * from blog_tb_link where blogID=@blogID and menuIsDisabled=0 order by linkOrder DESC";
            DataTable dt = DbInstance.GetDataTable(sql);
            return ObjectHelper.DataTableToModel<blog_tb_link>(dt);
        }
    }
}

