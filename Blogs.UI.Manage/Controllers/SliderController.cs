using Blogs.Entity;
using FYJ;
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
    public class SliderController : Controller
    {
        //
        // GET: /Slider/

        public ActionResult Index()
        {
            return View("~/Views/" + Utility.Version + "/Slider/Index.cshtml");
        }

        public JsonResult GetList(GridPager pager, string queryStr)
        {
            pager.CurrentPage = Convert.ToInt32(Request["page"]);
            pager.PageSize = Convert.ToInt32(Request["rows"]);
            pager.OrderColumn = Request["sort"];
            pager.Order = Request["order"];
            pager.Where = "blogID=@blogID";

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("blogID", UserInfo.BlogID);
            pager.Parameters = dic;
            IEnumerable<blog_tb_slider> list = Utility.SliderBll.GetList(ref pager);

            var json = new
            {
                total = pager.TotalRows,
                rows = (from r in list
                        select new
                        {
                            ID = r.ID,
                            Url = r.Url,
                            Pic = r.Pic,
                            r.OrderWeight,
                            ADD_DATE = Convert.ToDateTime(r.ADD_DATE).ToString("yyyy-MM-dd HH:mm:ss"),
                            UPDATE_DATE = Convert.ToDateTime(r.UPDATE_DATE).ToString("yyyy-MM-dd HH:mm:ss")
                        }).ToArray()
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(string id)
        {
            blog_tb_slider model = new blog_tb_slider { ADD_DATE = DateTime.Now, UPDATE_DATE = DateTime.Now };
            if ((!String.IsNullOrEmpty(id)) && id.ToString() != "0")
            {
                model = Utility.SliderBll.GetEntity(id);
                model.BlogID = UserInfo.BlogID;
                model.UPDATE_DATE = DateTime.Now;
            }
            else
            {
                model.ID = Guid.NewGuid().ToString("N");
            }
         
            if(String.IsNullOrEmpty(model.Pic))
            {
               model.Pic="http://static.kecq.com/images/common/nopic.jpg";
            }

            ViewBag.ObjectID = model.ID;
            return View("~/Views/" + Utility.Version + "/Slider/Edit.cshtml",model);
        }


        [HttpPost]

        public JsonResult Edit(string id, FormCollection collection)
        {
            blog_tb_slider model = new blog_tb_slider();
            if (String.IsNullOrEmpty(id) || id == "0")
            {
                UpdateModel(model);
                model.ID = Request["ObjectID"];
                model.Pic = Request["lastImageName_mainPic"];
                model.BlogID = UserInfo.BlogID;
                model.ADD_DATE = DateTime.Now;
                model.UPDATE_DATE = DateTime.Now;
                Utility.SliderBll.Insert(model);
            }
            else
            {
                model = Utility.SliderBll.GetEntity(id);
                UpdateModel(model);
                model.Pic = Request["lastImageName_mainPic"];
                model.UPDATE_DATE = DateTime.Now;
                Utility.SliderBll.Update(model);
            }

            return Json(new { code = 1, message = "操作成功" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(FormCollection collection)
        {
            Utility.SliderBll.Delete(collection["ids"]);
            return Json(new { code = 1, message = "删除成功" }, JsonRequestBehavior.AllowGet);
        }

    }
}
