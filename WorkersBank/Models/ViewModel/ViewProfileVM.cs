using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkersBank.Models.ViewModel
{
    public class ViewProfileVM
    {
        public UsersVM User { get; set; }
        public List<SeekerJobVM> SeekerJobs { get; set; }
        public JobSeekersVM JobSeeker { get; set; }
        public HirerVM Hirer { get; set; }
    }
}