using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WorkersBank.Models.Data;

namespace WorkersBank.Models.ViewModel
{
    public class JobsVM
    {
        public JobsVM()
        {

        }
        public JobsVM(JobsDTO row)
        {
            Id = row.Id;
            JobName = row.JobName;
        }

        public int Id { get; set; }
        public string JobName { get; set; }
    }
}