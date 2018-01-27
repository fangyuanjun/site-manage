using BlogFiles.Upload;
using FYJ;
using FYJ.Common;
using FYJ.Framework.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Blogs.UI.Manage
{
    public class PhotoUpload : UploadAbstract
    {

        private readonly FYJ.Data.IDbHelper db = IocFactory<FYJ.Data.IDbFactory>.Instance.GetDbInstance("Blogs");

        public override string UploadResultUrl
        {
            get
            {
                string appName = HttpContext.Current.Request["appName"];
                string albumName = HttpContext.Current.Request["albumName"];
                return UploadResultRootUrl.TrimEnd('/') + "/" + appName + "/album/" + DateTime.Now.ToString("yyyy") + "/" + albumName;
            }
        }


        public override string UploadLocalPath
        {
            get
            {
                string appName = HttpContext.Current.Request["appName"];
                string albumName = HttpContext.Current.Request["albumName"];
                return Path.Combine(UploadLocalRootPath, appName, "album", DateTime.Now.ToString("yyyy"), albumName);
            }
        }

        private int maxWidth = 1920;
        //限制最大宽1920
        public override int MaxWidth
        {
            get
            {
                return maxWidth;
            }
            set
            {
                maxWidth = value;
            }
        }

        private int maxHeight = 1280;
        //限制最大高1280
        public override int MaxHeight
        {
            get
            {
                return maxHeight;
            }
            set
            {
                maxHeight = value;
            }
        }

        public override UploadResult Upload(UploadInfo info)
        {
            //相册ID
            string albumID = HttpContext.Current.Request["albumID"];
            var album = Utility.AlbumBll.GetEntity(albumID);
            if (album == null)
            {
                throw new CustomException("相册不存在");
            }
            if (album.UserID != UserInfo.UserID)
            {
                throw new CustomException("你不是该相册的所有者");
            }

            UploadResult reslut = base.Upload(info);

            DataTable dt = Utility.PhotoBll.GetPhotoBySha1(reslut.Sha1, UserInfo.UserID);
            if (dt.Rows.Count > 0)
            {

            }
            else
            {
                Blogs.Entity.blog_tb_Photo entity = new Entity.blog_tb_Photo();
                entity.ID = Guid.NewGuid().ToString("N");
                entity.UserID = UserInfo.UserID;
                entity.ADD_DATE = DateTime.Now;
                entity.UPDATE_DATE = DateTime.Now;
                entity.FileName = reslut.FileName;
                entity.Size = (int)reslut.Size;
                entity.Width = reslut.Width;
                entity.Height = reslut.Height;
                entity.Url = reslut.Url;
                entity.Sha1 = reslut.Sha1;
                entity.ThumbUrl = reslut.ThumbUrl;
                entity.FromUrl = info.FromUrl;
                entity.AlbumID = albumID;
                entity.Exif = reslut.Exif;

                Blogs.Entity.blog_tb_Exif exif = null;
                if (reslut.CurrentExif != null)
                {
                    exif = new Entity.blog_tb_Exif();
                    exif.ADD_DATE = DateTime.Now;
                    exif.Aperture = reslut.CurrentExif.Aperture;
                    exif.Balance = reslut.CurrentExif.Balance;
                    exif.Camera = reslut.CurrentExif.Camera;
                    exif.Exposure = reslut.CurrentExif.Exposure;
                    exif.Flashlight = reslut.CurrentExif.Flashlight;
                    exif.Focus = reslut.CurrentExif.Focus;
                    exif.ID = Guid.NewGuid().ToString("N");
                    exif.ISO = reslut.CurrentExif.ISO;
                    exif.Lens = reslut.CurrentExif.Lens;
                    exif.Metering = reslut.CurrentExif.Metering;
                    exif.Mode = reslut.CurrentExif.Mode;
                    exif.PhotoID = entity.ID;
                    exif.ShotDate = reslut.CurrentExif.ShotDate;
                    exif.Shutter = reslut.CurrentExif.Shutter;
                    exif.UPDATE_DATE = DateTime.Now;
                }

                Utility.PhotoBll.Add(entity, exif);
            }

            return reslut;
        }

        public override void ProcessRequest(HttpContext context)
        {
            try
            {
                if (context.Request.Files.Count == 0)
                {
                    return;
                }

                context.Response.ContentType = "text/plain";
                context.Response.Charset = "utf-8";

                List<UploadResult> resultList = new List<UploadResult>();
                for (int i = 0; i < context.Request.Files.Count; i++)
                {
                    HttpPostedFile postedFile = context.Request.Files[i];
                    UploadInfo info = new UploadInfo();
                    info.InputStream = postedFile.InputStream;
                    info.FileName = postedFile.FileName;
                    UploadResult result = Upload(info);

                    resultList.Add(result);
                }

                UploadResult lastResult = resultList.Last();

                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("error", 0);
                dic.Add("url", lastResult.Url);
                dic.Add("fileID", lastResult.ID);
                dic.Add("relationID", lastResult.RelationID);
                dic.Add("code", 1);
                dic.Add("message", "上传成功");

                string json = JsonHelper.ToJson(dic);
                if (HttpContext.Current.Request["mod"] == "kindeditor")
                {
                    json = "{\"error\":0,\"url\":\"" + lastResult.Url + "\"}";
                }
                context.Response.Write(json);
            }
            catch (Exception ex)
            {
                if (context.Request["mod"] == "webuploader")
                {
                    throw new Exception("服务器处理上传出错");
                }
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("error", 1);
                dic.Add("code", -1);
                dic.Add("message", "上传失败" + ex.Message);

                string json = JsonHelper.ToJson(dic);
                context.Response.Write(json);
            }

            context.Response.End();
        }
    }
}