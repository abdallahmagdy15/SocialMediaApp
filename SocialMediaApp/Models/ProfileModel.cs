using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMediaApp.Models
{
    public class ProfileModel
    {
        //personal info **pic and name
        public USER owner { set; get; }
        // other users data
        public USER user { set; get; }

        public string profileStatus { set; get; }

        //posts
        public ArrayList posts { set; get; }
        //create post
        public string postContent { get; set; }

        //create comment
        public string commentContent { get; set; }

        //create reply
        public string replyContent { get; set; }
        //friends list
        public ArrayList friends_list { set; get; }
        public headerModel headerm { set; get; }
    }
}