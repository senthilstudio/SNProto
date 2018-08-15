using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using SimpleFormAuth.Helpers;

namespace SimpleFormAuth.Models
{
    public class OTCUsersModel
    {
        public string UserID { get; set; }

        [DisplayName("Name")]
        [StringLength(250, ErrorMessage = "Maximum length is 250")]
        [Required(ErrorMessage = "Name Required")]
        public string UserName { get; set; }

        [DisplayName("Email Address")]
        [Required(ErrorMessage = "Email Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Mail { get; set; }

        [DisplayName("Phone")]
        [Required(ErrorMessage = "Phone Required")]
        [StringLength(10, ErrorMessage = "Mobile numbere must have 10 charecters", MinimumLength = 10)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string Mobile { get; set; }

        [DisplayName("Useer Name")]
        [Required(ErrorMessage = "Usesr name Required")]
        public string UserLogin { get; set; }

        [DisplayName("Password")]
        [Required(ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        public  string UserPassword { get; set; }

        public bool IsUserExist(string email)
        {
            Int32 count;
            using (SqlConnection con = new SqlConnection(SampleHelper.GetConnectionString()))
            {
                string query = "Select 1 From OTCUsers where UserName=@email";

                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.AddWithValue("@email", email);
                    count = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
            }
            return (count>0?true:false);
        }

        public string ValidateUser(string username, string password)
        {
            string userInfo = string.Empty;
            using (MD5 md5Hash = MD5.Create())
            {
                password = SampleHelper.GetMd5Hash(md5Hash, password);
            }
            using (SqlConnection con = new SqlConnection(SampleHelper.GetConnectionString()))
            {
                string query = "Select * From OTCUsers where UserName=@email and password=@password";

                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.AddWithValue("@email", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        if (Convert.ToBoolean(dr["IsLockedOut"]))
                        {
                            userInfo = "X";
                        }
                        else
                        {
                            userInfo = Convert.ToString(dr["UserID"]) + "|" + Convert.ToString(dr["Name"]) + "|" + Convert.ToString(dr["UserName"]) + "|" + (Convert.ToInt32(dr["mtType"]) == 1 ? "User" : "Admin");
                        }
                    }
                    else
                    {
                        userInfo = "O";
                    }
                    con.Close();
                }
            }
            return userInfo;
        }

        public string CreateUseer(string name, string email, string mobile)
        {
            string password =  SampleHelper.GetRandomPassword();
            string hashPassword = string.Empty;


            using (MD5 md5Hash = MD5.Create())
            {
                hashPassword = SampleHelper.GetMd5Hash(md5Hash, password);
            }

            using (SqlConnection con = new SqlConnection(SampleHelper.GetConnectionString()))
            {
                string query = "INSERT INTO [OTCUsers] (UserName,Password,Name,Mobile,CreatedBy) values(@email,@password,@name,@mobile,101)";

                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", hashPassword);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@mobile", mobile);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            SampleHelper.SendO365Mail(email, name,"myTest Account", string.Format("User Name :{0} password : {1}", email, password));

            return string.Format("User created for {0}.",name);
        }

   }
   
}