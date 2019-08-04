using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkersBank.Models.ViewModel
{
    public class FetchJobs
    {
        public List<JobsVM> Jobs { get; set; }
        public List<int> JobsSelected { get; set; }
    }
}