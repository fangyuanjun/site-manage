
using FYJ;
using FYJ.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blogs.UI.Manage
{
    public class CheckIDFilter : FilterAttribute, IActionFilter, IResultFilter
    {

        public CheckIDFilter()
        {

        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string s = filterContext.HttpContext.Request.CurrentExecutionFilePath;
            string id = s.Substring(s.LastIndexOf("/") + 1);
            if (id != "0")
            {
                if (!Utility.BlogBll.CheckID(id, UserInfo.UserID))
                {
                    ContentResult result = new ContentResult();
                    result.Content = "没有权限操作该对象";
                    filterContext.Result = result;
                }
            }
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {

        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {

        }

        public static void CheckID(string id)
        {
            if (id != "0")
            {
                if (!Utility.BlogBll.CheckID(id, UserInfo.UserID))
                {
                    throw new CustomException(-1, "没有权限操作该对象");
                }
            }
        }
    }
}