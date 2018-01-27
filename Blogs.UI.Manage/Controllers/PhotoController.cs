using Blogs.Entity;
using Blogs.UI.Manage.Models;
using FYJ;
using FYJ.Common;
using FYJ.Framework.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blogs.UI.Manage.Controllers
{
    public class PhotoController : Controller
    {
        //
        // GET: /Photo/
        public ActionResult Index()
        {
            IList<SelectModel> list = Utility.AlbumBll.QueryAlbumSelect(UserInfo.UserID);
            ViewData["albumSelect"] = new SelectList(list, "Value", "Text");
            return View("~/Views/" + Utility.Version + "/Photo/Index.cshtml");
        }

        public JsonResult GetList(GridPager pager, string queryStr)
        {
            pager.CurrentPage = Convert.ToInt32(Request["page"]);
            pager.PageSize = Convert.ToInt32(Request["rows"]);
            pager.OrderColumn = Request["sort"];
            pager.Order = Request["order"];


            string where = " IsDelete=0 and  userID=@userID";
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userID", UserInfo.UserID);
            if (!String.IsNullOrWhiteSpace(Request["ddlAlbumSelect"]))
            {
                where += " and AlbumID=@AlbumID";
                parameters.Add("@AlbumID", Request["ddlAlbumSelect"]);
            }

            pager.Where = where;

            pager.Parameters = parameters;
            IEnumerable<Blogs.Entity.blog_tb_Photo> list = Utility.PhotoBll.GetList(ref pager);

            var json = new
            {
                total = pager.TotalRows,
                rows = (from r in list
                        select new
                        {
                            r.ID,
                            r.FileName,
                            r.Display,
                            r.Permission,
                            r.AlbumID,
                            r.Author,
                            r.Exif,
                            r.FromUrl,
                            r.Height,
                            r.IsDelete,
                            r.IsToAliyun,
                            r.IsToLocal,
                            r.IsToRemote,
                            r.Mark,
                            r.Url,
                            r.ThumbUrl,
                            r.Width,
                            r.Sha1,
                            Size=ToSizeString(r.Size),
                            cc = 0,
                            ADD_DATE = r.ADD_DATE.ToString("yyyy-MM-dd HH:mm:ss"),
                            UPDATE_DATE = r.UPDATE_DATE.ToString("yyyy-MM-dd HH:mm:ss")
                        }).ToArray()
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(string id)
        {
            IList<SelectModel> list = Utility.AlbumBll.QueryAlbumSelect(UserInfo.UserID);
            ViewData["albumSelect"] = new SelectList(list, "Value", "Text");

            blog_tb_Photo model = new blog_tb_Photo { ADD_DATE = DateTime.Now, UPDATE_DATE = DateTime.Now };
            if ((!String.IsNullOrEmpty(id)) && id.ToString() != "0")
            {
                model = Utility.PhotoBll.GetEntity(id);
                model.UserID = UserInfo.UserID;
                model.UPDATE_DATE = DateTime.Now;
            }

            return View("~/Views/" + Utility.Version + "/Photo/Edit.cshtml",model);
        }


        public ActionResult PUpload()
        {
            Entity.blog_tb_Photo entity = new blog_tb_Photo();
            entity.ID = Guid.NewGuid().ToString("N");
            entity.AlbumID = Request["albumID"];
            entity.UserID = UserInfo.UserID;
            Entity.blog_tb_Album album = Utility.AlbumBll.GetEntity(entity.AlbumID);
            ViewBag.AlbumName = album.Name;
            return View("~/Views/" + Utility.Version + "/Photo/PUpload.cshtml", entity);
        }

        public ActionResult PList()
        {
            PhotoListViewModel model = new PhotoListViewModel();
            string albumID = Request["albumID"];
            blog_tb_Album album = Utility.AlbumBll.GetEntity(albumID);
            if(album==null)
            {
                throw new CustomException("相册不存在");
            }
            if(album.UserID!=UserInfo.UserID)
            {
                throw new CustomException("你没有权限管理该相册");
            }
            model.Title = album.Display + "-查看照片";
            model.PhotoCollection = Utility.PhotoBll.Query(albumID);
           
            return View("~/Views/" + Utility.Version + "/Photo/PList.cshtml", model);
        }

        [HttpPost]

        public JsonResult Edit(string id, FormCollection collection)
        {
            blog_tb_Photo model = new blog_tb_Photo();
            if (String.IsNullOrEmpty(id) || id == "0")
            {
                UpdateModel(model);
                model.ID = Guid.NewGuid().ToString("N");
                model.UserID = UserInfo.UserID;
                model.ADD_DATE = DateTime.Now;
                model.UPDATE_DATE = DateTime.Now;
                Utility.PhotoBll.Insert(model);
            }
            else
            {
                model = Utility.PhotoBll.GetEntity(id);
                UpdateModel(model);
                model.UPDATE_DATE = DateTime.Now;
                Utility.PhotoBll.Update(model);
            }

            return Json(new { code = 1, message = "操作成功" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(FormCollection collection)
        {
            Utility.PhotoBll.Delete(collection["ids"]);
            return Json(new { code = 1, message = "删除成功" }, JsonRequestBehavior.AllowGet);
        }


        private string ToSizeString(long size)
        {
            if (size > 0 && size < 1024)
            {
                return size + "B";
            }

            if (size >= 1024 && size < 1048576)
            {
                return Math.Round(size / 1024.0, 2) + "KB";
            }

            if (size >= 1048576 && size < 1048576 * 1024)
            {
                return Math.Round(size / 1048576.0, 2) + "MB";
            }

            if (size >= 1073741824)
            {
                return Math.Round(size / 1073741824.0, 2) + "GB";
            }

            return size + "";
        }
    }
}
