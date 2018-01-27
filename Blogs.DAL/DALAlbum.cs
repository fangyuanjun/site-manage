using Blogs.IDAL;
using FYJ.Common;
using FYJ.Framework.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Blogs.DAL
{
    public class DALAlbum : FYJ.Framework.Core.DAL.DALAbstract<Blogs.Entity.blog_tb_Album>, IDALAlbum
    {
        protected override string PrimaryKey
        {
            get { return "ID"; }
        }

        public override object NewID()
        {
            return new Random().Next(10000000, 99999999) + "";
        }

        public System.Data.DataTable QueryAlbum(string userID)
        {
            string sql = "select * from blog_view_Album where userID=@userID";
            return DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@userID", userID));
        }


        public System.Data.DataTable QueryAlbum(ref GridPager pager)
        {
            int count = 0;
            List<IDataParameter> plist = null;
            if (pager.Parameters != null && pager.Parameters.Count > 0)
            {
                plist = new List<IDataParameter>();
                foreach (string key in pager.Parameters.Keys)
                {
                    plist.Add(DbInstance.CreateParameter("@" + key.TrimStart('@'), pager.Parameters[key]));
                }
            }
            DataTable dt = DbInstance.GetDataTable(out count, "blog_view_Album", pager.OrderColumn + " " + pager.Order, plist, "*", pager.Where, pager.CurrentPage, pager.PageSize);
            pager.TotalRows = count;

            return dt;
        }

        public override int Delete(string id)
        {
            string tmp = "";
            foreach (string s in id.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string t = s.Trim('\'');
                tmp += "'" + t + "',";
            }
            tmp = tmp.TrimEnd(',');
            string sql = "select count(*) from blog_tb_Photo where AlbumID in(" + tmp + ")";
            if (DbInstance.GetInt(sql) > 0)
            {
                throw new CustomException("相册中有照片，不能删除");
            }

            return base.Delete(id);
        }

        public List<string> GetCoverPhotos(string albumID)
        {
            List<string> list = new List<string>();
            Entity.blog_tb_Album entity = GetEntity(albumID);
            if (!String.IsNullOrEmpty(entity.CoverUrl))
            {
                list.Add(entity.CoverUrl);
            }

            string sql = "select top 6  ThumbUrl from blog_tb_Photo where AlbumID=@AlbumID and ISNULL(ThumbUrl,'')<>'' order by ADD_DATE desc";
            DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@AlbumID", albumID));
            foreach (DataRow dr in dt.Rows)
            {
                if (!list.Contains(dr[0].ToString()))
                {
                    list.Add(dr[0].ToString());
                }
            }

            return list;
        }
    }
}
