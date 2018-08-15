using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace SimpleFormAuth.Helpers
{
    public static class SampleHelper
    {
        public static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetConnectionString()
        {
            string constr = ConfigurationManager.ConnectionStrings["SNTestDB"].ConnectionString;
            return constr;
        }

        public static string GetRandomPassword()
        {
            string password = string.Empty;
            Random ran  = new Random();
            password  = ran.Next(100000, 900000).ToString();
            return password;

        }

        public static void SendO365Mail(string receiver, string receivername, string subject, string body)
        {
            MailMessage message = new MailMessage();
            message.To.Add(new MailAddress(receiver, receivername));
            message.Bcc.Add(new MailAddress("tryanytest@gmail.com", "Senthil"));
            message.From = new MailAddress("senthiln365@outlook.com", "Senthil");
            message.Subject = subject;
            message.Body = body +" " + SampleHelper.GetMySite();
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("senthiln365@outlook.com", "try@mytest");
            client.Port = 587;
            client.Host = "smtp.office365.com";
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;

            client.Send(message);
        }

        public static string GetAuthCookieName()
        {
            return (FormsAuthentication.FormsCookieName + "SNTest");
        }

        public static string GetMySite()
        {
            //return HttpContext.Current.Request.Url.AbsoluteUri;
            return "http://snproto.azurewebsites.net";
        }
    }
}