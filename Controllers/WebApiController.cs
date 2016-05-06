using GetaJob.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

//using System.Web.Http.Results;

namespace GetaJob.Controllers
{
    public class WebApiController : ApiController
    {

        [Route("JobSearch/api/WebApi/GetJobSearchs")]
        [HttpGet]
        public AngularJobSearch GetJobSearchs()
        {
            var angularJobSearch = new AngularJobSearch();
            using (var db = new curtisGodaddyEntities())
            {
                var dbJobSearches = db.JobSearches.Where(js => js.UserId == User.Identity.Name).ToList();
                foreach (JobSearch js in dbJobSearches)
                {
                    angularJobSearch.jobSearches.Add(new AngularJobSearchs()
                    {
                        id = js.JobSearchId,
                        start = js.Start.ToShortDateString(),
                        description = js.Description,
                        completed = js.Completed == null ? "" : js.Completed.Value.ToShortDateString()
                    });
                }
            }
            return angularJobSearch;
        }

        [Route("JobSearch/api/WebApi/AddEditJobSearch")]
        [HttpPost]
        public string AddEditJobSearch(AngularJobSearchs selectedJobSearch)
        {
            string success = "ono";
            try
            {

                using (var db = new curtisGodaddyEntities())
                {
                    JobSearch jobSearch = null;
                    if (selectedJobSearch.id == 0)
                    {
                        jobSearch = new JobSearch();
                        jobSearch.UserId = db.GetaJob_User.Where(p => p.UserName == User.Identity.Name).FirstOrDefault().UserId;
                        db.JobSearches.Add(jobSearch);
                    }
                    else
                    {
                        jobSearch = db.JobSearches.Where(j => j.JobSearchId == selectedJobSearch.id).First();
                    }
                    jobSearch.Description = selectedJobSearch.description;
                    jobSearch.Start = DateTime.Parse(selectedJobSearch.start);
                    if (selectedJobSearch.completed == null)
                        jobSearch.Completed = null;
                    else
                        jobSearch.Completed = DateTime.Parse(selectedJobSearch.completed);

                    db.SaveChanges();
                    success = "ok";
                }
            }
            catch (DbEntityValidationException ex)
            {
                success = "ValidationExceptions: ";
                foreach (var eve in ex.EntityValidationErrors)
                {
                    success += eve.Entry.Entity.GetType().Name + ": " + eve.Entry.State;
                    foreach (var ve in eve.ValidationErrors)
                    {
                        success += " Property: " + ve.PropertyName + " Error: " + ve.ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                success = ex.Message;
                if (ex.InnerException != null)
                    success += " inner: " + ex.InnerException.Message;
            }
            return success;
        }

        [Route("JobSearch/api/WebApi/GetJobListings")]
        [HttpGet]
        public JobListingModel GetJobListings()
        {
            string success = "ono";
            var jobListingModel = new JobListingModel();
            try
            {
                using (var db = new curtisGodaddyEntities())
                {

                    var currentJobSearchId = db.GetaJob_User.Where(p => p.UserName == User.Identity.Name).FirstOrDefault().CurrentJobSearchId;
                    var dbJobListings = db.JobListings.Where(jl => jl.JobSearchId == currentJobSearchId).ToList();
                    foreach (JobListing jl in dbJobListings)
                    {
                        var item = new AngularJobListing()
                        {
                            Id = jl.JobListingId,
                            PostedDate = jl.PostedDate == null ? "" : jl.PostedDate.Value.ToShortDateString(),
                            JobTitle = jl.JobTitle,
                            Rate = jl.Rate,
                            ListingStatus = jl.ListingStatus,
                            EmploymentType = jl.EmploymentType,
                            ListingSource = jl.ListingSource,
                            Desirability = jl.Desirability,
                            Fit = jl.Fit,
                            Comments = jl.Comments,
                            Distance = jl.Distance
                        };
                        jobListingModel.JobListings.Add(item);

                        if (jl.TargetCompanyId != null)
                        {
                            var targetCompany = db.TargetCompanies.Where(tc => tc.CompanyId == jl.TargetCompanyId).First();
                            item.TargetCompanyName = targetCompany.Name;
                            item.TargetCompanyCity = targetCompany.City;
                            item.TargetCompanyState = targetCompany.State;
                            item.TargetCompanyZip = targetCompany.Zip;
                        }

                        if (jl.AgentId != null)
                        {
                            var agent = db.Agents.Where(a => a.AgentId == jl.AgentId).First();
                            item.AgentName = agent.AgentName;
                            item.AgentWorkPhone = agent.WorkPhone;
                            item.AgentCellPhone = agent.CellPhone;
                            item.AgentEmail = agent.Email;
                            if (agent.AgencyId != null)
                            {
                                var agency = db.Agencies.Where(a => a.AgencyId == agent.AgencyId).First();
                                item.AgencyId = agent.AgencyId.Value;
                                item.AgencyName = agency.Name;
                                item.AgencyStreetAddress = agency.StreetAddress;
                                item.AgencyCity = agency.City;
                                item.AgencyState = agency.State;
                                item.AgencyZip = agency.Zip;
                                item.AgencyWorkPhone = agency.WorkPhone;
                                item.AgencyCellPhone = agency.CellPhone;
                                item.AgencyEmail = agency.Email;
                            }
                        }
                    }
                }
                jobListingModel.ListingStatuses = GetRefs(2);
                jobListingModel.ListingSources = GetRefs(3);
                jobListingModel.Desirabilities = GetRefs(4);
                jobListingModel.EmploymentTypes = GetRefs(5);
                jobListingModel.Fits = GetRefs(6);
            }
            catch (DbEntityValidationException ex)
            {
                success = "ValidationExceptions: ";
                foreach (var eve in ex.EntityValidationErrors)
                {
                    success += eve.Entry.Entity.GetType().Name + ": " + eve.Entry.State;
                    foreach (var ve in eve.ValidationErrors)
                    {
                        success += " Property: " + ve.PropertyName + " Error: " + ve.ErrorMessage;
                    }
                }
                throw new Exception(success);
            }
            catch (Exception ex)
            {
                success = ex.Message;
                if (ex.InnerException != null)
                    success += " inner: " + ex.InnerException.Message;
                throw new Exception(success);
            }
            return jobListingModel;
        }

        [Route("JobSearch/api/WebApi/AddEditJobListing")]
        [HttpPost]
        public int AddEditJobListing(AngularJobListing selectedJobListing)
        {
            string success = "ono";
            int newId = 0;
            try
            {
                using (var db = new curtisGodaddyEntities())
                {
                    JobListing dbJobListing = null;
                    if (selectedJobListing.Id == 0)
                    {
                        dbJobListing = new JobListing { };
                        dbJobListing.JobSearchId = db.GetaJob_User.Where(u => u.UserName == User.Identity.Name).First().CurrentJobSearchId;
                        db.JobListings.Add(dbJobListing);
                    }
                    else
                    {
                        dbJobListing = db.JobListings.Where(j => j.JobListingId == selectedJobListing.Id).First();
                    }

                    dbJobListing.JobTitle = selectedJobListing.JobTitle;
                    dbJobListing.PostedDate = DateTime.Parse(selectedJobListing.PostedDate);
                    dbJobListing.Rate = selectedJobListing.Rate;
                    dbJobListing.Comments = selectedJobListing.Comments;
                    dbJobListing.Distance = selectedJobListing.Distance;
                    dbJobListing.EmploymentType = selectedJobListing.EmploymentType;
                    dbJobListing.ListingStatus = selectedJobListing.ListingStatus;
                    dbJobListing.ListingSource = selectedJobListing.ListingSource;
                    dbJobListing.Desirability = selectedJobListing.Desirability;
                    dbJobListing.Fit = selectedJobListing.Fit;

                    dbJobListing.TargetCompanyId = AddEditTargetCompany(selectedJobListing);

                    int? agencyId = AddEditAgency(selectedJobListing);
                    if (agencyId != null)
                        selectedJobListing.AgencyId = agencyId.Value;

                    dbJobListing.AgentId = AddEditAgent(selectedJobListing);

                    db.SaveChanges();

                    newId = dbJobListing.JobListingId;
                }
            }
            catch (DbEntityValidationException ex)
            {
                success = "ValidationExceptions: ";
                foreach (var eve in ex.EntityValidationErrors)
                {
                    success += eve.Entry.Entity.GetType().Name + ": " + eve.Entry.State;
                    foreach (var ve in eve.ValidationErrors)
                    {
                        success += " Property: " + ve.PropertyName + " Error: " + ve.ErrorMessage;
                    }
                    throw new Exception(success);
                }
            }
            catch (Exception ex)
            {
                success = ex.Message;
                if (ex.InnerException != null)
                    success += " inner: " + ex.InnerException.Message;
                throw new Exception(success);
            }
            return newId;
        }

        int? AddEditAgent(AngularJobListing selectedJobListing)
        {
            int? agentId = null;
            using (var db = new curtisGodaddyEntities())
            {
                Agent agent = null;
                if (selectedJobListing.AgentId != 0)
                {
                    agent = db.Agents.Where(a => a.AgentId == selectedJobListing.AgentId).FirstOrDefault();
                }
                if (agent == null)
                {
                    if (!string.IsNullOrWhiteSpace(selectedJobListing.AgentName))
                        agent = db.Agents.Where(a => a.AgentName == selectedJobListing.AgentName).FirstOrDefault();
                }
                if (agent == null)
                {
                    agent = new Agent();
                    db.Agents.Add(agent);
                }
                agent.AgentName = selectedJobListing.AgentName;
                agent.WorkPhone = selectedJobListing.AgentWorkPhone;
                agent.CellPhone = selectedJobListing.AgentCellPhone;
                agent.Email = selectedJobListing.AgentEmail;
                agent.AgencyId = selectedJobListing.AgencyId;

                db.SaveChanges();
                agentId = agent.AgentId;
            }
            return agentId;
        }

        int? AddEditAgency(AngularJobListing selectedJobListing)
        {
            int? agencyId = null;
            if (!string.IsNullOrWhiteSpace(selectedJobListing.AgencyName))
            {
                using (var db = new curtisGodaddyEntities())
                {
                    Agency agency = null;
                    if ((selectedJobListing.Id != 0) && selectedJobListing.AgencyId != 0)
                    {
                        agency = db.Agencies.Where(a => a.AgencyId == selectedJobListing.AgencyId).FirstOrDefault();
                    }
                    if (agency == null)
                        agency = db.Agencies.Where(a => a.Name == selectedJobListing.AgencyName).FirstOrDefault();
                    if (agency == null)
                    {
                        agency = new Agency();
                        db.Agencies.Add(agency);
                    }
                    agency.Name = selectedJobListing.AgencyName;
                    agency.StreetAddress = selectedJobListing.AgencyStreetAddress;
                    agency.City = selectedJobListing.AgencyCity;
                    agency.State = selectedJobListing.AgencyState;
                    agency.Zip = selectedJobListing.AgencyZip;
                    agency.WorkPhone = selectedJobListing.AgencyWorkPhone;
                    agency.CellPhone = selectedJobListing.AgencyCellPhone;
                    agency.Email = selectedJobListing.AgencyEmail;

                    db.SaveChanges();
                    agencyId = db.Agencies.Where(c => c.Name == selectedJobListing.AgencyName).First().AgencyId;
                }
            }
            return agencyId;
        }

        int? AddEditTargetCompany(AngularJobListing selectedJobListing)
        {
            //Unable to update the EntitySet 'TargetCompany' because it has a DefiningQuery and no<InsertFunction>
            //element exists in the<ModificationFunctionMapping> element to support the current operation.

            int? targetCompanyId = null;
            TargetCompany targetCompany = null;
            if (!string.IsNullOrWhiteSpace(selectedJobListing.TargetCompanyName))
                using (var db = new curtisGodaddyEntities())
                {
                    if ((selectedJobListing.Id != 0) && selectedJobListing.TargetCompanyId != 0)
                        targetCompany = db.TargetCompanies.Where(tc => tc.CompanyId == selectedJobListing.TargetCompanyId).FirstOrDefault();
                    if (targetCompany == null)
                        targetCompany = db.TargetCompanies.Where(tc => tc.Name == selectedJobListing.TargetCompanyName).FirstOrDefault();
                    if (targetCompany == null)
                    {
                        targetCompany = new TargetCompany();
                        db.TargetCompanies.Add(targetCompany);
                    }
                    targetCompany.Name = selectedJobListing.TargetCompanyName;
                    targetCompany.StreetAddress = selectedJobListing.TargetCompanyStreetAddress;
                    targetCompany.City = selectedJobListing.TargetCompanyCity;
                    targetCompany.State = selectedJobListing.TargetCompanyState;
                    targetCompany.Zip = selectedJobListing.TargetCompanyZip;

                    db.SaveChanges();
                    targetCompanyId = db.TargetCompanies.Where(c => c.Name == selectedJobListing.TargetCompanyName).First().CompanyId;
                }
            return targetCompanyId;
        }

        [Route("JobSearch/api/WebApi/GetJobSearchActivities")]
        [HttpGet]
        public JobActivityModel GetJobSearchActivities(int selectedJobListingId)
        {
            JobActivityModel jobActivityModel = new JobActivityModel();
            using (var db = new curtisGodaddyEntities())
            {
                var activitites = db.JobSearchActivities.Where(jl => jl.JobListingId == selectedJobListingId).ToList();
                foreach (JobSearchActivity ja in activitites)
                {
                    var item = new AngularActivity()
                    {
                        JobListingId = selectedJobListingId,
                        ActivityId = ja.JobSearchActivityId,
                        ActivityDate = ja.ActivityDate.ToShortDateString(),
                        ActivityStatus = ja.ActivityStatus.Value,
                        Comments = ja.Comments
                    };
                    jobActivityModel.Activitites.Add(item);
                }
                jobActivityModel.ActivityTypes = GetRefs(1);

                return jobActivityModel;
            }
        }

        [Route("JobSearch/api/WebApi/AddEditActivity")]
        [HttpPost]
        public int AddEditActivity(AngularActivity selectedActivity)
        {
            int newId = 0;
            string success = "ono";
            try
            {
                using (var db = new curtisGodaddyEntities())
                {
                    JobSearchActivity dbJobSearchActivity = null;
                    if (selectedActivity.ActivityId == 0)
                    {
                        dbJobSearchActivity = new JobSearchActivity { };
                        dbJobSearchActivity.JobListingId = selectedActivity.JobListingId;
                        db.JobSearchActivities.Add(dbJobSearchActivity);
                    }
                    else
                    {
                        dbJobSearchActivity = db.JobSearchActivities.Where(j => j.JobSearchActivityId == selectedActivity.ActivityId).First();
                    }
                    dbJobSearchActivity.Comments = selectedActivity.Comments;
                    dbJobSearchActivity.ActivityDate = DateTime.Parse(selectedActivity.ActivityDate);
                    dbJobSearchActivity.ActivityStatus = selectedActivity.ActivityStatus;                                           

                    db.SaveChanges();
                    newId = dbJobSearchActivity.JobSearchActivityId;
                    success = "ok";
                }
            }
            catch (DbEntityValidationException ex)
            {
                success = "ValidationExceptions: ";
                foreach (var eve in ex.EntityValidationErrors)
                {
                    success += eve.Entry.Entity.GetType().Name + ": " + eve.Entry.State;
                    foreach (var ve in eve.ValidationErrors)
                    {
                        success += " Property: " + ve.PropertyName + " Error: " + ve.ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                success = ex.Message;
                if (ex.InnerException != null)
                    success += " inner: " + ex.InnerException.Message;
            }
            return newId;
        }

        [Route("JobSearch/api/WebApi/GetRefs")]
        [HttpGet]
        public List<AngularRef> GetRefs(int refType)
        {
            var angularRefs = new List<AngularRef>();
            using (var db = new curtisGodaddyEntities()) {
                var dbdd= db.DropDownValues.Where(dd => dd.RefType == refType);
                foreach (DropDownValue dv in dbdd)
                {
                    angularRefs.Add(new AngularRef() { RefCode = dv.RefCode, RefType = dv.RefType.Value, RefDescription = dv.RefDescription });
                }
            }
            return angularRefs;
        }

        [Route("JobSearch/api/WebApi/AddEditRef")]
        [HttpPost]
        public string AddEditRef(AngularRef selectedRef)
        {
            using (var db = new curtisGodaddyEntities())
            {
                if (selectedRef.RefCode == -1)
                {
                    var dbRef = new DropDownValue() { RefDescription = selectedRef.RefDescription, RefType = selectedRef.RefType };
                    db.DropDownValues.Add(dbRef);
                    db.SaveChanges();
                }
                else {
                    var dbRef = db.DropDownValues.Where(r => r.RefCode == selectedRef.RefCode).First();
                    dbRef.RefDescription = selectedRef.RefDescription;
                    db.SaveChanges();
                }
            }
            return "ok";
        }

    }
}










