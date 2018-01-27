using Blogs.Entity;
using Blogs.UI.Manage.Models;
using FYJ.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using FYJ;

namespace Blogs.UI.Manage.Controllers
{

    public class BlogManagerController : Controller
    {

        public JsonResult GetList()
        {
            IEnumerable<blog_tb_blog> list = new List<blog_tb_blog> { Utility.BlogBll.GetSingleBlogByUserID(UserInfo.UserID) };

            var json = new
            {
                total = 1,
                rows = (from r in list
                        select new
                        {
                            ID = r.blogID,
                            r.blogID,
                            r.blogName,
                            r.blogTitle,
                            r.blogLogo,
                            r.blogDomain,
                            r.blogIsDisabled,
                            r.themeID,
                            ADD_DATE = r.ADD_DATE.ToString("yyyy-MM-dd HH:mm:ss"),
                            UPDATE_DATE = r.UPDATE_DATE.ToString("yyyy-MM-dd HH:mm:ss")
                        }).ToArray()
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }


        // GET: /BlogManager/
        public ActionResult Index()
        {
            return View("~/Views/" + Utility.Version + "/BlogManager/Index.cshtml");
        }


        public ActionResult Create()
        {
            if (Request.Url.Host.ToLower() != "manage.blogmi.cn")
            {
                return Content("本域名下不允许创建博客,请在<a href='http://manage.blogmi.cn/BlogManager/Create'>这里创建", "text/html");
            }

            blog_tb_blog entity = new blog_tb_blog();
            entity.userID = UserInfo.UserID;
            entity.ADD_DATE = DateTime.Now;
            entity.UPDATE_DATE = DateTime.Now;
            Blog m = ObjectHelper.CloneProperties<Blog>(entity);

            var themeSelect = new List<object> {
                new { themeID="Default"},
                new { themeID="Cnblogs"},
                new { themeID="Muchun"},
                new { themeID="V2"},
                new { themeID="V3"},
                new { themeID="V4"},
            };
            ViewData["themeIDSelect"] = new SelectList(themeSelect, "themeID", "themeID");

            return View("~/Views/" + Utility.Version + "/BlogManager/Create.cshtml",m);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            blog_tb_blog entity = Utility.BlogBll.GetSingleBlogByUserID(UserInfo.UserID);
            if (entity == null)
            {
                entity = new blog_tb_blog();
                entity.userID = UserInfo.UserID;
                entity.ADD_DATE = DateTime.Now;
                entity.UPDATE_DATE = DateTime.Now;
            }
            else
            {
                return Json(new { code = -1, message = "暂时只允许创建一个博客" }, JsonRequestBehavior.AllowGet);
            }

            UpdateModel(entity);

            if (String.IsNullOrEmpty(entity.blogName))
            {
                return Json(new { code = -2, message = "博客名不能为空" }, JsonRequestBehavior.AllowGet);
            }

            if (String.IsNullOrEmpty(entity.blogTitle))
            {
                return Json(new { code = -3, message = "博客标题不能为空" }, JsonRequestBehavior.AllowGet);
            }

            //"fyj","kecq","bijiaoben","devblog","dev", "fangyjnet"
            List<string> notAllowBlogNameList = new List<string> { "www", "manage", "admin", "user", "passport", "my","fyj","kecq","bijiaoben","devblog","dev", "fangyjnet" };

            if (notAllowBlogNameList.Contains(entity.blogName.ToLower()))
            {
                return Json(new { code = -4, message = "不允许的博客名" }, JsonRequestBehavior.AllowGet);
            }

            if(Utility.BlogBll.IsExistsBlogName(entity.blogName))
            {
                return Json(new { code = -5, message = "博客名已经存在" }, JsonRequestBehavior.AllowGet);
            }

            Utility.BlogBll.Save(entity);

            return Json(new { code = 1, message = "操作成功" }, JsonRequestBehavior.AllowGet);
        }


        // GET: /BlogManager/Edit/5
        public ActionResult Edit(string id)
        {
            blog_tb_blog entity = Utility.BlogBll.GetSingleBlogByUserID(UserInfo.UserID);
            if (entity == null)
            {
                throw new CustomException("你还没有创建博客");
            }

            if (Convert.ToInt32(id) != entity.blogID)
            {
                return Json(new { code = -1, message = "你无权操作该博客" }, JsonRequestBehavior.AllowGet);
            }

            Blog m = ObjectHelper.CloneProperties<Blog>(entity);
            ViewBag.Pic = m.blogLogo;
            if (String.IsNullOrEmpty(m.blogLogo))
            {
                ViewBag.Pic = "http://static.kecq.com/images/common/nopic.jpg";
            }

            return View("~/Views/" + Utility.Version + "/BlogManager/Edit.cshtml",m);
        }


        public ActionResult EditModel(string id)
        {
            blog_tb_blog entity = Utility.BlogBll.GetSingleBlogByUserID(UserInfo.UserID);
            if (entity == null)
            {
                throw new CustomException("你还没有创建博客");
            }

            if (Convert.ToInt32(id) != entity.blogID)
            {
                return Json(new { code = -1, message = "你无权操作该博客" }, JsonRequestBehavior.AllowGet);
            }

            Blog m = ObjectHelper.CloneProperties<Blog>(entity);
            ViewBag.Pic = m.blogLogo;
            if (String.IsNullOrEmpty(m.blogLogo))
            {
                ViewBag.Pic = "http://static.kecq.com/images/common/nopic.jpg";
            }

            var themes = new List<object> {
                new { themeID="Default"},
                new { themeID="Cnblogs"},
                new { themeID="Muchun"},
                new { themeID="V2"},
                new { themeID="V3"},
                new { themeID="V4"},
            };

            //return new JsonNetResult(model, dateTimeFormat: "yyyy-MM-ddThh:mm");
            return new JsonNetResult(new { code = 1, message = "ok", data = m, themes = themes });
        }

        [HttpPost]
        public JsonResult Edit(string id, FormCollection collection)
        {
            blog_tb_blog entity = Utility.BlogBll.GetSingleBlogByUserID(UserInfo.UserID);

            if (Convert.ToInt32(id) != entity.blogID)
            {
                return Json(new { code = -1, message = "你无权操作该博客" }, JsonRequestBehavior.AllowGet);
            }

            UpdateModel(entity);
            if (Request["lastImageName_mainPic"] != "http://static.kecq.com/images/common/nopic.jpg")
            {
                entity.blogLogo = Request["lastImageName_mainPic"];
            }
            Utility.BlogBll.Save(entity);

            return Json(new { code = 1, message = "操作成功" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(FormCollection collection)
        {
            return Json(new { code = -1, message = "不允许删除" }, JsonRequestBehavior.AllowGet);
        }


        // GET: /BlogManager/Domain/5
        public ActionResult Domain(string id)
        {
            ViewBag.blogID = id;
            return View("~/Views/" + Utility.Version + "/BlogManager/Domain.cshtml");
        }

        
        public JsonResult GetDomainList(string id)
        {
            blog_tb_blog entity = Utility.BlogBll.GetSingleBlogByUserID(UserInfo.UserID);

            if (Convert.ToInt32(id) != entity.blogID)
            {
                return Json(new { code = -1, message = "你无权操作该博客" }, JsonRequestBehavior.AllowGet);
            }

            List<blog_tb_domain> list= Utility.BlogBll.GetDomainList(id);
            return Json(new { code = 1, data = list }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddDomain(string id, FormCollection collection)
        {
            blog_tb_blog entity = Utility.BlogBll.GetSingleBlogByUserID(UserInfo.UserID);

            if (Convert.ToInt32(id) != entity.blogID)
            {
                return Json(new { code = -1, message = "你无权操作该博客" }, JsonRequestBehavior.AllowGet);
            }

            string domain = collection["domain"];
            int port = Convert.ToInt32(collection["port"]);
            if(Utility.BlogBll.AddDomain(id,domain,port))
            {
                return Json(new { code = 1, message = "操作成功" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { code = -1, message = "操作失败" }, JsonRequestBehavior.AllowGet);
        }

       
        [HttpPost]
        public JsonResult UpdateDomain(string id, FormCollection collection)  //id为域名id   如果id 名称改为 domainid 就不行
        {
           if( Utility.BlogBll.GetDomainList(UserInfo.BlogID).Where(c=>c.ID==id).Count()==0)
            {
                return Json(new { code = -1, message = "你无权操作" }, JsonRequestBehavior.AllowGet);
            }

           
            string domain = collection["domain"];
            int port = Convert.ToInt32(collection["port"]);
            if (Utility.BlogBll.UpdateDomain(id, domain, port))
            {
                return Json(new { code = 1, message = "操作成功" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { code = -1, message = "操作失败" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteDomain(string id, FormCollection collection)   //id为域名id   如果id 名称改为 domainid 就不行
        {
            if (Utility.BlogBll.GetDomainList(UserInfo.BlogID).Where(c => c.ID == id).Count() == 0)
            {
                return Json(new { code = -1, message = "你无权操作" }, JsonRequestBehavior.AllowGet);
            }

            if (Utility.BlogBll.DeleteDomain(id))
            {
                return Json(new { code = 1, message = "操作成功" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { code = -1, message = "操作失败" }, JsonRequestBehavior.AllowGet);
        }
    }
}