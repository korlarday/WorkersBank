using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkersBank.Models.Data
{
    [Table("tblSpecifiedJobs")]
    public class SpecifiedJobsDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string JobName { get; set; }
    }
}