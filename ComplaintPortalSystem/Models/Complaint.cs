//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ComplaintPortalSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Complaint
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Complaint()
        {
            this.HandlerAssignments = new HashSet<HandlerAssignment>();
            this.SupervisorAssignments = new HashSet<SupervisorAssignment>();
        }
    
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public System.DateTime DateSubmitted { get; set; }
        public Nullable<System.DateTime> DateClose { get; set; }
        public Nullable<bool> IsRedFlag { get; set; }
        public Nullable<int> CentralUnitID { get; set; }
        public Nullable<int> ComplaintOwnerID { get; set; }
        public string PublicEmail { get; set; }
        public string PublicName { get; set; }
        public int CategoryID { get; set; }
        public Nullable<int> ExternalAgencyID { get; set; }
        public string RatingFeedback { get; set; }
        public string Attachment { get; set; }
        public Nullable<int> RatingEfficacy { get; set; }
        public Nullable<int> RatingSpeed { get; set; }
        public Nullable<int> RatingFriendliness { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual ExternalAgency ExternalAgency { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HandlerAssignment> HandlerAssignments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupervisorAssignment> SupervisorAssignments { get; set; }
        public virtual AccountHolder AccountHolder { get; set; }
    }
}
