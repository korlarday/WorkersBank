using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkersBank.Models.Data
{
    [Table("tblHirer")]
    public class HirerDTO
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string NameOfOrganisation { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

        [ForeignKey("UserId")]
        public virtual UsersDTO User { get; set; }
    }
}