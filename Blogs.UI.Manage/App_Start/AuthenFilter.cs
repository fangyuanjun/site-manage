using FYJ;
using FYJ.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Blogs.UI.Manage
{
    public class AuthenFilter : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {

            HttpContextBase context = filterContext.HttpContext;
            HttpRequestBase Request = context.Request;
            HttpResponseBase Response = context.Response;
            HttpServerUtilityBase Server = context.Server;

            string url = Request.Url.ToString().ToLower();

            //20170109
            url = Request.Url.Scheme + "://" + Request.Url.Host + Request.RawUrl;

            //如果用下面一段  nginx 不能正常
            //if (Regex.IsMatch(url, "^http://", RegexOptions.IgnoreCase) && Request.Url.Port == 80)
            //{
            //    Response.StatusCode = 301;
            //    Response.Status = "301 Moved Permanently";
            //    Response.AddHeader("Location", url.Replace("http://", "https://"));
            //    Response.End();
            //    return;
            //}

            if (Regex.IsMatch(url, "^http://" + Request.Url.Host + "/Account", RegexOptions.IgnoreCase))
            {
                return;
            }

            if (Regex.IsMatch(url, "^http://" + Request.Url.Host + ":\\d+/Account", RegexOptions.IgnoreCase))
            {
                return;
            }

            if (HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session["blog"] == null)
                {
                    if (!String.IsNullOrEmpty(UserInfo.UserID))
                    {
                        Blogs.Entity.blog_tb_blog blog = Utility.BlogBll.GetSingleBlogByUserID(UserInfo.UserID);
                        if (blog == null)
                        {
                            Response.ContentType = "text/html";
                            Response.Write("你无权管理该博客,<a href='http://user.kecq.com'>我的个人中心</a>");
                            Response.End();

                            //if (!Request.FilePath.Equals("/BlogManager/Create",StringComparison.CurrentCultureIgnoreCase))
                            //{
                            //    Response.ContentType = "text/html";
                            //    Response.Write("你还没有创建博客<a href='http://manage.blogmi.cn/BlogManager/Create'>点这里创建</a>");
                            //    Response.End();
                            //    throw new CustomException("你还没有创建博客");
                            //}
                        }
                        else
                        {
                            HttpContext.Current.Session["blog"] = blog;
                        }
                    }
                }
            }

            string ingoreLogined = "&IsIngoreLogined=1";
            ingoreLogined = "";   //暂时不支持一个浏览器登录多个用户
            if (context.User != null)
            {
                if (!context.User.Identity.IsAuthenticated)
                {
                    if (String.IsNullOrEmpty(Request.QueryString["token"]))
                    {
                        context.Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["PassportRootUrl"].TrimEnd('/') + "/Login?BackUrl=" + context.Server.UrlEncode(url) + ingoreLogined);
                        context.Response.End();
                    }
                    else
                    {
                        string s = FYJ.Common.HttpHelper.DoGet(System.Configuration.ConfigurationManager.AppSettings["PassportRootUrl"].TrimEnd('/') + "/Login/TokenGetCredence?token=" + Request.QueryString["Token"]);
                        if (!String.IsNullOrEmpty(s))
                        {
                            JObject v = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(s);
                            string userName = v["userName"].ToString();
                            string userID = v["userID"].ToString();
                            string userRole = "会员";

                            if (userName == "admin")
                            {
                                userRole = "管理员,会员";
                            }

                            //建立表单验证票据
                            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(1, s, DateTime.Now, DateTime.MaxValue, true, userRole, "/");
                            //使用webcongfi中定义的方式,加密序列化票据为字符串
                            string HashTicket = FormsAuthentication.Encrypt(Ticket);
                            //将加密后的票据转化成cookie
                            HttpCookie UserCookie = new HttpCookie(FormsAuthentication.FormsCookieName, HashTicket);
                            //添加到客户端cookie
                            Response.Cookies.Add(UserCookie);

                            url = Regex.Replace(url, @"(\?|&)Token=.*", "", RegexOptions.IgnoreCase);
                            //登录成功后重定向
                            Response.Redirect(url);
                        }
                        else
                        {
                            url = Regex.Replace(url, @"(\?|&)Token=.*", "", RegexOptions.IgnoreCase);
                            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["PassportRootUrl"].TrimEnd('/') + "/Login?BackURL=" + Server.UrlEncode(url) + ingoreLogined);
                        }
                    }
                }
            }
        }

    }
}