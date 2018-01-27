using Blogs.Entity;
using Blogs.IDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Blogs.DAL
{
    public class DALPhoto : FYJ.Framework.Core.DAL.DALAbstract<Blogs.Entity.blog_tb_Photo>, IDALPhoto
    {
        protected override string PrimaryKey
        {
            get { return "ID"; }
        }

        public IList<blog_tb_Photo> GetPhotoBySha1(string sha1, string userID)
        {
            DataTable dt = DbInstance.GetDataTable("select * from blog_tb_Photo where Sha1=@Sha1 and  UserID=@UserID "
               , DbInstance.CreateParameter("@Sha1", sha1)
               , DbInstance.CreateParameter("@UserID", userID));

            return FYJ.ObjectHelper.DataTableToModel<blog_tb_Photo>(dt);
        }

        public int Add(blog_tb_Photo entity, blog_tb_Exif exif)
        {
            int result = base.Insert(entity);
            if (exif != null)
            {
                result += FYJ.Data.Entity.EntityHelper<object>.Insert(exif, "blog_tb_Exif", "ID", true, DbInstance);
            }

            return result;
        }


        public IList<blog_tb_Photo> Query(string albumID)
        {
            string sql = "select * from blog_tb_Photo where AlbumID=@AlbumID and IsDelete=0";
            DataTable dt = DbInstance.GetDataTable(sql,DbInstance.CreateParameter("@AlbumID",albumID));

            return FYJ.ObjectHelper.DataTableToModel<blog_tb_Photo>(dt);
        }

        public override int Delete(string id)
        {
            return base.Delete(id);
        }
    }
}
