using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleFormAuth.Models;
using SimpleFormAuth.Security;

namespace SimpleFormAuth.Controllers
{
    
    public class OTCController : Controller
    {
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult NewUser()

        {
            return View();
        }


        [HttpPost]
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult NewUser(FormCollection formData)
        {
            string name, email, phone, msg = string.Empty;
            name = Convert.ToString(formData["UserName"]);
            email = Convert.ToString(formData["Mail"]);
            phone = Convert.ToString(formData["Mobile"]);

            try
            {
                if (ModelState.IsValid)
                {
                    OTCUsersModel otcUseer = new OTCUsersModel();

                    if (!otcUseer.IsUserExist(email))
                    {
                        msg = otcUseer.CreateUseer(name, email, phone);
                    }
                    else
                    {
                        msg = string.Format("User recode already exist with email address {0}", email);
                    }
                    ViewBag.Message = msg;
                }
                
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }



    }
}