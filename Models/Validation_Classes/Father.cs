using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace School.Models
{
    public class Father_signup_Model
    {
        [Required(ErrorMessage = "Username is Required")]
        [MaxLength(25, ErrorMessage = "FullName must be maximum 25 characters"), MinLength(5, ErrorMessage = "FullName can't be less than 5 characters")]
        [RegularExpression(@"^[a-zA-Z]{1}[a-zA-z1-9_]*$", ErrorMessage = "Combination[letters, numbers, Underscores ], first character must be a letter")]
        [Remote("Json_signup", "Home", HttpMethod = "POST", ErrorMessage = "User name already exists. Please enter a different user name.")]
        public string username { get; set; }
    }

    public class Father_signin_Model
    {
        [Required(ErrorMessage = "Username is Required")]
        [Remote("Json_signin", "Home", HttpMethod = "POST", ErrorMessage = "UserName is Incorrect")]
        public string username { get; set; }
    }

}
