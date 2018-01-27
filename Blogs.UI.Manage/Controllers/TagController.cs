using Blogs.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using FYJ;

namespace Blogs.UI.Manage.Controllers
{
    public class TagController : Controller
    {

        public ActionResult Index()
        {
            return View("~/Views/" + Utility.Version + "/Tag/Index.cshtml");
        }

        public JsonResult GetList()
        {
            IEnumerable<blog_tb_tag> list = Utility.TagBll.GetTags(UserInfo.BlogID);
            var json = new
            {
                total = list.Count(),
                rows = list
            };

            return new JsonNetResult(json, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(string id)
        {
            blog_tb_tag model = new blog_tb_tag { ADD_DATE = DateTime.Now, UPDATE_DATE = DateTime.Now };
            if ((!String.IsNullOrEmpty(id)) && id.ToString() != "0")
            {
                model = Utility.TagBll.GetEntity(id);
                model.UPDATE_DATE = DateTime.Now;
            }

            Models.Tag tag = ObjectHelper.CloneProperties<Models.Tag>(model);
            return View("~/Views/" + Utility.Version + "/Tag/Edit.cshtml", tag);
        }

        public ActionResult EditModel(string id)
        {
            List<blog_tb_category> list = Utility.CategoryBll.GetTreeData(UserInfo.BlogID);
            blog_tb_category insertmodel = new blog_tb_category();
            insertmodel.categoryID = 0;
            insertmodel.TextWithTreeSpace = "--请选择--";
            list.Insert(0, insertmodel);

            blog_tb_tag model = new blog_tb_tag { ADD_DATE = DateTime.Now, UPDATE_DATE = DateTime.Now };
            if ((!String.IsNullOrEmpty(id)) && id.ToString() != "0")
            {
                model = Utility.TagBll.GetEntity(id);
                model.UPDATE_DATE = DateTime.Now;
            }

            return new JsonNetResult(new { code = 1, message = "ok", data = model, catelist = list });
        }


        [HttpPost]

        public JsonResult Edit(blog_tb_tag model)
        {
            if (ModelState.IsValid)
            {
                if (model.tagID == 0)
                {
                    UpdateModel(model);
                    model.tagID = Convert.ToInt32(Utility.TagBll.NewID());
                    model.blogID = Convert.ToInt32(UserInfo.BlogID);
                    model.ADD_DATE = DateTime.Now;
                    model.UPDATE_DATE = DateTime.Now;
                    Utility.TagBll.Insert(model);
                }
                else
                {
                    model = Utility.TagBll.GetEntity(model.tagID + "");
                    UpdateModel(model);
                    model.UPDATE_DATE = DateTime.Now;
                    Utility.TagBll.Update(model);
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
            Utility.TagBll.Delete(collection["ids"]);
            return Json(new { code = 1, message = "删除成功" }, JsonRequestBehavior.AllowGet);
        }

    }
}