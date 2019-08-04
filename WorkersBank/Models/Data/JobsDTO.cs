using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkersBank.Models.Data
{
    [Table("tblJobs")]
    public class JobsDTO
    {
        [Key]
        public int Id { get; set; }
        public string JobName { get; set; }
    }
}