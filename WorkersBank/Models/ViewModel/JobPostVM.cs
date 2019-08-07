using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WorkersBank.Models.Data;

namespace WorkersBank.Models.ViewModel
{
    public class JobPostVM
    {
        public JobPostVM()
        {

        }
        public JobPostVM(JobPostDTO row)
        {
            Id = row.Id;
            NameOfOrganisation = row.NameOfOrganisation;
            Address = row.Address;
            Description = row.Description;
            CreatedBy = row.CreatedBy;
            TimeCreated = row.TimeCreated;
            NumberApplied = row.NumberApplied;
            Slug = row.Slug;
            Type = row.Type;
            User = new UsersVM(row.User);
        }

        public int Id { get; set; }
        [Display(Name = "Name Of Organization")]
        [Required]
        public string NameOfOrganisation { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Description of Job")]
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime TimeCreated { get; set; }
        public int NumberApplied { get; set; }
        public string Slug { get; set; }
        public string Type { get; set; }

        public UsersVM User { get; set; }
    }
}