using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Canary.Web.Controllers
{
    public class EventController : Controller
    {
        //
        // GET: /Event/

        public ActionResult Index(string hash)
        {
            return View();
        }

    }
}
