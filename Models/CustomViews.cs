using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetaJob.Models
{
    public class AngularJobSearchs
    {
        public int id { get; set; }
        public string start { get; set; }
        public string description { get; set; }
        public string completed { get; set; }
    }
    public class AngularJobSearch
    {
        public AngularJobSearch()
        {
            jobSearches = new List<AngularJobSearchs>();
        }
        public string prop1 { get; set; }
        public List<AngularJobSearchs> jobSearches { get; set; }
    }

    public class AngularJobListing
    {
        public int Id { get; set; }
        public string PostedDate { get; set; }
        public int? ListingStatus { get; set; }
        public string JobTitle { get; set; }
        public string Rate { get; set; }
        public int? EmploymentType { get; set; }
        public string Comments { get; set; }
        public string Distance { get; set; }
        public int? ListingSource { get; set; }
        public int? Desirability { get; set; }
        public int? Fit { get; set; }

        public int TargetCompanyId { get; set; }
        public string TargetCompanyName { get; set; }
        public string TargetCompanyStreetAddress { get; set; }
        public string TargetCompanyCity { get; set; }
        public string TargetCompanyState { get; set; }
        public string TargetCompanyZip { get; set; }

        public int AgentId { get; set; }
        public string AgentName { get; set; }
        public string AgentWorkPhone { get; set; }
        public string AgentCellPhone { get; set; }
        public string AgentEmail { get; set; }

        public int AgencyId { get; set; }
        public string AgencyName { get; set; }
        public string AgencyStreetAddress { get; set; }
        public string AgencyCity { get; set; }
        public string AgencyState { get; set; }
        public string AgencyZip { get; set; }
        public string AgencyWorkPhone { get; set; }
        public string AgencyCellPhone { get; set; }
        public string AgencyEmail { get; set; }
    }
    
    public class AngularActivity
    {
        public int ActivityId { get; set; }
        public int JobListingId { get; set; }
        public string ActivityDate { get; set; }
        public int ActivityStatus { get; set; }
        public string Comments { get; set; }
    }
    public class JobActivityModel
    {
        public JobActivityModel()
        {
            Activitites = new List<AngularActivity>();
        }
        public List<AngularActivity> Activitites { get; set; }
        public List<AngularRef> ActivityTypes { get; set; }
    }

    public class JobListingModel
    {
        public JobListingModel()
        {
            JobListings = new List<AngularJobListing>();
        }
        public List<AngularJobListing> JobListings { get; set; }
        public List<AngularRef> ListingStatuses { get; set; }
        public List<AngularRef> EmploymentTypes { get; set; }
        public List<AngularRef> ListingSources { get; set; }
        public List<AngularRef> Desirabilities { get; set; }
        public List<AngularRef> Fits { get; set; }
    }

    public class AngularRef
    {
        public int RefType { get; set; }
        public int RefCode { get; set; }
        public string RefDescription { get; set; }
    }

}