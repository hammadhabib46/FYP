//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Final_Project.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class accfuctional
    {
        public int AccF_ID { get; set; }
        public int AccF_MsID { get; set; }
        public string AccF_FName { get; set; }
        public string AccF_LName { get; set; }
        public Nullable<System.DateTime> AccF_DOB { get; set; }
        public string AccF_CNIC { get; set; }
        public string AccF_Phone { get; set; }
        public string AccF_Address { get; set; }
        public byte[] AccF_Pic { get; set; }
        public string AccF_Email { get; set; }
        public Nullable<bool> AccF_Gender { get; set; }
        public string AccF_PasswordName { get; set; }
        public string AccF_Qualification { get; set; }
        public string AccF_userNumber { get; set; }
        public string AccF_password { get; set; }
    
        public virtual m m { get; set; }
    }
}