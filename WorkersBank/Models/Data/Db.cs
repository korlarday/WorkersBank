using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WorkersBank.Models.Data
{
    public class Db: DbContext
    {
        public DbSet<UsersDTO> Users { get; set; }
        public DbSet<SeekerJobDTO> SeekerJobs { get; set; }
        public DbSet<RolesDTO> Roles { get; set; }
        public DbSet<JobSeekersDTO> JobSeekers { get; set; }
        public DbSet<JobsDTO> Jobs { get; set; }
        public DbSet<HirerDTO> Hirers { get; set; }
        public DbSet<SpecifiedJobsDTO> SpecifiedJobs { get; set; }
        public DbSet<UserRolesDTO> UserRoles { get; set; }
        public DbSet<JobPostDTO> JobPosts { get; set; }
        public DbSet<JobAppliedDTO> JobApplieds { get; set; }

    }
}