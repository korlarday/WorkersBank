using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkersBank.Models.Data
{
    [Table("tblJobPost")]
    public class JobPostDTO
    {
        [Key]
        public int Id { get; set; }
        public string NameOfOrganisation { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime TimeCreated { get; set; }
        public int NumberApplied { get; set; }
        public string Slug { get; set; }
        public string Type { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual UsersDTO User { get; set; }
    }
}