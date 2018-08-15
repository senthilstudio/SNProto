using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleFormAuth.Helpers;
using SimpleFormAuth.Models;
using SimpleFormAuth.Security;

namespace SimpleFormAuth.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class AdminController : BaseeController
    {
        // GET: Admin

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewUser(FormCollection formData)
        {
            string name, email, phone, msg = string.Empty;
            name = Convert.ToString(formData["UserName"]);
            email = Convert.ToString(formData["Mail"]);
            phone = Convert.ToString(formData["Mobile"]);

            try
            {
                if (ModelState.IsValid && !string.IsNullOrEmpty(email))
                {
                    OTCUsersModel otcUseer = new OTCUsersModel();

                    if (!otcUseer.IsUserExist(email))
                    {
                        msg = otcUseer.CreateUseer(name, email, phone);
                        return RedirectToAction("ListUsers", "Admin");
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

        public ActionResult ListUsers()
        {
            StudentModel studentModel = new StudentModel();
            studentModel.PageSize = 10;

            List<Student> students = Student.GetStudentsList();

            if (students != null)
            {
                studentModel.TotalCount = students.Count();
                studentModel.Students = students;
            }

            return View(studentModel);

        }
    }
}