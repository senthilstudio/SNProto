using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleFormAuth.Security;

namespace SimpleFormAuth.Controllers
{
    [CustomAuthorize(Roles = "User")]
    public class StudentController : BaseeController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyTests()
        {
            return View();
        }

    }
}