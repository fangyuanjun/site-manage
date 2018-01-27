using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FYJ;

namespace Blogs.UI.Manage
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        /// <summary>
        /// 返回是否显示错误
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool IsShowError(HttpContext context)
        {
            string str = String.Empty;
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load(context.Server.MapPath("~/web.config"));
            return (doc.SelectSingleNode("//customErrors").Attributes["mode"].Value == "Off");
        }

        void Application_Error(object sender, EventArgs e)
        {
            if (HttpContext.Current.Server.GetLastError() is HttpRequestValidationException)
            {
                HttpContext.Current.Response.Write("请输入合法的字符串【<a href=\"javascript:history.back(0);\">返回</a>】");
                HttpContext.Current.Server.ClearError();
                return;
            }

            if (HttpContext.Current.Server.GetLastError() is CustomException)
            {
                CustomException ex = HttpContext.Current.Server.GetLastError() as CustomException;
                HttpContext.Current.Response.ContentType = "text/plain";
                HttpContext.Current.Response.Write(ex.ToString());
                HttpContext.Current.Server.ClearError();
                return;
            }

           
            //Server.GetLastError().GetBaseException();
            string url = "";
            try
            {
                url = HttpContext.Current.Request.Url.ToString();
            }
            catch { }

            Exception excep = HttpContext.Current.Server.GetLastError();
            if (excep.Message.StartsWith("The controller for path"))
            {
                //FYJ.Common.LogHelper.WriteLog("HTTP 404:" + url);
            }
            else if (excep.Message.StartsWith("__RequestVerificationToken"))
            {
                //FYJ.Common.LogHelper.WriteLog("跨站点攻击:" + url);
                HttpContext.Current.Response.Write(new MessageModel(-99, "服务器拒绝,原因:跨站点攻击").ToString());
                HttpContext.Current.Server.ClearError();
            }
            else
            {
                LogHelper.WriteLog(excep, Environment.NewLine + "系统异常:" + url);
            }

            if (!IsShowError(HttpContext.Current))
            {
                HttpContext.Current.Response.Write("系统运行发生异常");
                HttpContext.Current.Server.ClearError();
            }
        }
    }
}