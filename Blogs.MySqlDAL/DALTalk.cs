using Blogs.Entity;
using Blogs.IDAL;
using FYJ;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Blogs.DAL
{
    public class DALTalk : FYJ.Framework.Core.DAL.DALAbstract<Blogs.Entity.blog_tb_Talk>, IDALTalk
    {
        protected override string PrimaryKey
        {
            get { return "ID"; }
        }

        public string ReadTemp(string userID)
        {
            string sql = "select    TalkContent from blog_tb_Talk where UserID=@UserID and IsTemp=1 limit 0,1";
            return DbInstance.GetString(sql,DbInstance.CreateParameter("@UserID",userID));
        }

        public int SaveTemp(string userID, string content)
        {
            if(!String.IsNullOrWhiteSpace(content))
            {
                string sql = "select   ID,TalkContent from blog_tb_Talk where UserID=@UserID and IsTemp=1 limit 0,1";
                DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@UserID", userID));
                if (dt.Rows.Count > 0)
                {
                    blog_tb_Talk entity = GetEntity(dt.Rows[0]["ID"].ToString());
                    entity.TalkContent = content;
                    entity.TalkDatetime = DateTime.Now;
                    entity.ADD_DATE = DateTime.Now;
                    entity.UPDATE_DATE = DateTime.Now;
                    Update(entity);
                }
                else
                {
                    blog_tb_Talk entity = new blog_tb_Talk();
                    entity.UserID = userID;
                    entity.ID = Guid.NewGuid().ToString("N");
                    entity.TalkContent = content;
                    entity.TalkDatetime = DateTime.Now;
                    entity.ADD_DATE = DateTime.Now;
                    entity.UPDATE_DATE = DateTime.Now;
                    entity.IsTemp = true;
                    Insert(entity);
                }
            }
           
            return 1;
        }


        public List<blog_tb_Talk> Query(string userID)
        {
            string sql = "select   *  from blog_tb_Talk where UserID=@UserID and IsTemp=0 and IsDisabled=0 order by TalkDatetime DESC limit 0,100";
            DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@UserID", userID));
            return ObjectHelper.DataTableToModel<blog_tb_Talk>(dt);
        }
    }
}
