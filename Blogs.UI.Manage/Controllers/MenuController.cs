using Blogs.Entity;
using FYJ.Common;
using FYJ.Framework.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using Menu = Blogs.UI.Manage.Models.Menu;
using FYJ;

namespace Blogs.UI.Manage.Controllers
{
    public class MenuController : Controller
    {
        //
        // GET: /Menu/
        public ActionResult Index()
        {
            return View("~/Views/" + Utility.Version + "/Menu/Index.cshtml");
        }

        public JsonResult GetList()
        {
            List<blog_tb_menu> list = Utility.MenuBll.GetAllList(UserInfo.BlogID);

            return new JsonNetResult(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(string id)
        {
            Menu m = new Menu { ADD_DATE = DateTime.Now, UPDATE_DATE = DateTime.Now };
            if ((!String.IsNullOrEmpty(id)) && id.ToString() != "0")
            {
                blog_tb_menu model = Utility.MenuBll.GetEntity(id);
                m = ObjectHelper.CloneProperties<Menu>(model);
            }

            return View("~/Views/" + Utility.Version + "/Menu/Edit.cshtml", m);
        }

        public ActionResult EditModel(string id)
        {
            List<blog_tb_menu> list = Utility.MenuBll.GetTreeData(UserInfo.BlogID);
            blog_tb_menu insertmodel = new blog_tb_menu();
            insertmodel.menuID = "";
            insertmodel.TextWithTreeSpace = "--请选择--";
            list.Insert(0, insertmodel);

            blog_tb_menu model = new blog_tb_menu { ADD_DATE = DateTime.Now, UPDATE_DATE = DateTime.Now };
            if ((!String.IsNullOrEmpty(id)) && id.ToString() != "0")
            {
                model = Utility.MenuBll.GetEntity(id);
                model.UPDATE_DATE = DateTime.Now;
            }

            List<object> targetList = new List<object> {
            new {Text="当前页",Value="_self"},
            new {Text="新窗口",Value="_blank"}
            };

            return new JsonNetResult(new { code = 1, message = "ok", data = model, catelist = list, targetList = targetList });
        }

        [HttpPost]

        public JsonResult Edit(blog_tb_menu model)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(model.menuID) || model.menuID == "0")
                {
                    UpdateModel(model);
                    model.menuID = Utility.MenuBll.NewID().ToString();
                    model.blogID = Convert.ToInt32(UserInfo.BlogID);
                    model.ADD_DATE = DateTime.Now;
                    model.UPDATE_DATE = DateTime.Now;
                    model.menuTarget = Request["menuTarget"];
                    Utility.MenuBll.Insert(model);
                }
                else
                {
                    model = Utility.MenuBll.GetEntity(model.menuID);
                    UpdateModel(model);
                    model.UPDATE_DATE = DateTime.Now;
                    model.menuTarget = Request["menuTarget"];
                    Utility.MenuBll.Update(model);
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
            Utility.MenuBll.Delete(collection["ids"]);
            return Json(new { code = 1, message = "删除成功" }, JsonRequestBehavior.AllowGet);
        }
    }
}