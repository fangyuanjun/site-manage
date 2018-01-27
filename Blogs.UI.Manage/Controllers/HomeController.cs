using Blogs.Entity;
using Blogs.UI.Manage.Filters;
using FYJ.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Blogs.UI.Manage.Controllers
{
    [LogFilter]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            int count = Utility.CommentBll.GetNoReadCommentCount(UserInfo.BlogID);
            ViewData["NoReadCommentCount"] = count;

            return View("~/Views/" + Utility.Version + "/Home/Index.cshtml");
        }

        [LogFilter("登出", "进行了登出操作", true)]
        public ActionResult LogOff()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Session.RemoveAll();
            return Redirect(System.Configuration.ConfigurationManager.AppSettings["PassportRootUrl"].TrimEnd('/') +"/Login/Logout?BackURL=" + Server.UrlEncode(FYJ.Common.HttpHelper.GetRootPath(Request.Url.ToString())));
        }

        public ActionResult AuthJump()
        {
            if (Request.QueryString["action"] == "logout")
            {
                Session.RemoveAll();
                Response.Redirect(Server.UrlDecode(Request.QueryString["ref"]));
            }
            else
            {
                //Passport.Sso.AuthLoad auth = new Passport.Sso.AuthLoad();
                //auth.IsClearSession = true;  //重要
                //auth.IsRedirect = false;
                //auth.Login();

                Session["Token"] = "{\"userID\":\"8dc2885cd7bb44aa9344557dbc0c1630\",\"userName\":\"admin\",\"userNumber\":\"10000\"}";


                if (Session["Token"] != null)
                {
                    if (!String.IsNullOrEmpty(Request.QueryString["ref"]))
                    {
                        Response.Redirect(Request.QueryString["ref"]);
                    }
                    else
                    {
                        JObject jobj = Newtonsoft.Json.JsonConvert.DeserializeObject(Session["Token"].ToString()) as JObject;
                        string userID = jobj["userID"].ToString();
                        string userName = jobj["userName"].ToString();

                        return Json(new { code = 1, message = "已登录" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { code = -1, message = "未登录" }, JsonRequestBehavior.AllowGet);
                }
            }

            return new EmptyResult();
        }


    }
}