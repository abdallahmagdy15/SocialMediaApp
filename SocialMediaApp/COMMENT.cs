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
    
    public partial class COMMENT
    {
        public COMMENT()
        {
            this.REPLIES = new HashSet<REPLY>();
        }
    
        public int COMMENTID { get; set; }
        public string CONTENT { get; set; }
        public int USERID { get; set; }
        public Nullable<int> LIKES { get; set; }
        public int POSTID { get; set; }
    
        public virtual POST POST { get; set; }
        public virtual USER USER { get; set; }
        public virtual ICollection<REPLY> REPLIES { get; set; }
    }
}
