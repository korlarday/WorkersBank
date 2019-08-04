using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkersBank.Models.Data;
using WorkersBank.Models.ViewModel;

namespace WorkersBank.Controllers
{
    public class ApplicantsController : Controller
    {
        // GET: Applicants
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("search-applicants")]
        public ActionResult SearchApplicants()
        {
            return View("SearchApplicants");
        }

        [Authorize(Roles = "Hirer")]
        [ActionName("job-slots-created")]
        public ActionResult CreatedJobSlots()
        {
            List<JobPostVM> jobPosts = new List<JobPostVM>();
            using(Db db = new Db())
            {
                // Fetch the user
                string username = User.Identity.Name;
                UsersDTO user = db.Users.Where(x => x.Username == username).SingleOrDefault();

                // Fetch all the job post related to this user
                jobPosts = db.JobPosts.Where(x => x.CreatedBy == user.Id).ToArray().Select(x => new JobPostVM(x)).ToList();
            }
            return View("CreatedJobSlots", jobPosts);
        }

        [Authorize(Roles = "Hirer")]
        [ActionName("create-job-slot")]
        public ActionResult CreateJobSlot()
        {
            JobPostVM model = new JobPostVM();

            using(Db db = new Db())
            {
                // Fetch the user
                string username = User.Identity.Name;
                UsersDTO user = db.Users.Where(x => x.Username == username).SingleOrDefault();

                // Fetch the company info
                HirerDTO company = db.Hirers.Where(x => x.UserId == user.Id).SingleOrDefault();
                model.NameOfOrganisation = company.NameOfOrganisation;
                model.Address = company.Address;
            }
            return View("CreateJobSlot", model);
        }

        [Authorize(Roles = "Hirer")]
        [ActionName("create-job-slot")]
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult CreateJobSlot(JobPostVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateJobSlot", model);
            }

            using (Db db = new Db())
            {
                // fetch the user
                string username = User.Identity.Name;
                UsersDTO user = db.Users.Where(x => x.Username == username).SingleOrDefault();
                int userId = user.Id;

                string slug = model.NameOfOrganisation.Replace(" ", "-") + "-" + DateTime.Now.ToString().Replace(" ", "-").Replace("/", "-").Replace(":", "-");
                JobPostDTO newJob = new JobPostDTO()
                {
                    NameOfOrganisation = model.NameOfOrganisation,
                    Address = model.Address,
                    Description = model.Description,
                    CreatedBy = userId,
                    TimeCreated = DateTime.Now,
                    NumberApplied = 0,
                    Slug = slug
                };
                db.JobPosts.Add(newJob);
                db.SaveChanges();
            }

            TempData["SM"] = "New Job Application created successfully";

            return RedirectToAction("job-slots-created");
        }

        [Authorize]
        [ActionName("job-posts")]
        public ActionResult JobPosts()
        {
            List<JobPostVM> model;
            using(Db db = new Db())
            {
                model = db.JobPosts.OrderByDescending(x=> x.TimeCreated).ToArray().Select(x => new JobPostVM(x)).ToList();
            }
            return View("JobPosts", model);
        }

        [Authorize]
        [ActionName("view-post")]
        public ActionResult ViewPost(string slug)
        {
            JobPostVM model;
            using(Db db = new Db())
            {
                // Fetch the post
                JobPostDTO job = db.JobPosts.Where(x => x.Slug == slug).SingleOrDefault();
                if(job == null)
                {
                    return Redirect("/Home");
                }

                model = new JobPostVM(job);
            }
            return View("ViewPost", model);
        }

        [Authorize(Roles = "Seeker")]
        [ActionName("apply-for-job")]
        public ActionResult ApplyForJob(string slug)
        {
            using(Db db = new Db())
            {
                // fetch the Job post
                JobPostDTO jobPost = db.JobPosts.Where(x => x.Slug == slug).SingleOrDefault();
                if(jobPost == null)
                {
                    return RedirectToAction("job-posts");
                }

                // fetch the user
                string username = User.Identity.Name;
                UsersDTO user = db.Users.Where(x => x.Username == username).SingleOrDefault();

                // Check if user has not applied before
                JobAppliedDTO jobApplied = db.JobApplieds.Where(x => x.UserId == user.Id && jobPost.Id == x.JobPostId).SingleOrDefault();
                if(jobApplied != null)
                {
                    TempData["SM"] = "Sorry, you have already applied for the job. Please wait for job invitation mail";
                    return RedirectToAction("view-post", new { slug = slug });
                }

                JobAppliedDTO newJobApplied = new JobAppliedDTO()
                {
                    JobPostId = jobPost.Id,
                    UserId = user.Id,
                    TimeApplied = DateTime.Now
                };
                db.JobApplieds.Add(newJobApplied);
                db.SaveChanges();

                jobPost.NumberApplied += 1;
                db.SaveChanges();
            }
            TempData["SM"] = "You have successfully applied for this job. Please wait for job invitation mail";
            return RedirectToAction("view-post", new { slug = slug });
        }
    }
}