//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SocialMediaApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class POST
    {
        public POST()
        {
            this.COMMENTS = new HashSet<COMMENT>();
            this.LIKEDPOSTS = new HashSet<LIKEDPOST>();
        }
    
        public int POSTID { get; set; }
        public int USERID { get; set; }
        public string CONTENT { get; set; }
        public System.DateTime POSTDATE { get; set; }
        public Nullable<int> LIKES { get; set; }
        public string MEDIA { get; set; }
        public bool liked { set; get; }
        public virtual ICollection<COMMENT> COMMENTS { get; set; }
        public virtual USER USER { get; set; }
        public virtual ICollection<LIKEDPOST> LIKEDPOSTS { get; set; }
    }
}