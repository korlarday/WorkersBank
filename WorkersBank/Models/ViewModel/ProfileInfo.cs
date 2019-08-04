using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WorkersBank.Models.ViewModel
{
    public class ProfileInfo
    {
        public UsersVM User { get; set; }
        public JobSeekersVM JobSeeker { get; set; }
        public HirerVM Hirer { get; set; }
        public List<SeekerJobVM> Jobs { get; set; }
        public List<SpecifiedJobsVM> SpecifiedJobs { get; set; }

        [Required(ErrorMessage = "Please upload your passport")]
        public HttpPostedFileBase Passport { get; set; }

    }
}