using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Blogs.UI.Manage.Controllers
{
    public class WelcomeController : Controller
    {
        //
        // GET: /Welcome/

        public ActionResult Index()
        {
            return View("~/Views/" + Utility.Version + "/Welcome/Index.cshtml");
        }

    }
}
