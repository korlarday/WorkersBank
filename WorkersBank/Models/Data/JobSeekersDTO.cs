using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkersBank.Models.Data
{
    [Table("tblJobSeekers")]
    public class JobSeekersDTO
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Qualification { get; set; }
        public string LastInstitution { get; set; }
        public string WorkExperience { get; set; }
        public string SkillsAcquired { get; set; }

        [ForeignKey("UserId")]
        public virtual UsersDTO User { get; set; }
    }
}