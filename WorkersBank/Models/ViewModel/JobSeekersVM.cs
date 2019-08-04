using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkersBank.Models.Data;

namespace WorkersBank.Models.ViewModel
{
    public class JobSeekersVM
    {
        public JobSeekersVM()
        {

        }
        public JobSeekersVM(JobSeekersDTO row)
        {
            Id = row.Id;
            UserId = row.UserId;
            Qualification = row.Qualification;
            LastInstitution = row.LastInstitution;
            WorkExperience = row.WorkExperience;
            SkillsAcquired = row.SkillsAcquired;
            User = new UsersVM(row.User);
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        [AllowHtml]
        public string Qualification { get; set; }
        public string LastInstitution { get; set; }
        [AllowHtml]
        public string WorkExperience { get; set; }
        [AllowHtml]
        public string SkillsAcquired { get; set; }

        public UsersVM User { get; set; }
    }
}