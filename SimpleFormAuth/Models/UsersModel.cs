using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;


namespace SimpleFormAuth.Models
{
    public class UsersModel
    {
        public int uid { get; set; }
        [DisplayName("User Name")]
        [Required(ErrorMessage = "User Name Required")]
        public string UseName { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }

        public string LogginErrorMessage { get; set; }
    }
}