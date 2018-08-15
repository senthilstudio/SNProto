using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using SimpleFormAuth.Helpers;

namespace SimpleFormAuth.Controllers
{
    public class ADOUserController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GenerateHash()
        {
            return View();
        }

        public ActionResult SendMail()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public string Authorize(SimpleFormAuth.Models.UsersModel userModel)
        {
            string AuthorizedUser = string.Empty;
            string constr = ConfigurationManager.ConnectionStrings["SNTestDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "Select * From Users where UseName=@UserName and Password=@Password";

                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.AddWithValue("@UserName", userModel.UseName);
                    cmd.Parameters.AddWithValue("@Password", userModel.Password);
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        userModel.uid = Convert.ToInt32(dr["uid"]);
                        AuthorizedUser = userModel.uid.ToString();
                    }
                    con.Close();
                }
            }

            return AuthorizedUser;
        }

        public string GenerateMD5Data(FormCollection formData)
        {
            string userName = string.Empty;
            string password = String.Empty;
            string hashPassword = string.Empty;

            userName = Convert.ToString(formData["UserName"]);
            password = Convert.ToString(formData["Password"]);

            using (MD5 md5Hash = MD5.Create())
            {
                hashPassword = SampleHelper.GetMd5Hash(md5Hash, userName);
            }
            return hashPassword;
        }

        [HttpPost]
        public ActionResult SendMail(FormCollection formData)
        {
            try
            {
                string from, fromname, to, toname, msgsubject, msgbody = string.Empty;

                from = Convert.ToString(formData["sender"]);
                fromname = "mytest";
                to = Convert.ToString(formData["receiver"]);
                toname = "student";
                msgsubject = Convert.ToString(formData["Subject"]);
                msgbody = Convert.ToString(formData["Message"]);
                SendO365Mail(from,fromname,to,toname,msgsubject,msgbody);

                /*if (Convert.ToString(formData["sender"]).Trim().Length > 0)
                {
                    var senderEmail = new MailAddress(Convert.ToString(formData["sender"]), "Sender");
                    var receiverEmail = new MailAddress(Convert.ToString(formData["receiver"]), "Receiver");
                    var password = Convert.ToString(formData["senderpwd"]);
                    var sub = Convert.ToString(formData["Subject"]);
                    var body = Convert.ToString(formData["Message"]);
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = sub,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                    return View();
                }*/
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.StackTrace;
            }
            return View();
        }

        [NonAction]
        public void SendO365Mail(string sender, string sendername, string receiver, string receivername,string subject, string body)
        {
            MailMessage message = new MailMessage();
            message.To.Add(new MailAddress(receiver, sendername));
            message.From = new MailAddress("senthiln365@outlook.com", "senthil");
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("senthiln365@outlook.com", "try@mytest");
            client.Port = 587;
            client.Host = "smtp.office365.com";
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message + " --->" + ex.StackTrace;
            }
        }

    }
}