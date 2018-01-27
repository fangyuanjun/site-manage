using Blogs.Entity;
using FYJ;
using FYJ.Common;
using FYJ.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Blogs.UI.Manage.Controllers
{
    public class TalkController : Controller
    {
        //
        // GET: /Talk/

        public ActionResult Index()
        {
            return View("~/Views/" + Utility.Version + "/Talk/Index.cshtml");
        }

        public JsonResult GetList(GridPager pager, string queryStr)
        {
            pager.CurrentPage = Convert.ToInt32(Request["page"]);
            pager.PageSize = Convert.ToInt32(Request["rows"]);
            pager.OrderColumn = Request["sort"];
            pager.Order = Request["order"];
            pager.Where = "userID=@userID and IsTemp=0";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("userID", UserInfo.UserID);
            pager.Parameters = dic;
            IEnumerable<Blogs.Entity.blog_tb_Talk> list = Utility.TalkBll.GetList(ref pager);

            var json = new
            {
                total = pager.TotalRows,
                rows = (from r in list
                        select new
                        {
                            ID = r.ID,
                            r.TalkText,
                            r.Pic,
                            r.IsDisabled,
                            TalkDatetime = r.TalkDatetime.ToString("yyyy-MM-dd HH:mm:ss"),
                            ADD_DATE = r.ADD_DATE.ToString("yyyy-MM-dd HH:mm:ss"),
                            UPDATE_DATE = r.UPDATE_DATE.ToString("yyyy-MM-dd HH:mm:ss")
                        }).ToArray()
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(string id)
        {
            blog_tb_Talk entity = new blog_tb_Talk { TalkDatetime=DateTime.Now, ADD_DATE = DateTime.Now, UPDATE_DATE = DateTime.Now };
            if ((!String.IsNullOrEmpty(id)) && id.ToString() != "0")
            {
                entity = Utility.TalkBll.GetEntity(id);
                entity.UPDATE_DATE = DateTime.Now;
            }
            else
            {
                entity.ID = Guid.NewGuid().ToString("N");
            }
            ViewBag.ObjectID = entity.ID;
            return View("~/Views/" + Utility.Version + "/Talk/Edit.cshtml",entity);
        }


        [HttpPost]
        public JsonResult Edit(string id, FormCollection collection)
        {
            blog_tb_Talk entity = new blog_tb_Talk();
            if (String.IsNullOrEmpty(id) || id == "0")
            {
                UpdateModel(entity);
                entity.ID = Request["ObjectID"];
                entity.UserID = UserInfo.UserID;
                entity.ADD_DATE = DateTime.Now;
                entity.UPDATE_DATE = DateTime.Now;
            }
            else
            {
                entity = Utility.TalkBll.GetEntity(id);
                UpdateModel(entity);
                entity.UPDATE_DATE = DateTime.Now;
            }

            if (String.IsNullOrEmpty(entity.TalkContent))
            {
                Match m = Regex.Match(entity.TalkContent, "src=\"(http://static.kecq.com.*?)\"");
                if (m.Success)
                {
                    entity.Pic = m.Groups[1].Value;
                }
            }

            string talkText = HttpHelper.HtmlFilter(entity.TalkContent);
            entity.TalkText = talkText.Substring(0, Math.Min(300, talkText.Length));

            if (String.IsNullOrEmpty(id) || id == "0")
            {
                Utility.TalkBll.Insert(entity);
            }
            else
            {
                Utility.TalkBll.Update(entity);
            }
            return Json(new { code = 1, message = "操作成功" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveTempContent()
        {
            string talkContent = Request["talkContent"];
            Utility.TalkBll.SaveTemp(UserInfo.UserID, talkContent);
            return Json(new { code = 1, message = DateTime.Now.ToString("HH:mm:ss") + "草稿保存成功" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(FormCollection collection)
        {
            Utility.TalkBll.Delete(collection["ids"]);
            return Json(new { code = 1, message = "删除成功" }, JsonRequestBehavior.AllowGet);
        }
    }
}
