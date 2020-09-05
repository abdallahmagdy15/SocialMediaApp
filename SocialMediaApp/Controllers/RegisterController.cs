using SocialMediaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMediaApp.Controllers
{
    public class RegisterController : Controller
    {
        //
        // GET: /Register/
        [HttpGet]
        public ActionResult Register(RegisterModel m)
        {
            if (m == null)
            {
                m = new RegisterModel();
                m.status = "";
            }
            return View(m);
        }
        [HttpPost]
        public ActionResult Register(USER user)
        {
            var m = new RegisterModel();
            var con = new SOCIALMEDIA_DBEntities();
            //check email , username
            var us = con.USERS.Where(u => u.EMAIL == user.EMAIL || u.USERNAME == user.USERNAME);
            if(us.Count() !=0){
                m.status = "registered";
                return View(m);
            }
            else
            {
                //get last id
                var ids = con.USERS.OrderByDescending(u => u.USERID);
                user.USERID = (ids.Count() != 0) ? ids.First().USERID + 1 : 1; 
                con.USERS.Add(user);
                con.SaveChanges();
            }
            return RedirectToAction("Login","Login");
        }
	}
}