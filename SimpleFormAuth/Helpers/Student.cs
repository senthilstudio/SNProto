using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SimpleFormAuth.Helpers
{
    public class Student
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
        public bool Locked { get; set; }

        public static List<Student> GetStudentsList()
        {
            List<Student> usersList = new List<Student>();
            Student myUser;
            using (SqlConnection con = new SqlConnection(SampleHelper.GetConnectionString()))
            {
                string query = "Select UserID, UserName, Name, Mobile, State, IsLockedOut, CreatedDate from OTCUsers where mtType=1"; //type =1  - students / type =2 admin

                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        myUser = new Student();
                        myUser.UserID = Convert.ToInt32(dr["UserID"]);
                        myUser.UserName = Convert.ToString(dr["UserName"]);
                        myUser.Name = Convert.ToString(dr["Name"]);
                        myUser.Mobile = Convert.ToString(dr["Mobile"]);
                        myUser.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                        myUser.Status = Convert.ToBoolean(dr["State"]);
                        myUser.Locked = Convert.ToBoolean(dr["IsLockedOut"]);
                        usersList.Add(myUser);
                    }
                    dr.Close();
                }
            }
            return usersList;
        }
    }
}