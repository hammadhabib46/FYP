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
    
    public partial class studfuctional
    {
        public int studF_ID { get; set; }
        public Nullable<int> studF_FamilyID { get; set; }
        public int studF_MSID { get; set; }
        public string studF_ClassName { get; set; }
        public string studF_SectionName { get; set; }
        public string studF_FName { get; set; }
        public string studF_LName { get; set; }
        public Nullable<System.DateTime> studF_DOB { get; set; }
        public string studF_CNIC { get; set; }
        public string studF_Phone { get; set; }
        public string studF_Address { get; set; }
        public byte[] studF_Pic { get; set; }
        public string studF_Email { get; set; }
        public string studF_Gender { get; set; }
        public string studF_GuardName { get; set; }
        public string studF_GuardCNIC { get; set; }
        public string studF_GuardContact { get; set; }
        public Nullable<double> studF_PendingFee { get; set; }
        public string studF_RollNO { get; set; }
        public string studF_password { get; set; }
    
        public virtual m m { get; set; }
    }
}