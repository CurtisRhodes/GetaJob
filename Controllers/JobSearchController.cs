using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using GetaJob.Models;

namespace GetaJob.Controllers
{
    public class JobSearchController : Controller
    {
        private curtisGodaddyEntities db = new curtisGodaddyEntities();

        [CustomAuthorize]
        public ActionResult JobSearch()
        {
            return View();
        }

        [CustomAuthorize]
        public ActionResult JobListing()
        {
            return View();
        }
        [CustomAuthorize]
        public ActionResult Admin()
        {
            return View();
        }
    }
}