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
    
    public partial class subject
    {
        public int Subj_ID { get; set; }
        public string Subj_name { get; set; }
        public int Cls_id { get; set; }
    
        public virtual @class @class { get; set; }
    }
}