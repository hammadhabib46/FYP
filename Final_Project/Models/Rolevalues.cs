using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models
{
    public class Rolevalues
    {
        public Boolean Admin { get; set; }
        public Boolean Student { get; set; }
        public Boolean HR { get; set; }
        public Boolean Accountant { get; set; }

        public Boolean Adminportal { get; set; }
        public Boolean Studentportal { get; set; }
        public Boolean HRportal { get; set; }
        public Boolean Accountantportal { get; set; }


    }
}