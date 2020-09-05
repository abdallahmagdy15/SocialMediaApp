using SocialMediaApp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMediaApp.Controllers
{

    public class ProfileController : Controller
    {
        //
        // GET: /Profile/
        [HttpGet]
        public ActionResult Profile(int userid)
        {
            var con = new SOCIALMEDIA_DBEntities();

            if (Session["ownerid"] == null)
                return RedirectToAction("Login", "Login");

            int ownerid = (Int32)Session["ownerid"];
            ProfileModel m = new ProfileModel();
            m.owner = con.USERS.Find(ownerid);
            m.headerm = new headerModel();
            m.headerm.owner = m.owner;

            //check my followings
            var friend = con.FOLLOWINGS.Where(f => f.USERID == ownerid && f.FOLLOWINGID == userid);
            if (friend.Count() != 0 )
            {
                if (friend.First().FR_CONFIRMED == true)
                    m.profileStatus = "friend";
                else
                    m.profileStatus = "waitConfirm";
            }
            // check his followings
            friend = con.FOLLOWINGS.Where(f => f.USERID == userid && f.FOLLOWINGID == ownerid);
            if(friend.Count() != 0)
            {
                if (friend.First().FR_CONFIRMED == true)
                    m.profileStatus = "friend";
                else
                    m.profileStatus = "FRsent";
            }
            if (m.profileStatus == null && userid != ownerid)
                m.profileStatus = "notFriend";

            return View(loadProfile(ownerid,userid , m));
        }
        public ProfileModel loadProfile(int ownerid ,int userid , ProfileModel m)
        {
            var con = new SOCIALMEDIA_DBEntities();
            m.user = new USER();
            m.friends_list = new ArrayList();
            m.posts = new ArrayList();

            //get user info
            m.user = con.USERS.Find(userid);

            //show only birth date in profile not hours
            var dob = m.user.DOB;
            m.user.DOB = Convert.ToDateTime(dob).Date;

            ///get friends list
            ArrayList friendsIds = new ArrayList();
            var friends = con.FOLLOWINGS.Where(f => f.USERID == userid && f.FR_CONFIRMED == true).Select(f => f.FOLLOWINGID);
            foreach(int id in friends){
                friendsIds.Add(id);
            }
            friends = con.FOLLOWINGS.Where(f => f.FOLLOWINGID == userid && f.FR_CONFIRMED == true).Select(f => f.USERID);
            foreach (int id in friends)
            {
                friendsIds.Add(id);
            }
            foreach (int fr in friendsIds)
            {
                USER friend = con.USERS.Find(fr);
                m.friends_list.Add(friend);
            }
            //load posts
            var posts = con.POSTS.Where(p => p.USERID == userid);
            foreach (POST post in posts)
            {
                //set like status
                if (con.LIKEDPOSTS.Where(l => l.POSTID == post.POSTID && l.USERID == ownerid).Count() != 0)
                    post.liked = true;
                else
                    post.liked = false;

                //load comments
                var comments = con.COMMENTS.Where(p => p.POSTID == post.POSTID);
                foreach (COMMENT comment in comments)
                {
                    // load replies
                    var replies = con.REPLIES.Where(p => p.COMMENTID == comment.COMMENTID);
                    foreach (REPLY reply in replies)
                    {
                        if (reply != null)
                            comment.REPLIES.Add(reply);
                    }
                    if (comment != null)
                        post.COMMENTS.Add(comment);
                }
                if (post != null)
                    m.posts.Add(post);
            }


            return m;
        }
        public ActionResult sendFR(int userid, int ownerid, String action_ , string controller_ )
        {
            var con = new SOCIALMEDIA_DBEntities();
            FOLLOWING follow = new FOLLOWING();

            //get last following id
            var arr = con.FOLLOWINGS.OrderByDescending(f => f.ID);
            int lastId = (arr.Count() != 0 ) ?arr.First().ID : 0 ;
            follow.ID = lastId + 1;
            follow.USERID = userid;
            follow.FOLLOWINGID = ownerid;
            follow.FR_CONFIRMED = false;
            //add user to the other user as temp friend 
            con.FOLLOWINGS.Add(follow);
            con.SaveChanges();
            return RedirectToAction(action_,controller_, new {userid = userid  });
        }
        public ActionResult confirmFR(int userid, int ownerid, String action_, string controller_)
        {
            var con = new SOCIALMEDIA_DBEntities();
            var friend = con.FOLLOWINGS.Where(f => f.USERID == ownerid && f.FOLLOWINGID == userid).First();
            friend.FR_CONFIRMED = true;
            con.SaveChanges();
            return RedirectToAction( action_ , controller_, new { userid = userid });
        }
        public ActionResult cancelFR(int userid, int ownerid, String action_, string controller_)
        {
            var con = new SOCIALMEDIA_DBEntities();
            var friend = con.FOLLOWINGS.Where(f => f.USERID == userid && f.FOLLOWINGID == ownerid).First();
            con.FOLLOWINGS.Remove(friend);
            con.SaveChanges();
            return RedirectToAction(action_, controller_, new { userid = userid });
        }
        public ActionResult unfriend(int userid, int ownerid, String action_, string controller_)
        {
            var con = new SOCIALMEDIA_DBEntities();
            var follow = con.FOLLOWINGS.Where(f => f.USERID == userid && f.FOLLOWINGID == ownerid);
            if(follow.Count() == 0)
                follow = con.FOLLOWINGS.Where(f => f.FOLLOWINGID == userid && f.USERID == ownerid);

            con.FOLLOWINGS.Remove(follow.FirstOrDefault());
            con.SaveChanges();
            return RedirectToAction(action_, controller_, new { userid = userid });
        }
        public ActionResult uploadDP(HttpPostedFileBase dp , int ownerid)
        {
            //upload Display Picture
            upload(dp,ownerid,"~/Data/Display_Pictures/","dp");
            return RedirectToAction("Profile", "Profile", new { userid=ownerid});
        }
        public ActionResult uploadBG(HttpPostedFileBase bg, int ownerid)
        {
            //upload Display Picture
            upload(bg,ownerid,"~/Data/BG/","bg");
            return RedirectToAction("Profile", "Profile", new { userid = ownerid });
        }
        public void upload(HttpPostedFileBase file, int ownerid,string dataPath,string type)
        {
            //upload Display Picture
            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath(dataPath), fileName);
                file.SaveAs(path);
                var con = new SOCIALMEDIA_DBEntities();
                var owner = con.USERS.Find(ownerid);
                if(type == "dp")
                    owner.DISPLAYPICTURE = "../Data/Display_Pictures/" + fileName;
                else if (type == "bg")
                    owner.BG = "../Data/BG/" + fileName;
                
                con.SaveChanges();
            }
        }
    }
}