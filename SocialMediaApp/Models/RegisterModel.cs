using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SocialMediaApp.Models
{
    public class RegisterModel
    {
        public USER user { set; get; }
        public string status { set; get; }
        [Required(ErrorMessage="Repeat Password is Required!")]
        [DataType(DataType.Password)]
        //[Compare("user.PASSWORD",ErrorMessage="Passwords must be the same!")]
        public string repeatPass { set; get; }
    }
}


