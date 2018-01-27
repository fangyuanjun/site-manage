using FYJ;
using FYJ.Common;
using System.Web.Mvc;

namespace Blogs.UI.Manage
{
    public class ExceptionFilter :  IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            if (filterContext.Exception is CustomException)
            {
                JsonResult json = new JsonResult();
                json.Data = new { code = -1, message = filterContext.Exception.Message };
                filterContext.Result = json;
            }
            else
            {
                filterContext.ExceptionHandled = false;
            }
        }
    }
}