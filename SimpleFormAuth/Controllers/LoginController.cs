using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using SimpleFormAuth.Models;
using SimpleFormAuth.Security;
using SimpleFormAuth.Helpers;
namespace SimpleFormAuth.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(SimpleFormAuth.Models.User userModel)
        {
            using (SNTestEntities db = new SNTestEntities())
            {
                var userDetails = db.Users.Where(x => x.UseName == userModel.UseName && x.Password == userModel.Password).FirstOrDefault();
                if (userDetails == null)
                {
                    userModel.LogginErrorMessage = "Invalid user name  / password";
                    return View("Index", userModel);
                }
                else
                {
                    return View();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection formData)
        {
            OTCUsersModel userModel = new OTCUsersModel();
            string username = Convert.ToString(formData["UserLogin"]);
            string password = Convert.ToString(formData["UserPassword"]);
            string userInfo = userModel.ValidateUser(username, password);

            if (userInfo.Trim().Length > 0)
            {
                if (userInfo == "X")
                {
                    ViewBag.Message = "Your account got locked. Please contact admin!";
                }
                else if (userInfo == "O")
                {
                    ViewBag.Message = "Invalid User Name or Password.";
                }
                else if (userInfo.Trim().Length > 10) //Asume valid userInfo will have more than 10 lenth
                {
                    var UserDetails = userInfo.Split('|');

                    CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                    serializeModel.UserID = Convert.ToInt32(UserDetails[0]);
                    serializeModel.Name = UserDetails[1];
                    serializeModel.UserName = UserDetails[2];
                    serializeModel.Role = UserDetails[3];

                    string userData = JsonConvert.SerializeObject(serializeModel);

                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                             1,
                             UserDetails[0],
                             DateTime.Now,
                             DateTime.Now.AddMinutes(15),
                             false,
                             userData);

                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie(SampleHelper.GetAuthCookieName(), encTicket);
                    Response.Cookies.Add(faCookie);

                    if (serializeModel.Role == "User")
                    {
                        return RedirectToAction("Index", "Student");
                    }
                    else if (serializeModel.Role == "Admin")
                    {
                        return RedirectToAction("Index", "Admin");
                        //return RedirectToAction("NewUser", "OTC");
                    }
                }
            }
            //return RedirectToAction("Index", "Login");
            return Login();
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Response.Cookies[SampleHelper.GetAuthCookieName()].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("Login", "Login");
        }
    }
}