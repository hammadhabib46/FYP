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
    
    public partial class markstotal
    {
        public int Mtotal_ID { get; set; }
        public int Mforgn_ID { get; set; }
        public Nullable<double> Mtotal { get; set; }
    
        public virtual marksp marksp { get; set; }
    }
}