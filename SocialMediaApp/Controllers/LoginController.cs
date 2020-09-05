using SocialMediaApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMediaApp.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        [HttpGet]
        public ActionResult Login(LoginModel m)
        {
            if (m == null)
            {
                m = new LoginModel();
                m.status = "";
            }
            return View(m);
        }
        [HttpPost]

        public ActionResult Login(string username , string password)
        {
            bool x = false;
            using (DbContext dbContext = new DbContext("name=SOCIALMEDIA_DBEntities"))
            {
                x = dbContext.Database.Exists();
            }
            var con = new SOCIALMEDIA_DBEntities();
            LoginModel login = new LoginModel();
            var user = con.USERS.Where(u => u.USERNAME == username && u.PASSWORD == password);
            if (user.Count() != 0){
                Session["ownerid"] = user.First().USERID;
                return RedirectToAction("Home", "Home", null);
            }
                
            else
            {
                login.status = "failed";
                return View(login);
            }

        }
	}
}