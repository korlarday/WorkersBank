using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkersBank.Models.Data
{
    [Table("tblJobApplied")]
    public class JobAppliedDTO
    {
        [Key]
        public int Id { get; set; }
        public int JobPostId { get; set; }
        public int UserId { get; set; }
        public DateTime TimeApplied { get; set; }
        public bool InvitationSent { get; set; }
        public bool UserRespond { get; set; }
        public bool Accept { get; set; }
        public bool Reject { get; set; }
        public DateTime? TimeSent { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

        [ForeignKey("JobPostId")]
        public virtual JobPostDTO JobPost { get; set; }

        [ForeignKey("UserId")]
        public virtual UsersDTO User { get; set; }
    }
}