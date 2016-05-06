using GetaJob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GetaJob.Controllers
{
    public class LoginController : Controller
    {
        private curtisGodaddyEntities db = new curtisGodaddyEntities();

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(GetaJob_User person)
        {
            var possiblePerson = db.GetaJob_User.Where(p => p.UserId == person.UserId && p.Password == person.Password).FirstOrDefault();
            if (possiblePerson != null)
            {
                SetCookie(possiblePerson.UserName );
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "userId not found";
            }
            return View();
        }

        public ActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddUser(GetaJob_User user)
        {
            db.GetaJob_User.Add(user);
            db.SaveChanges();
            (new LoginController()).SetCookie(user.UserName);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult UpdateUser()
        {
            var getaJob_User = db.GetaJob_User.Where(p => p.UserName == User.Identity.Name).FirstOrDefault();
            Session["olduserId"] = getaJob_User.UserId;
            //model.JobSearches = db.JobSearches.Where(j => j.UserId == model.UserId).ToList();
            return View(getaJob_User);
        }
        [HttpPost]
        public ActionResult UpdateUser(GetaJob_User changes)
        {
            var oldUserId = Session["olduserId"].ToString();
            GetaJob_User current = db.GetaJob_User.Where(c => c.UserId == oldUserId).First();
            current.UserId = changes.UserId;
            current.FirstName = changes.FirstName;
            current.LastName = changes.LastName;
            current.UserName = changes.UserName;
            current.UserId = changes.UserId;
            current.Password = changes.Password;
            current.CurrentJobSearchId = changes.CurrentJobSearchId;
            current.dob = changes.dob;

            db.SaveChanges();
            SetCookie(current.UserName);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            //Request.Cookies.Remove(FormsAuthentication.FormsCookieName);
            //System.Web.HttpContext.Current.User = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }


        public void SetCookie(string userName)
        {
            //string userData = "none";  // string.Join("|", GetCustomUserRoles());
            //FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
            //  1,                                     // ticket version
            //  userName,                              // authenticated username
            //  DateTime.Now,                          // issueDate
            //  DateTime.Now.AddDays(30),              // expiryDate
            //  true,                                  // true to persist across browser sessions
            //  userData,                              // can be used to store additional user data
            //  FormsAuthentication.FormsCookiePath);  // the path for the cookie

            //// Encrypt the ticket using the machine key
            //string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            //HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            ////cookie.HttpOnly = true;
            //System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            FormsAuthentication.SetAuthCookie(userName, true);
        }

    }
}