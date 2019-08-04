using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WorkersBank.Models.Data;

namespace WorkersBank.Models.ViewModel
{
    public class SpecifiedJobsVM
    {
        public SpecifiedJobsVM()
        {

        }
        public SpecifiedJobsVM(SpecifiedJobsDTO row)
        {
            Id = row.Id;
            UserId = row.UserId;
            JobName = row.JobName;
        }
        public int Id { get; set; }
        public int UserId { get; set; }
        public string JobName { get; set; }
    }
}