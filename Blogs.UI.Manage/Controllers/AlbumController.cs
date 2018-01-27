using Blogs.Entity;
using FYJ;
using FYJ.Common;
using FYJ.Framework.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blogs.UI.Manage.Controllers
{
    public class AlbumController : Controller
    {
        //
        // GET: /Album/
        public ActionResult Index()
        {
            return View("~/Views/" + Utility.Version + "/Album/Index.cshtml");
        }

        public JsonResult GetList(GridPager pager, string queryStr)
        {
            pager.CurrentPage = Convert.ToInt32(Request["page"]);
            pager.PageSize = Convert.ToInt32(Request["rows"]);
            pager.OrderColumn = Request["sort"];
            pager.Order = Request["order"];
            pager.Where = "userID=@userID";

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("userID", UserInfo.UserID);
            pager.Parameters = dic;
            IList<blog_tb_Album> list = Utility.AlbumBll.QueryAlbum(ref pager);

            var json = new
            {
                total = pager.TotalRows,
                rows = list
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(string id)
        {
            blog_tb_Album model = new blog_tb_Album { ADD_DATE = DateTime.Now, UPDATE_DATE = DateTime.Now };
            if ((!String.IsNullOrEmpty(id)) && id.ToString() != "0")
            {
                model = Utility.AlbumBll.GetEntity(id);
                model.UserID = UserInfo.UserID;
                model.UPDATE_DATE = DateTime.Now;
            }

            return View("~/Views/" + Utility.Version + "/Album/Edit.cshtml",model);
        }


        [HttpPost]

        public JsonResult Edit(string id, FormCollection collection)
        {
            blog_tb_Album model = new blog_tb_Album();
            if (String.IsNullOrEmpty(id) || id == "0")
            {
                UpdateModel(model);
                model.ID = Utility.AlbumBll.NewID().ToString();
                model.UserID = UserInfo.UserID;
                model.ADD_DATE = DateTime.Now;
                model.UPDATE_DATE = DateTime.Now;
                Utility.AlbumBll.Insert(model);
            }
            else
            {
                model = Utility.AlbumBll.GetEntity(id);
                UpdateModel(model);
                model.UPDATE_DATE = DateTime.Now;
                Utility.AlbumBll.Update(model);
            }

            return Json(new { code = 1, message = "操作成功" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(FormCollection collection)
        {
            Utility.AlbumBll.Delete(collection["ids"]);
            return Json(new { code = 1, message = "删除成功" }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SetCover(string id, FormCollection collection)
        {
            string url = collection["url"];
            if (String.IsNullOrEmpty(url))
            {
                return Json(new { code = -1, message = "url参数不能为空" }, JsonRequestBehavior.AllowGet);
            }

            blog_tb_Album album = Utility.AlbumBll.GetEntity(id);
            album.CoverUrl = url;
            album.UPDATE_DATE = DateTime.Now;
            Utility.AlbumBll.Update(album);
            return Json(new { code = 1, message = "操作成功" }, JsonRequestBehavior.AllowGet);
        }
    }
}
