using Blogs.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using FYJ;

namespace Blogs.UI.Manage.Controllers
{
    public class LinkController : Controller
    {

        public ActionResult Index()
        {
            return View("~/Views/" + Utility.Version + "/Link/Index.cshtml");
        }

        public JsonResult GetList(GridPager pager, string queryStr)
        {
            if (!String.IsNullOrEmpty(Request["page"]))
            {
                pager.CurrentPage = Convert.ToInt32(Request["page"]);
            }

            if (!String.IsNullOrEmpty(Request["rows"]))
            {
                pager.PageSize = Convert.ToInt32(Request["rows"]);
            }

            if (!String.IsNullOrEmpty(Request["limit"]))
            {
                pager.PageSize = Convert.ToInt32(Request["limit"]);
            }

            if (!String.IsNullOrEmpty(Request["offset"]))
            {
                pager.Offset = Convert.ToInt32(Request["offset"]);
            }

            pager.OrderColumn = Request["sort"];
            pager.Order = Request["order"];

            //如果不分页取消下面的注释
            //pager.CurrentPage = 1;
            //pager.PageSize = 10000;

            string where = " blogID=@blogID";
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@blogID", UserInfo.BlogID);
            if (!String.IsNullOrEmpty(Request["search"]))
            {
                where += " and LOWER(linkName)  like @search ";
                parameters.Add("@search", "%" + Request["search"].Trim() + "%");
            }

            pager.Where = where;
            pager.Parameters = parameters;

            IEnumerable<blog_tb_link> list = Utility.LinkBll.GetList(ref pager);
            var json = new
            {
                total = pager.TotalRows,
                rows = list
            };

            return new JsonNetResult(json, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(string id)
        {
            blog_tb_link model = new blog_tb_link { ADD_DATE = DateTime.Now, UPDATE_DATE = DateTime.Now };
            if ((!String.IsNullOrEmpty(id)) && id.ToString() != "0")
            {
                model = Utility.LinkBll.GetEntity(id);
                model.UPDATE_DATE = DateTime.Now;
            }

            Models.Link m = ObjectHelper.CloneProperties<Models.Link>(model);
            return View("~/Views/" + Utility.Version + "/Link/Edit.cshtml", m);
        }

        public ActionResult EditModel(string id)
        {
            blog_tb_link model = new blog_tb_link { ADD_DATE = DateTime.Now, UPDATE_DATE = DateTime.Now };
            if ((!String.IsNullOrEmpty(id)) && id.ToString() != "0")
            {
                model = Utility.LinkBll.GetEntity(id);
                model.UPDATE_DATE = DateTime.Now;
            }

            return new JsonNetResult(new { code = 1, message = "ok", data = model });
        }


        [HttpPost]

        public JsonResult Edit(blog_tb_link model)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(model.linkID + "") || (model.linkID + "") == "0")
                {
                    UpdateModel(model);
                    model.linkID = Utility.LinkBll.NewID().ToString();
                    model.blogID = Convert.ToInt32(UserInfo.BlogID);
                    model.ADD_DATE = DateTime.Now;
                    model.UPDATE_DATE = DateTime.Now;
                    Utility.LinkBll.Insert(model);
                }
                else
                {
                    model = Utility.LinkBll.GetEntity(model.linkID + "");
                    UpdateModel(model);
                    model.UPDATE_DATE = DateTime.Now;
                    Utility.LinkBll.Update(model);
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
            Utility.LinkBll.Delete(collection["ids"]);
            return Json(new { code = 1, message = "删除成功" }, JsonRequestBehavior.AllowGet);
        }

    }
}