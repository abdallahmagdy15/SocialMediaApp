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
    
    public partial class POSTS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public POSTS()
        {
            this.COMMENTS = new HashSet<COMMENTS>();
            this.LIKEDPOSTS = new HashSet<LIKEDPOSTS>();
        }
    
        public int POSTID { get; set; }
        public int USERID { get; set; }
        public string CONTENT { get; set; }
        public System.DateTime POSTDATE { get; set; }
        public Nullable<int> LIKES { get; set; }
        public string MEDIA { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMMENTS> COMMENTS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LIKEDPOSTS> LIKEDPOSTS { get; set; }
        public virtual USERS USERS { get; set; }
    }
}
