using Blogs.Entity;
using Blogs.UI.Manage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using FYJ;

namespace Blogs.UI.Manage.Controllers
{
    public class TopicController : Controller
    {
        //
        // GET: /Topic/
        public ActionResult Index()
        {
            return View("~/Views/" + Utility.Version + "/Topic/Index.cshtml");
        }

        public JsonResult GetList(GridPager pager, string queryStr)
        {
            IEnumerable<Blogs.Entity.blog_tb_topic> list = Utility.TopicBll.GetTopic(UserInfo.BlogID);
            var json = new
            {
                total = list.Count(),
                rows = list
            };

            return new JsonNetResult(json, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(string id)
        {
            blog_tb_topic model = new blog_tb_topic { ADD_DATE = DateTime.Now, UPDATE_DATE = DateTime.Now };
            if ((!String.IsNullOrEmpty(id)) && id.ToString() != "0")
            {
                model = Utility.TopicBll.GetEntity(id);
                model.UPDATE_DATE = DateTime.Now;
            }

            Topic m = ObjectHelper.CloneProperties<Topic>(model);

            return View("~/Views/" + Utility.Version + "/Topic/Edit.cshtml", m);
        }

        public ActionResult EditModel(string id)
        {
            blog_tb_topic model = new blog_tb_topic { ADD_DATE = DateTime.Now, UPDATE_DATE = DateTime.Now };
            if ((!String.IsNullOrEmpty(id)) && id.ToString() != "0")
            {
                model = Utility.TopicBll.GetEntity(id);
                model.UPDATE_DATE = DateTime.Now;
            }

            return new JsonNetResult(new { code = 1, message = "ok", data = model });
        }


        [HttpPost]

        public JsonResult Edit(blog_tb_topic model)
        {
            if (ModelState.IsValid)
            {
                if (model.topicID == 0)
                {
                    UpdateModel(model);
                    model.topicID = Convert.ToInt32(Utility.TopicBll.NewID());
                    model.blogID = Convert.ToInt32(UserInfo.BlogID);
                    model.ADD_DATE = DateTime.Now;
                    model.UPDATE_DATE = DateTime.Now;
                    Utility.TopicBll.Insert(model);
                }
                else
                {
                    model = Utility.TopicBll.GetEntity(model.topicID + "");
                    UpdateModel(model);
                    model.UPDATE_DATE = DateTime.Now;
                    Utility.TopicBll.Update(model);
                }

                return Json(new { code = 1, message = "操作成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = -1, message = "验证失败," + Utility.GetModelValidateError(ModelState) }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Delete(FormCollection collection)
        {
            Utility.TopicBll.Delete(collection["ids"]);
            return Json(new { code = 1, message = "删除成功" }, JsonRequestBehavior.AllowGet);
        }

    }
}