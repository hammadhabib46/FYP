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
    
    public partial class creator
    {
        public creator()
        {
            this.ms = new HashSet<m>();
        }
    
        public long id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string c_password { get; set; }
        public string email { get; set; }
    
        public virtual ICollection<m> ms { get; set; }
    }
}
