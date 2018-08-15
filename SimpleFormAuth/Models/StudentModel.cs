using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimpleFormAuth.Helpers;

namespace SimpleFormAuth.Models
{
    public class StudentModel
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
        public bool Locked { get; set; }

        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int PagerCount { get; set; }

        public List<Student> Students { get; set; }
    }
}