using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMediaApp.Models
{
    public class chatModel
    {
        public int conID { get; set; }
        public ArrayList userOneMess { get; set; }
        public ArrayList userTwoMess { get; set; }
        public ArrayList date { get; set; }
        public int starter { get; set; }
    }
}