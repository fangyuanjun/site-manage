using Blogs.Entity;
using Blogs.UI.Manage.Models;
using FYJ.Common;
using FYJ.Framework.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using FYJ;

namespace Blogs.UI.Manage.Controllers
{

    public class CateController : Controller
    {
        //
        // GET: /Cate/
        public ActionResult Index()
        {
            return View("~/Views/" + Utility.Version + "/Cate/Index.cshtml");
        }

        public JsonResult GetList()
        {
            List<blog_tb_category> list = Utility.CategoryBll.GetAllList(UserInfo.BlogID);
            return new JsonNetResult(list, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(string id)
        {
            Category m = new Category { categoryID = Convert.ToInt32(id), ADD_DATE = DateTime.Now, UPDATE_DATE = DateTime.Now };
            if ((!String.IsNullOrEmpty(id)) && id.ToString() != "0")
            {
                blog_tb_category model = Utility.CategoryBll.GetEntity(id);
                m = ObjectHelper.CloneProperties<Category>(model);
            }
            return View("~/Views/" + Utility.Version + "/Cate/Edit.cshtml", m);
        }

        public ActionResult EditModel(string id)
        {
            List<blog_tb_category> list = Utility.CategoryBll.GetTreeData(UserInfo.BlogID);
            blog_tb_category insertmodel = new blog_tb_category();
            insertmodel.categoryID = 0;
            insertmodel.TextWithTreeSpace = "--请选择--";
            list.Insert(0, insertmodel);

            blog_tb_category model = new blog_tb_category { ADD_DATE = DateTime.Now, UPDATE_DATE = DateTime.Now };
            if ((!String.IsNullOrEmpty(id)) && id.ToString() != "0")
            {
                model = Utility.CategoryBll.GetEntity(id);
                model.UPDATE_DATE = DateTime.Now;
            }

            return new JsonNetResult(new { code = 1, message = "ok", data = model, catelist = list });
        }

        [HttpPost]

        public JsonResult Edit(blog_tb_category model)
        {
            if (ModelState.IsValid)
            {
                if (model.categoryID == 0)
                {
                    UpdateModel(model);
                    model.categoryID = Convert.ToInt32(Utility.CategoryBll.NewID());
                    model.blogID = Convert.ToInt32(UserInfo.BlogID);
                    model.ADD_DATE = DateTime.Now;
                    model.UPDATE_DATE = DateTime.Now;
                    Utility.CategoryBll.Insert(model);
                }
                else
                {
                    model = Utility.CategoryBll.GetEntity(model.categoryID + "");
                    UpdateModel(model);
                    model.UPDATE_DATE = DateTime.Now;
                    Utility.CategoryBll.Update(model);
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
            Utility.CategoryBll.Delete(collection["ids"]);
            return Json(new { code = 1, message = "删除成功" }, JsonRequestBehavior.AllowGet);
        }
    }
}