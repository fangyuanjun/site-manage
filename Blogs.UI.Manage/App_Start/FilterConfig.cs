using System.Web;
using System.Web.Mvc;

namespace Blogs.UI.Manage
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            filters.Add(new AuthenFilter());
        }
    }
}
