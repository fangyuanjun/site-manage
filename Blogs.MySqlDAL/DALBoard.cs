using Blogs.Entity;
using Blogs.IDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Blogs.DAL
{
    public class DALBoard : FYJ.Framework.Core.DAL.DALAbstract<Blogs.Entity.blog_tb_Board>, IDALBoard
    {
        protected override string PrimaryKey
        {
            get { return "ID"; }
        }

        public IList<blog_tb_Board> Query(int state)
        {
            string sql = "select * from blog_tb_Board where 1=1";
            if(state!=-1)
            {
                sql += " and state="+state;
            }

            DataTable dt = DbInstance.GetDataTable(sql);
            return FYJ.ObjectHelper.DataTableToModel<blog_tb_Board>(dt);
        }
    }
}
