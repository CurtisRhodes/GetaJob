//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GetaJob.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class JobListing
    {
        public int JobListingId { get; set; }
        public Nullable<int> JobSearchId { get; set; }
        public Nullable<System.DateTime> PostedDate { get; set; }
        public Nullable<int> ListingStatus { get; set; }
        public Nullable<int> AgentId { get; set; }
        public Nullable<int> TargetCompanyId { get; set; }
        public string JobTitle { get; set; }
        public string Rate { get; set; }
        public Nullable<int> EmploymentType { get; set; }
        public string Comments { get; set; }
        public string Distance { get; set; }
        public Nullable<int> ListingSource { get; set; }
        public Nullable<int> Desirability { get; set; }
        public Nullable<int> Fit { get; set; }
    
        public virtual Agent Agent { get; set; }
        public virtual JobSearch JobSearch { get; set; }
        public virtual TargetCompany TargetCompany { get; set; }
    }
}
