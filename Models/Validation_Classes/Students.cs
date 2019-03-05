using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace School.Models
{
    [MetadataType(typeof(Student_Validation))]
    public partial class Student
    {
    }

    public class Student_Validation
    {
        [Required( ErrorMessage = "FullName is Required")]
        [MaxLength(150, ErrorMessage = "FullName must be maximum 150 characters or less"), MinLength(10,ErrorMessage = "FullName can't be less than 10 characters")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Name contains Invalid Characters")]

        [Remote("Json_fullname", "Students", AdditionalFields = "InitialName",HttpMethod = "POST", ErrorMessage = "Duplicate Fullname")]
        public string fullname { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int age { get; set; }
        [Required]
        public string gender { get; set; }
        [Required]
        public System.DateTime birth_date { get; set; }

        [RegularExpression(@"^01[0-9]{9}$", ErrorMessage = "Not a valid Phone number [01XXXXXXXXX]")]
        public string phone { get; set; }
    }

}
