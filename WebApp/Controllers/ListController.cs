using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    using Baike.Pagebuild.Models;

    public class ListController : Controller
    {
        //
        // GET: /List/
        public ActionResult Index()
        {
            var model = new ListPageModel();

            return View(model);
        }
    }
}