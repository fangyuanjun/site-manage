using Blogs.Entity;
using Blogs.IDAL;
using FYJ;
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

        public IList<SelectModel> QueryAlbumSelect(string userID)
        {
            string sql = @"SELECT   ID Value,Display Text FROM  blog_tb_Album where UserID=@UserID";
            DataTable dt= DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@UserID", userID));
            return FYJ.ObjectHelper.DataTableToModel<SelectModel>(dt);
        }


        public IList<blog_tb_Album> QueryAlbum(ref GridPager pager)
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

            string table = @"
(SELECT
	a.ID,
	a.UserID,
  a. NAME,
	a.Display,
	a.ADD_DATE,
	a.Permission,
	a.UPDATE_DATE,
	IFNULL(c.cc, 0) AS PhotoCount,
	CASE WHEN IFNULL(CoverUrl, '') = '' THEN p.ThumbUrl ELSE CoverUrl END AS CoverUrl
FROM
	blog_tb_Album AS a
LEFT  JOIN (
	SELECT COUNT(*) AS cc,AlbumID
	FROM
		blog_tb_Photo
	GROUP BY
		AlbumID
) AS c ON c.AlbumID = a.ID
LEFT OUTER JOIN (
	SELECT
		AlbumID,	MIN(ThumbUrl) AS ThumbUrl
	FROM
		blog_tb_Photo AS blog_tb_Photo_1
	GROUP BY
		AlbumID
) AS p ON p.AlbumID = a.ID 
) t
";

            DataTable dt = DbInstance.GetDataTable(out count, table, pager.OrderColumn + " " + pager.Order, plist, "*", pager.Where, pager.CurrentPage, pager.PageSize);
            pager.TotalRows = count;

            return FYJ.ObjectHelper.DataTableToModel<blog_tb_Album>(dt);
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

            string sql = "select   ThumbUrl from blog_tb_Photo where AlbumID=@AlbumID and ISNULL(ThumbUrl,'')<>'' order by ADD_DATE desc limit 0,6";
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
