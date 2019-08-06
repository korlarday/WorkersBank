using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkersBank.Models.ViewModel
{
    public class ViewApplicantsVM
    {
        public List<JobAppliedVM> Applicants { get; set; }
        public JobPostVM Job { get; set; }
        public HirerVM Hirer { get; set; }
    }
}