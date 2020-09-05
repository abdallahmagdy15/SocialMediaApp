using SocialMediaApp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialMediaApp;
using System.Globalization;
using System.IO;

namespace SocialMediaApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home/
        [HttpGet]
        public ActionResult Home()
        {
            if(Session["ownerid"]==null)
                return RedirectToAction("Login","Login");

            int ownerid = (Int32)Session["ownerid"];
            var con = new SOCIALMEDIA_DBEntities();

            HomeModel m = new HomeModel();
            m.friends_list = new ArrayList();
            m.posts = new ArrayList();
            m.sugg_friends = new ArrayList();

            //get owner info
            m.owner = con.USERS.Find(ownerid);
            //set user to default *owner info*
            m.user = m.owner;
            m.headerm = new headerModel();
            m.headerm.owner = m.owner;
            ///get friends list
            ArrayList friendsIds =new ArrayList();
            var friends = con.FOLLOWINGS.Where(f => f.USERID == ownerid && f.FR_CONFIRMED == true).Select(f => f.FOLLOWINGID);
            foreach (int id in friends)
            {
                friendsIds.Add(id);
            }
            var friends_ = con.FOLLOWINGS.Where(f => f.FOLLOWINGID == ownerid && f.FR_CONFIRMED == true).Select(f => f.USERID);
            foreach (int id in friends_)
            {
                friendsIds.Add(id);
            }

            foreach (int fr in friendsIds)
            {
                USER friend = con.USERS.Find(fr);
                m.friends_list.Add(friend);
            }

            //get sugg_friends list
            var All = con.USERS.Select(u => u.USERID);
            foreach (int id in All)
            {
                if( !friendsIds.Contains(id) && id != ownerid){
                    USER sugg = con.USERS.Find(id);

                    //set sugg status
                    //  check my followings
                    var friend = con.FOLLOWINGS.Where(f => f.USERID == ownerid && f.FOLLOWINGID == id);
                    if (friend.Count() != 0)
                    {
                        if (friend.First().FR_CONFIRMED == true)
                            sugg.FRstatus = "friend";
                        else
                            sugg.FRstatus = "waitConfirm";
                    }
                    // check his followings
                    friend = con.FOLLOWINGS.Where(f => f.USERID == id && f.FOLLOWINGID == ownerid);
                    if (friend.Count() != 0)
                    {
                        if (friend.First().FR_CONFIRMED == true)
                            sugg.FRstatus = "friend";
                        else
                            sugg.FRstatus = "FRsent";
                    }
                    if (sugg.FRstatus == null)
                        sugg.FRstatus = "notFriend";

                    m.sugg_friends.Add(sugg);
                }
                
            }

            //load posts
            var posts = con.POSTS.Where(p => friends.Contains(p.USERID) || friends_.Contains(p.USERID) || p.USERID == ownerid);
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

            return View(m);
        }
        [HttpGet]
        public ActionResult gotoProfile(int userid){
            return RedirectToAction("Profile", "Profile", new { userid = userid });
        }
        
        [HttpPost]
        public ActionResult createPost(string postContent, int ownerid, string action_, string controller_, HttpPostedFileBase attach)
        {
            POST post = new POST();
            post.CONTENT = postContent;
            post.POSTDATE = DateTime.Now;
            post.LIKES = 0;
            post.USERID = ownerid;

            //upload attachments
            if (attach != null)
            {
                var fileName = Path.GetFileName(attach.FileName);
                var path = Path.Combine(Server.MapPath("~/Data/Posts_Attachments/"), fileName);
                attach.SaveAs(path);

                post.MEDIA = "../Data/Posts_Attachments/" + fileName;

            }
            
            var con = new SOCIALMEDIA_DBEntities();
            
            var posts = con.POSTS.OrderByDescending(p=>p.POSTID);
            post.POSTID = (posts.Count() != 0)? posts.First().POSTID + 1:1;
            con.POSTS.Add(post);
            
            con.SaveChanges();
            return RedirectToAction(action_, controller_, new { userid = ownerid });
        }
        [HttpPost]
        public ActionResult createComment(int postid, string commentContent, int ownerid, string action_, string controller_, int userid=1)
        {
            COMMENT comment = new COMMENT();
            comment.CONTENT = commentContent;
            comment.LIKES = 0;
            comment.USERID = ownerid;
            comment.POSTID = postid;
            var con = new SOCIALMEDIA_DBEntities();

            //get the last id and ++ the new id
            var comms = con.COMMENTS.OrderByDescending(c => c.COMMENTID);
            comment.COMMENTID =(comms.Count() != 0 )? comms.First().COMMENTID + 1:1;

            con.COMMENTS.Add(comment);
            con.SaveChanges();
            return RedirectToAction(action_, controller_, new { userid = userid});
        }

        [HttpPost]
        public ActionResult createReply(int commentid, string replyContent, int ownerid, string action_, string controller_, int userid=1)
        {
            REPLY reply = new REPLY();
            reply.CONTENT = replyContent;
            reply.LIKES = 0;
            reply.USERID = ownerid;
            reply.COMMENTID = commentid;
            var con = new SOCIALMEDIA_DBEntities();

            //get the last id and ++ the new id
            var reps = con.REPLIES.OrderByDescending(r => r.REPLYID);
            reply.REPLYID = (reps.Count() != 0 )? reps.First().REPLYID + 1:1;

            con.REPLIES.Add(reply);
            con.SaveChanges();
            return RedirectToAction(action_, controller_, new { userid = userid});
        }

        public ActionResult likePost(int postid, string action_, string controller_, int ownerid, int userid = 0)
        {
            var con = new SOCIALMEDIA_DBEntities();
            var likedpost = con.LIKEDPOSTS.Where(l => l.USERID == ownerid && l.POSTID == postid);
            if (likedpost.Count() == 0)
            {
                var post = con.POSTS.Find(postid);
                post.LIKES++;
                LIKEDPOST liked = new LIKEDPOST();
                var last = con.LIKEDPOSTS.OrderByDescending(l => l.ID);
                liked.ID = (last.Count()!=0 )? con.LIKEDPOSTS.OrderByDescending(l => l.ID).First().ID + 1 : 1;
                liked.USERID = ownerid;
                liked.POSTID = postid;
                con.LIKEDPOSTS.Add(liked);
            }
            else
            {
                var post = con.POSTS.Find(postid);
                post.LIKES--;
                con.LIKEDPOSTS.Remove(likedpost.First());
            }

            con.SaveChanges();
            return RedirectToAction(action_, controller_, new { userid = userid, ownerid = ownerid });
        }
        public ActionResult delPost(int postid, string action_, string controller_, int userid = 0){
            var con = new SOCIALMEDIA_DBEntities();
            var post = con.POSTS.Find(postid);
            var comments = con.COMMENTS.Where(c => c.POSTID == postid);
            foreach(var c in comments){//remove replies for each comment
                con.REPLIES.RemoveRange(con.REPLIES.Where(r => r.COMMENTID == c.COMMENTID));
            }
            var liked = con.LIKEDPOSTS.Where(l => l.POSTID == postid);
            con.COMMENTS.RemoveRange(comments);
            con.LIKEDPOSTS.RemoveRange(liked);
            con.POSTS.Remove(post);
            con.SaveChanges();
            return RedirectToAction(action_, controller_, new { userid = userid});
        }
        public ActionResult delComment(int commentid, string action_, string controller_, int userid = 0)
        {
            var con = new SOCIALMEDIA_DBEntities();
            var comment = con.COMMENTS.Find(commentid);
            var reps = con.REPLIES.Where(r => r.COMMENTID == commentid);
            con.REPLIES.RemoveRange(reps);
            con.COMMENTS.Remove(comment);
            con.SaveChanges();
            return RedirectToAction(action_, controller_, new { userid = userid });
        }
        public ActionResult delReply(int replyid, string action_, string controller_, int userid = 0)
        {
            var con = new SOCIALMEDIA_DBEntities();
            var reply = con.REPLIES.Find(replyid);
            con.REPLIES.Remove(reply);
            con.SaveChanges();
            return RedirectToAction(action_, controller_, new { userid = userid});
        }
        public ActionResult editPost(int postid, string action_, string controller_, string content, int userid)
        {
            var con = new SOCIALMEDIA_DBEntities();
            var post = con.POSTS.Find(postid);
            post.CONTENT = content;
            con.SaveChanges();
            return RedirectToAction(action_, controller_, new {userid=userid });
        }
        public ActionResult editComment(int commentid, string action_, string controller_, string content, int userid)
        {
            var con = new SOCIALMEDIA_DBEntities();
            var comment = con.COMMENTS.Find(commentid);
            comment.CONTENT = content;
            con.SaveChanges();
            return RedirectToAction(action_, controller_, new { userid = userid });
        }
        public ActionResult editReply(int replyid, string action_, string controller_, string content, int userid)
        {
            var con = new SOCIALMEDIA_DBEntities();
            var reply = con.REPLIES.Find(replyid);
            reply.CONTENT = content;
            con.SaveChanges();
            return RedirectToAction(action_, controller_, new { userid = userid });
        }

        public ActionResult logout()
        {
            Session["ownerid"]=null;
            return RedirectToAction("Login", "Login");
        }
    }
    
}