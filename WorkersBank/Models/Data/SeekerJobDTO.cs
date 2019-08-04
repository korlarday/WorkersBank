using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkersBank.Models.Data
{
    [Table("tblSeekerJob")]
    public class SeekerJobDTO
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int JobId { get; set; }

        [ForeignKey("UserId")]
        public virtual UsersDTO User { get; set; }

        [ForeignKey("JobId")]
        public virtual JobsDTO Jobs { get; set; }
    }
}