using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMediaApp.Models
{
    public class MessengerModel
    {
        public int userId { get; set; }
        public ArrayList date { get; set; }
        public ArrayList userOneMess { get; set; }
        public ArrayList userTwoMess { get; set; }

        public ArrayList consId { get; set; }
        public ArrayList conversations { get; set; }
        public ArrayList notifications { get; set; }
        public ArrayList starter { get; set; }
        public ArrayList contacts { get; set; }
        public headerModel headerm { set; get; }
    }
}