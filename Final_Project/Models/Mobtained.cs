using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Final_Project.Models
{
    public class Mobtained
    {
        [Required(ErrorMessage = "You must provide a Marks")]
        [DataType(DataType.PhoneNumber)]
        public double obtainedMarks { get; set; }
    }
}