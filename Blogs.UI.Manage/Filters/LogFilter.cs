
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blogs.UI.Manage.Filters
{
    public class LogFilter : FilterAttribute, IActionFilter, IResultFilter
    {

        private string logTitle;
        private string logContent;
        private bool isLogActionExecuted; //操作完成后是否记录日志
        public LogFilter()
        { 
        }

        public LogFilter(string logTitle, string logContent, bool isLogActionExecuted)
        {
            this.logTitle = logTitle;
            this.logContent = logContent;
            this.isLogActionExecuted = isLogActionExecuted;
        }

      
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //if (this.isLogActionExecuted)
            //{
            //    try
            //    {
            //        Blogs.Entity.blog_tb_log model = new Entity.blog_tb_log();
            //        model.logID = Guid.NewGuid().ToString("N");
            //        model.logClass = "ActionExecuted";
            //        model.logSystem = "后台管理系统";
            //        model.logTitle = this.logTitle;
            //        model.logContent = this.logContent;
            //        model.logIP = filterContext.HttpContext.Request.UserHostAddress;
            //        model.logUrl = filterContext.Controller.ToString();
            //        System.Web.Mvc.Controller controller = (System.Web.Mvc.Controller)filterContext.Controller;
            //        Passport.Login.ICustomIdentity identity = (controller.User.Identity as Passport.Login.ICustomIdentity);
            //        model.userID = identity.UserID;
            //        model.userName = identity.UserName;
            //        model.ADD_DATE = DateTime.Now;
            //        model.UPDATE_DATE = DateTime.Now;
            //        model.logLevel = "message";
            //        Dal.Insert(model);
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
           
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            
        }
    }
}