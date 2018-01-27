using FYJ.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Blogs.UI.Manage
{
    public class AuthenticationModule : IHttpModule, IRequiresSessionState
    {
        private const int AUTHENTICATION_TIMEOUT = 30;
        public AuthenticationModule()
        {
        }

        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += new EventHandler(context_AcquireRequestState);
            context.AuthenticateRequest += context_AuthenticateRequest;
        }

        void context_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;

            if (app.Context.User != null)
            {
                FormsIdentity idnetity = (FormsIdentity)app.Context.User.Identity;
                string str = idnetity.Ticket.UserData;
                string[] roles = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                app.Context.User = new GenericPrincipal(idnetity, roles);
            }
        }

        /// <summary>
        /// 这个非常重要，是用于对子页面的权限标签进行处理的代码，不可删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void context_AcquireRequestState(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpRequest Request = app.Request;
            HttpResponse Response = app.Response;
            HttpServerUtility Server = app.Server;

            string url = Request.Url.ToString().ToLower();
            //20170109
            url = Request.Url.Scheme + "://" + Request.Url.Host + Request.RawUrl;


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
            if (app.Context.User != null)
            {
                if (!app.Context.User.Identity.IsAuthenticated)
                {
                    if (String.IsNullOrEmpty(Request.QueryString["token"]))
                    {
                        app.Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["PassportRootUrl"].TrimEnd('/') + "/Login?BackUrl=" + app.Server.UrlEncode(url) + ingoreLogined);
                        app.Response.End();
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

        public void Dispose()
        {
        }
    }
}