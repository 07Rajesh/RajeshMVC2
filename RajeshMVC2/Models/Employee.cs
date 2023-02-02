using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RajeshMVC2.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Please Enter Name !")]
     //   [MaxLength(40,ErrorMessage ="Name con't be longer the 40 characters!")]
        [StringLength(40, ErrorMessage = "Name con't be longer the 40 characters!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter Email !")]
        [EmailAddress(ErrorMessage = "Please enter only valid Email!")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Please Enter Mobile !")]
        [RegularExpression(@"\+?[0-9]{10}", ErrorMessage = "Please enter only valid mobile number!")]
       [MaxLength(10,ErrorMessage ="number con't be longer the 10 number!")]

        public string Mobile { get; set; }

        [Required(ErrorMessage = "Please Enter Gender !")]
        public string Gender { get; set; }
        public string Image { get; set; }
        [Required(ErrorMessage = "Please Enter Salary !")]
        public decimal Salary { get; set; }
        [Required(ErrorMessage = "Please Enter Department !")]
        public int Dept_Id { get; set; }
    }
}
