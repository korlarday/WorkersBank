using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WorkersBank.Models.Data;
using WorkersBank.Models.ViewModel;

namespace WorkersBank.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            SearchVM model = new SearchVM();
            using(Db db = new Db())
            {
                // Fetch all the list
                model.Jobs = new SelectList(db.Jobs.ToList(), "Id", "JobName");
            }
            return View(model);
        }

        [ActionName("search-applicants")]
        public ActionResult SearchApplicants(int? page, SearchVM model)
        {
            var pageNumber = page ?? 1;

            List<UsersVM> users;
            List<UsersVM> filteredUsers;

            using (Db db = new Db())
            {
                // Fetch all the list
                users = db.Users.ToArray()
                        .Where(x => x.UserType == "Seeker")
                        .Where(x => model.Name == null || (x.Firstname + " " + x.Surname).ToLower().Contains(model.Name.ToLower()))
                        .Where(x => model.Location == null || x.Address.ToLower().Contains(model.Location.ToLower()))
                        .Where(x => model.Gender == null || x.Gender == model.Gender)
                    .Select(x => new UsersVM(x)).ToList();

                filteredUsers = users;

                if(!String.IsNullOrEmpty(model.Qualification))
                {
                    List<int> ids = new List<int>();
                    foreach (var item in users)
                    {
                        ids.Add(item.Id);
                    }
                    filteredUsers = new List<UsersVM>();

                    List<JobSeekersDTO> seekers = db.JobSeekers.ToList();
                    foreach (var item in seekers)
                    {
                        if (item.Qualification.ToLower().Contains(model.Qualification.ToLower()))
                        {
                            if (ids.Contains(item.UserId))
                            {
                                UsersVM userVm = new UsersVM(item.User);
                                filteredUsers.Add(userVm);
                            }
                        }

                    }
                }

                if(model.Job != 0)
                {
                    List<SeekerJobVM> seekerJobs = db.SeekerJobs.Where(x => x.JobId == model.Job).ToArray().Select(x => new SeekerJobVM(x)).ToList();
                    List<int> ids = new List<int>();
                    foreach (var item in filteredUsers)
                    {
                        ids.Add(item.Id);
                    }
                    filteredUsers = new List<UsersVM>();

                    foreach (var frag in seekerJobs)
                    {
                        if (ids.Contains(frag.User.Id))
                        {
                            filteredUsers.Add(frag.User);
                        }
                    }
                }
            }

            ViewBag.ResultNumber = filteredUsers.Count();

            // Set pagination
            var onePageOfApplicants = filteredUsers.ToPagedList(pageNumber, 20);
            ViewBag.OnePageOfApplicants = onePageOfApplicants;

            return View("SearchApplicants",filteredUsers);
        }

        [ActionName("view-profile")]
        public ActionResult ViewProfile(string slug)
        {
            ViewProfileVM model = new ViewProfileVM();

            using (Db db = new Db())
            {
                // fetch the user
                UsersDTO user = db.Users.Where(x => x.Username == slug).SingleOrDefault();
                if (user == null)
                {
                    return Redirect("/Home");
                }

                // push the user to the model
                model.User = new UsersVM(user);

                // fetch the list of jobs he chose
                model.SeekerJobs = db.SeekerJobs.Where(x => x.UserId == user.Id).ToArray().Select(x => new SeekerJobVM(x)).ToList();

                // fetch the career details
                JobSeekersDTO jobSeekers = db.JobSeekers.Where(x => x.UserId == user.Id).SingleOrDefault();
                if (jobSeekers != null)
                {
                    model.JobSeeker = new JobSeekersVM(jobSeekers);
                }

                // fetch the hirer details
                HirerDTO hirer = db.Hirers.Where(x => x.UserId == user.Id).SingleOrDefault();
                if (hirer != null)
                {
                    model.Hirer = new HirerVM(hirer);
                }
            }
            return View("ViewProfile", model);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendMail(MailsVM model)
        {

            if (!ModelState.IsValid)
            {
                TempData["SM"] = "All fields are required.";
                return RedirectToAction("view-profile", new { slug = model.Username });
            }

            using (Db db = new Db())
            {

                // fetch the reciever
                UsersDTO recieverr = db.Users.Where(x => x.Username == model.Username).SingleOrDefault();

                string slug = model.NameOfCompany.Replace(" ", "-") + "-" + DateTime.Now.ToString().Replace(" ", "-").Replace("/", "-").Replace(":", "-");

                // store the jop post as private
                JobPostDTO jobPost = new JobPostDTO()
                {
                    NameOfOrganisation = model.NameOfCompany,
                    Address = model.Address,
                    Description = model.Description,
                    CreatedBy = 2,
                    TimeCreated = DateTime.Now,
                    NumberApplied = 1,
                    Slug = slug,
                    Type = "Private"
                };
                db.JobPosts.Add(jobPost);
                db.SaveChanges();

                // store the jop applied
                JobAppliedDTO jobApplied = new JobAppliedDTO()
                {
                    JobPostId = jobPost.Id,
                    UserId = recieverr.Id,
                    TimeApplied = DateTime.Now,
                    InvitationSent = true,
                    UserRespond = false,
                    Accept = false,
                    Reject = false,
                    TimeSent = DateTime.Now,
                    Subject = model.Subject,
                    Message = model.Message
                };
                db.JobApplieds.Add(jobApplied);
                db.SaveChanges();

                // store the mail
                MailsDTO newMail = new MailsDTO()
                {
                    Subject = model.Subject,
                    Message = model.Message,
                    NameOfCompany = model.NameOfCompany,
                    Address = model.Address,
                    Description = model.Description,
                    SentBy = 0,
                    Recipient = recieverr.Id,
                    TimeSent = DateTime.Now,
                    Type = "Private"
                };
                db.Mails.Add(newMail);
                db.SaveChanges();


                string senderName = model.Name;
                var theMessage = model.Message;
                theMessage += "<p>Name of Organisation: " + model.NameOfCompany + "</p>";
                theMessage += "<p>Address:</p>" + model.Address;
                theMessage += "<p>Description of job:</p>" + model.Description;

                // Update the job applied
                // change the applicant status to sent
                JobAppliedDTO applicant = db.JobApplieds.Where(x => x.UserId == recieverr.Id && x.JobPostId == jobPost.Id).SingleOrDefault();
                applicant.InvitationSent = true;
                applicant.Subject = model.Subject;
                applicant.Message = theMessage;
                applicant.TimeSent = DateTime.Now;
                db.SaveChanges();

                var body = "<p>Email From: {0} </p><p>Message:</p><p>{1}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress(recieverr.Email));
                //message.From = new MailAddress("korlarday47@yahoo.com");
                message.Subject = model.Subject;
                message.Body = string.Format(body, senderName, theMessage);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(message);
                }
            }
            TempData["SM"] = "Your mail was sent successfully";
            return RedirectToAction("view-profile", new { slug = model.Username });

        }
    }
}