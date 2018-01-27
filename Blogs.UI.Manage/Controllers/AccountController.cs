using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Blogs.UI.Manage.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View("~/Views/"+Utility.Version+ "/Account/Index.cshtml");
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            //建立表单验证票据
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(1, "userid", DateTime.Now, DateTime.MaxValue, true, "管理员,会员", "/");

            //使用webcongfi中定义的方式,加密序列化票据为字符串
            string HashTicket = FormsAuthentication.Encrypt(Ticket);

            //将加密后的票据转化成cookie
            HttpCookie UserCookie = new HttpCookie(FormsAuthentication.FormsCookieName, HashTicket);

            //添加到客户端cookie
            Response.Cookies.Add(UserCookie);

            //Thread.CurrentPrincipal = new CustomPrincipal(user);
            //User = new CustomPrincipal(user);
            //FormsAuthentication.SetAuthCookie(user.Name, persistLogin);

            //登录成功后重定向

            return Redirect("/");
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public ActionResult Logout(FormCollection collection)
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            string url = Server.UrlEncode("http://" + Request.Url.Host);

            return Redirect(System.Configuration.ConfigurationManager.AppSettings["PassportRootUrl"].TrimEnd('/')+"/Login/Logout?BackURL=" + url);
        }
    }
}
