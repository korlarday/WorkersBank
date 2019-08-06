using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkersBank.Models.Data;
using WorkersBank.Models.ViewModel;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

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
        [ActionName("view-applicants")]
        public ActionResult ViewApplicants(string slug)
        {
            ViewApplicantsVM model = new ViewApplicantsVM();
            using(Db db = new Db())
            {
                // fetch the user
                string username = User.Identity.Name;
                UsersDTO user = db.Users.Where(x => x.Username == username).SingleOrDefault();

                // fetch hirer info
                HirerDTO hirer = db.Hirers.Where(x => x.UserId == user.Id).SingleOrDefault();
                model.Hirer = new HirerVM(hirer);

                // fetch the job post 
                JobPostDTO jobPost = db.JobPosts.Where(x => x.Slug == slug && x.CreatedBy == user.Id).SingleOrDefault();
                if(jobPost == null)
                {
                    return RedirectToAction("job-slots-created");
                }

                // then fetch the applicants
                model.Applicants = db.JobApplieds.Where(x => x.JobPostId == jobPost.Id).ToArray().Select(x => new JobAppliedVM(x)).ToList();
                model.Job = new JobPostVM(jobPost);
            }
            return View("ViewApplicants", model);
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
                    TimeApplied = DateTime.Now,
                    InvitationSent = false,
                    UserRespond = false,
                    Accept = false,
                    Reject = false,
                };
                db.JobApplieds.Add(newJobApplied);
                db.SaveChanges();

                jobPost.NumberApplied += 1;
                db.SaveChanges();
            }
            TempData["SM"] = "You have successfully applied for this job. Please wait for job invitation mail";
            return RedirectToAction("view-post", new { slug = slug });
        }

        [Authorize]
        [ActionName("view-profile")]
        public ActionResult ViewProfile(string slug)
        {
            ViewProfileVM model = new ViewProfileVM();

            using (Db db = new Db())
            {
                // fetch the user
                UsersDTO user = db.Users.Where(x => x.Username == slug).SingleOrDefault();
                if(user == null)
                {
                    return Redirect("/Home");
                }

                // push the user to the model
                model.User = new UsersVM(user);

                // fetch the list of jobs he chose
                model.SeekerJobs = db.SeekerJobs.Where(x => x.UserId == user.Id).ToArray().Select(x => new SeekerJobVM(x)).ToList();

                // fetch the career details
                JobSeekersDTO jobSeekers = db.JobSeekers.Where(x => x.UserId == user.Id).SingleOrDefault();
                if(jobSeekers != null)
                {
                    model.JobSeeker = new JobSeekersVM(jobSeekers);
                }

                // fetch the hirer details
                HirerDTO hirer = db.Hirers.Where(x => x.UserId == user.Id).SingleOrDefault();
                if(hirer != null)
                {
                    model.Hirer = new HirerVM(hirer);
                }
            }
            return View("ViewProfile", model);
        }

        [Authorize]
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendMail(MailsVM model)
        {

            if (!ModelState.IsValid)
            {
                TempData["SM"] = "All fields are required";
                return RedirectToAction("view-applicants", new { slug = model.Slug });
            }

            using(Db db = new Db())
            {
                // fetch the sender
                string username = User.Identity.Name;
                UsersDTO sender = db.Users.Where(x => x.Username == username).SingleOrDefault();

                // fetch the reciever
                UsersDTO recieverr = db.Users.Where(x => x.Username == model.Username).SingleOrDefault();

                // check if message was saved
                if (model.SaveMessage)
                {
                    HirerDTO hirer = db.Hirers.Where(x => x.UserId == sender.Id).SingleOrDefault();
                    hirer.Message = model.Message;
                    hirer.Subject = model.Subject;
                    hirer.NameOfOrganisation = model.NameOfCompany;
                    hirer.Address = model.Address;
                    hirer.Description = model.Description;
                    db.SaveChanges();
                }

                // store the mail
                MailsDTO newMail = new MailsDTO()
                {
                    Subject = model.Subject,
                    Message = model.Message,
                    NameOfCompany = model.NameOfCompany,
                    Address = model.Address,
                    Description = model.Description,
                    SentBy = sender.Id,
                    Recipient = recieverr.Id,
                    TimeSent = DateTime.Now
                };
                db.Mails.Add(newMail);
                db.SaveChanges();

                // fetch the job
                JobPostDTO job = db.JobPosts.Where(x => x.Slug == model.Slug).SingleOrDefault();


                string senderName = sender.Firstname + " " + sender.Surname;
                var theMessage = model.Message;
                theMessage += "<p>Name of Organisation: " + model.NameOfCompany + "</p>";
                theMessage += "<p>Address:</p>" + model.Address;
                theMessage += "<p>Description of job:</p>" + model.Description;

                // Update the job applied
                // change the applicant status to sent
                JobAppliedDTO applicant = db.JobApplieds.Where(x => x.UserId == recieverr.Id && x.JobPostId == job.Id).SingleOrDefault();
                applicant.InvitationSent = true;
                applicant.Subject = model.Subject;
                applicant.Message = theMessage;
                applicant.TimeSent = DateTime.Now;
                db.SaveChanges();

                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress(recieverr.Email));
                message.From = new MailAddress("korlarday47@yahoo.com");
                message.Subject = model.Subject;
                message.Body = string.Format(body, senderName, sender.Email, theMessage);
                message.IsBodyHtml = true;

                using(var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(message);
                }
            }
            TempData["SM"] = "Your mail was sent successfully";
            return RedirectToAction("view-applicants", new { slug = model.Slug });
        }

        [Authorize(Roles = "Seeker")]
        public ActionResult InvitationPartial()
        {
            // Get name
            string username = User.Identity.Name;

            // Declare model
            InvitationPartialVM model;

            using (Db db = new Db())
            {
                // Get the user
                UsersDTO user = db.Users.SingleOrDefault(x => x.Username == username);

                // count invitations
                int MessageNum = db.JobApplieds.Where(x => x.UserId == user.Id && x.UserRespond == false).Count();

                // Build the model
                model = new InvitationPartialVM()
                {
                    InvitationCount = MessageNum
                };
            }

            // Return partial view with model
            return PartialView(model);
        }

        [Authorize(Roles = "Seeker")]
        public ActionResult Invitations()
        {
            // declare the model
            List<JobAppliedVM> model;

            using(Db db = new Db())
            {
                // fetch the user
                string username = User.Identity.Name;
                UsersDTO user = db.Users.Where(x => x.Username == username).SingleOrDefault();

                // fetch all the invitations
                model = db.JobApplieds.Where(x => x.UserId == user.Id && x.InvitationSent == true).ToArray().Select(x => new JobAppliedVM(x)).ToList();

            }
            return View(model);
        }

        [Authorize(Roles = "Seeker")]
        [HttpPost]
        public string AcceptInvitation(int id)
        {
            using(Db db = new Db())
            {
                //fetch the user
                string username = User.Identity.Name;
                UsersDTO user = db.Users.Where(x => x.Username == username).SingleOrDefault();

                // fetch the application form
                JobAppliedDTO job = db.JobApplieds.Where(x => x.UserId == user.Id && x.Id == id).SingleOrDefault();

                job.UserRespond = true;
                job.Accept = true;
                db.SaveChanges();
            }
            TempData["SM"] = "Job invitation has been accepted successfully";
            return "success";
        }

        [Authorize(Roles = "Seeker")]
        [HttpPost]
        public string RejectInvitation(int id)
        {
            using (Db db = new Db())
            {
                //fetch the user
                string username = User.Identity.Name;
                UsersDTO user = db.Users.Where(x => x.Username == username).SingleOrDefault();

                // fetch the application form
                JobAppliedDTO job = db.JobApplieds.Where(x => x.UserId == user.Id && x.Id == id).SingleOrDefault();

                job.UserRespond = true;
                job.Reject = true;
                db.SaveChanges();
            }
            TempData["SM"] = "Job invitation has been rejected successfully";
            return "success";
        }
    }
}