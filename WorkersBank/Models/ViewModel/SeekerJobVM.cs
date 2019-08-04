using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WorkersBank.Models.Data;

namespace WorkersBank.Models.ViewModel
{
    public class SeekerJobVM
    {
        public SeekerJobVM()
        {

        }
        public SeekerJobVM(SeekerJobDTO row)
        {
            Id = row.Id;
            UserId = row.UserId;
            JobId = row.JobId;
            User = new UsersVM(row.User);
            Jobs = new JobsVM(row.Jobs);
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int JobId { get; set; }

        public UsersVM User { get; set; }
        public JobsVM Jobs { get; set; }
    }
}