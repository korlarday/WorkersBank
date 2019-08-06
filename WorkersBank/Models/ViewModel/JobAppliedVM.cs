using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            InvitationSent = row.InvitationSent;
            UserRespond = row.UserRespond;
            Accept = row.Accept;
            Reject = row.Reject;
            TimeSent = row.TimeSent;
            Subject = row.Subject;
            Message = row.Message;

            JobPost = new JobPostVM(row.JobPost);
            User = new UsersVM(row.User);
        }
        public int Id { get; set; }
        public int JobPostId { get; set; }
        public int UserId { get; set; }
        public DateTime TimeApplied { get; set; }
        public bool InvitationSent { get; set; }
        public bool UserRespond { get; set; }
        public bool Accept { get; set; }
        public bool Reject { get; set; }
        public DateTime? TimeSent { get; set; }
        public string Subject { get; set; }
        [AllowHtml]
        public string Message { get; set; }

        public JobPostVM JobPost { get; set; }
        public UsersVM User { get; set; }
    }
}