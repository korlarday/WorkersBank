using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WorkersBank.Models.Data;

namespace WorkersBank.Models.ViewModel
{
    public class JobAppliedVM
    {
        public JobAppliedVM()
        {

        }
        public JobAppliedVM(JobAppliedDTO row)
        {
            Id = row.Id;
            JobPostId = row.JobPostId;
            UserId = row.UserId;
            TimeApplied = row.TimeApplied;
            JobPost = new JobPostVM(row.JobPost);
            User = new UsersVM(row.User);
        }
        public int Id { get; set; }
        public int JobPostId { get; set; }
        public int UserId { get; set; }
        public DateTime TimeApplied { get; set; }

        public JobPostVM JobPost { get; set; }
        public UsersVM User { get; set; }
    }
}