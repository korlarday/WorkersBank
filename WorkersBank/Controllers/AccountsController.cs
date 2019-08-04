using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using WorkersBank.Models.Data;
using WorkersBank.Models.ViewModel;

namespace WorkersBank.Controllers
{

    public class AccountsController : Controller
    {
        // GET: Accounts
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        // GET: Login
        public ActionResult Login()
        {
            // Confirm user is not logged in

            string username = User.Identity.Name;

            if (!string.IsNullOrEmpty(username))
                return Redirect("/Home");

            LoginVM model = new LoginVM();

            return View(model);
        }

        // POST: Account/Login
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(LoginVM model)
        {
            // Check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using(Db db = new Db())
            {
                // Fetch the user with the username
                UsersDTO user = db.Users.Where(x => x.Username == model.Username).SingleOrDefault();
                if(user == null)
                {
                    ModelState.AddModelError("", "Invalid Username or Password");
                    return View(model);
                }

                // Check if password matches
                bool validPassword = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);
                if (!validPassword)
                {
                    ModelState.AddModelError("", "Invalid Username or Password");
                    return View(model);
                }


            }
            
            TempData["SM"] = "Welcome back " + model.Username + "!";
            FormsAuthentication.SetAuthCookie(model.Username, model.RememberMe);
            return Redirect(FormsAuthentication.GetRedirectUrl(model.Username, model.RememberMe));
            
        }

        // GET: /Account/Logout
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/Home");
        }

        // GET: RegisterAsSeeker
        [ActionName("register-as-seeker")]
        public ActionResult RegisterAsSeeker()
        {
            // Confirm user is not logged in

            string username = User.Identity.Name;

            if (!string.IsNullOrEmpty(username))
                return Redirect("/Home");

            RegisterSeeker model = new RegisterSeeker();
            model.DOB = new DateTime(2000, 01, 01);
            model.Gender = "Male";
            return View("RegisterAsSeeker", model);
        }

        // POST: RegisterAsSeeker
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("register-as-seeker")]
        public ActionResult RegisterAsSeeker(RegisterSeeker model)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                return View("RegisterAsSeeker", model);
            }

            // Check if the password does not match
            if (!model.Password.Equals(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "Password do not match");
                return View("RegisterAsSeeker", model);
            }

            using(Db db = new Db())
            {
                // check if the username is unique
                if(db.Users.Any(x => x.Username == model.Username))
                {
                    ModelState.AddModelError("", "Username already taken");
                    return View("RegisterAsSeeker", model);
                }
                // check for spaces in the username
                model.Username = model.Username.Replace(" ", "-");

                // Check if email is already used
                if(db.Users.Any(x => x.Email == model.Email))
                {
                    ModelState.AddModelError("", "The email address exists");
                    return View("RegisterAsSeeker", model);
                }

                // Store the User
                UsersDTO newUser = new UsersDTO();
                newUser.Username = model.Username;
                newUser.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
                newUser.UserType = "Seeker";
                newUser.Firstname = model.Firstname;
                newUser.Middlename = model.Middlename;
                newUser.Surname = model.Surname;
                newUser.Gender = model.Gender;
                newUser.DOB = model.DOB;
                newUser.PhoneNumber = model.PhoneNumber;
                newUser.Email = model.Email;
                newUser.StateOfOrigin = model.StateOfOrigin;
                db.Users.Add(newUser);
                db.SaveChanges();

                // Store the User role as Seeker
                UserRolesDTO userRole = new UserRolesDTO()
                {
                    UserId = newUser.Id,
                    RoleId = 1
                };
                db.UserRoles.Add(userRole);
                db.SaveChanges();
            }

            FormsAuthentication.SetAuthCookie(model.Username, false);
            return Redirect(FormsAuthentication.GetRedirectUrl(model.Username, false));

        }

        // GET: RegisterAsHirer
        [ActionName("register-as-hirer")]
        public ActionResult RegisterAsHirer()
        {
            // Confirm user is not logged in

            string username = User.Identity.Name;

            if (!string.IsNullOrEmpty(username))
                return Redirect("/Home");

            RegisterHirer model = new RegisterHirer();
            model.DOB = new DateTime(2000, 01, 01);
            model.Gender = "Male";
            return View("RegisterAsHirer", model);
        }

        // POST: RegisterAsHirer
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [ActionName("register-as-hirer")]
        public ActionResult RegisterAsHirer(RegisterHirer model)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                return View("RegisterAsHirer", model);
            }

            // Check if the password does not match
            if (!model.Password.Equals(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "Password do not match");
                return View("RegisterAsHirer", model);
            }

            using (Db db = new Db())
            {
                // check if the username is unique
                if (db.Users.Any(x => x.Username == model.Username))
                {
                    ModelState.AddModelError("", "Username already taken");
                    return View("RegisterAsHirer", model);
                }
                // check for spaces in the username
                model.Username = model.Username.Replace(" ", "-");

                // Check if email is already used
                if (db.Users.Any(x => x.Email == model.Email))
                {
                    ModelState.AddModelError("", "The email address exists");
                    return View("RegisterAsHirer", model);
                }

                // Store the User
                UsersDTO newUser = new UsersDTO();
                newUser.Username = model.Username;
                newUser.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
                newUser.UserType = "Hirer";
                newUser.Firstname = model.Firstname;
                newUser.Middlename = model.Middlename;
                newUser.Surname = model.Surname;
                newUser.Gender = model.Gender;
                newUser.DOB = model.DOB;
                newUser.PhoneNumber = model.PhoneNumber;
                newUser.Email = model.Email;
                newUser.StateOfOrigin = model.StateOfOrigin;
                db.Users.Add(newUser);
                db.SaveChanges();

                // Store the company details
                HirerDTO newHirer = new HirerDTO();
                newHirer.UserId = newUser.Id;
                newHirer.NameOfOrganisation = model.NameOfOrganization;
                newHirer.Address = model.Address;
                newHirer.Description = model.Description;
                db.Hirers.Add(newHirer);
                db.SaveChanges();

                // Store the User role as Hirer
                UserRolesDTO userRole = new UserRolesDTO()
                {
                    UserId = newUser.Id,
                    RoleId = 2
                };
                db.UserRoles.Add(userRole);
                db.SaveChanges();
            }

            FormsAuthentication.SetAuthCookie(model.Username, false);
            return Redirect(FormsAuthentication.GetRedirectUrl(model.Username, false));

        }

        // GET: MyProfile
        [ActionName("my-profile")]
        [Authorize]
        public ActionResult MyProfile()
        {
            string username = User.Identity.Name;
            if(username == "")
            {
                return Redirect("/Accounts/Login");
            }

            ProfileInfo myprofile = new ProfileInfo();
            myprofile.JobSeeker = new JobSeekersVM();
            myprofile.Hirer = new HirerVM();
            myprofile.Jobs = new List<SeekerJobVM>();

            using(Db db = new Db())
            {
                UsersDTO user = db.Users.Where(x => x.Username == username).SingleOrDefault();
                myprofile.User = new UsersVM(user);

                JobSeekersDTO jobSeeker = db.JobSeekers.Where(x => x.UserId == user.Id).SingleOrDefault();
                if(jobSeeker != null)
                {
                    myprofile.JobSeeker = new JobSeekersVM(jobSeeker);
                }

                if(user.UserType == "Seeker")
                {
                    myprofile.Jobs = db.SeekerJobs.Where(x => x.UserId == user.Id).ToArray().Select(x => new SeekerJobVM(x)).ToList();
                }

                if(user.UserType == "Hirer")
                {
                    HirerDTO hirer = db.Hirers.Where(x => x.UserId == user.Id).SingleOrDefault();
                    myprofile.Hirer = new HirerVM(hirer);
                }
            }
            return View("MyProfile", myprofile);
        }

        // POST: ChangePassport
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult ChangePassport(ProfileInfo model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("my-profile");
            }

            // Fetch the user
            using(Db db = new Db())
            {
                string username = User.Identity.Name;

                UsersDTO user = db.Users.Where(x => x.Username == username).SingleOrDefault();
                int id = user.Id;

                #region Upload Passport

                // Create necessary directories
                var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

                var pathString1 = Path.Combine(originalDirectory.ToString(), "Passports");
                var pathString2 = Path.Combine(originalDirectory.ToString(), "Passports\\" + id.ToString());
                var pathString3 = Path.Combine(originalDirectory.ToString(), "Passports\\" + id.ToString() + "\\Thumbs");

                if (!Directory.Exists(pathString1))
                    Directory.CreateDirectory(pathString1);

                if (!Directory.Exists(pathString2))
                    Directory.CreateDirectory(pathString2);

                if (!Directory.Exists(pathString3))
                    Directory.CreateDirectory(pathString3);


                // Check if a file was uploaded
                if (model.Passport != null && model.Passport.ContentLength > 0)
                {
                    // Get file extension
                    string ext = model.Passport.ContentType.ToLower();

                    // Verify extension
                    if (ext == "image/jpg" ||
                        ext == "image/jpeg" ||
                        ext == "image/pjpeg" ||
                        ext == "image/gif" ||
                        ext == "image/x-png" ||
                        ext == "image/png")
                    {

                        //  Init image name
                        string imageName = model.Passport.FileName;

                        // Save image name to DTO

                        user.PassportName = imageName;

                        db.SaveChanges();

                        // Set original and thumb image path
                        var path = string.Format("{0}\\{1}", pathString2, imageName);
                        var path2 = string.Format("{0}\\{1}", pathString3, imageName);

                        //  Save original
                        model.Passport.SaveAs(path);

                        // Create and save thumb
                        WebImage img = new WebImage(model.Passport.InputStream);
                        img.Resize(150, 150);
                        img.Save(path2);
                    }
                }
                #endregion
            }

            TempData["SM"] = "Passport changed successfully";
            return RedirectToAction("my-profile");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult UpdateBasicInformation(UsersVM model)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/Home");
            }
            
            using(Db db = new Db())
            {
                // fetch the user
                string username = User.Identity.Name;

                UsersDTO user = db.Users.Where(x => x.Username == username).SingleOrDefault();
                
                // Check if Username is still unique
                if(db.Users.Any(x => x.Id != user.Id && x.Username == model.Username))
                {
                    TempData["SM"] = "Sorry, the specified username is already taken";
                    return RedirectToAction("my-profile");
                }

                // Check if email is still unique
                if (db.Users.Any(x => x.Id != user.Id && x.Email == model.Email))
                {
                    TempData["SM"] = "Sorry, the specified email is already in use";
                    return RedirectToAction("my-profile");
                }

                // check if user enters a new username
                if (model.Username != user.Username)
                {
                    FormsAuthentication.SetAuthCookie(model.Username, true);
                }

                // store the updated infor
                user.Username = model.Username;
                user.Firstname = model.Firstname;
                user.Middlename = model.Middlename;
                user.Surname = model.Surname;
                user.Gender = model.Gender;
                user.DOB = model.DOB;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.StateOfOrigin = model.StateOfOrigin;
                db.SaveChanges();

                
            }
            return RedirectToAction("my-profile");
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Seeker")]
        public ActionResult UpdateCareerInformation(JobSeekersVM model)
        {
            using (Db db = new Db())
            {
                // Fetch the user
                string username = User.Identity.Name;

                UsersDTO user = db.Users.Where(x => x.Username == username).SingleOrDefault();

                // Check if User has info or Id in jobseeker table
                if(db.JobSeekers.Any(x => x.UserId == user.Id))
                {
                    JobSeekersDTO seeker = db.JobSeekers.Where(x => x.UserId == user.Id).SingleOrDefault();
                    seeker.Qualification = model.Qualification;
                    seeker.LastInstitution = model.LastInstitution;
                    seeker.WorkExperience = model.WorkExperience;
                    seeker.SkillsAcquired = model.SkillsAcquired;
                    db.SaveChanges();
                }
                else
                {
                    JobSeekersDTO newSeeker = new JobSeekersDTO()
                    {
                        UserId = user.Id,
                        Qualification = model.Qualification,
                        LastInstitution = model.LastInstitution,
                        WorkExperience = model.WorkExperience,
                        SkillsAcquired = model.SkillsAcquired
                    };
                    db.JobSeekers.Add(newSeeker);
                    db.SaveChanges();
                }
                return RedirectToAction("my-profile");
            }
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Hirer")]
        public ActionResult UpdateCompanyInformation(HirerVM model)
        {
            using (Db db = new Db())
            {
                // fetch the user
                string username = User.Identity.Name;

                UsersDTO user = db.Users.Where(x => x.Username == username).SingleOrDefault();

                // Fetch the company
                HirerDTO hirer = db.Hirers.Where(x => x.UserId == user.Id).SingleOrDefault();

                hirer.NameOfOrganisation = model.NameOfOrganisation;
                hirer.Address = model.Address;
                hirer.Description = model.Description;
                db.SaveChanges();
            }
            return RedirectToAction("my-profile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Seeker")]
        public ActionResult UpdateJobInformation(int[] jobs)
        {
            using(Db db = new Db())
            {
                // fetch the user
                string username = User.Identity.Name;
                UsersDTO user = db.Users.Where(x => x.Username == username).SingleOrDefault();
                int userId = user.Id;

                // delete existing jobs if found
                List<SeekerJobDTO> existJobs = db.SeekerJobs.Where(x => x.UserId == userId).ToList();
                if (existJobs.Any())
                {
                    foreach (var job in existJobs)
                    {
                        db.SeekerJobs.Remove(job);
                        db.SaveChanges();
                    }
                }

                // loop through the checked items and save
                foreach (var item in jobs)
                {
                    SeekerJobDTO job = new SeekerJobDTO()
                    {
                        UserId = userId,
                        JobId = item
                    };
                    db.SeekerJobs.Add(job);
                    db.SaveChanges();
                }
            }
            TempData["SM"] = "Your jobs profile has been updated successfully";
            return RedirectToAction("my-profile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Seeker")]
        public ActionResult RecommendJobs(string[] jobName)
        {
            using(Db db = new Db())
            {
                // Fetch the user
                string username = User.Identity.Name;
                UsersDTO user = db.Users.Where(x => x.Username == username).SingleOrDefault();
                int userId = user.Id;

                // List through the jobname list
                foreach (var item in jobName)
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        // check if the name has not been suggested before
                        if(!db.SpecifiedJobs.Any(x => x.JobName == item))
                        {
                            SpecifiedJobsDTO newJob = new SpecifiedJobsDTO()
                            {
                                UserId = userId,
                                JobName = item
                            };
                            db.SpecifiedJobs.Add(newJob);
                            db.SaveChanges();
                        }
                    }
                }

            }
            TempData["SM"] = "Your Job recommendations has been sent successfully";
            return RedirectToAction("my-profile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Seeker")]
        public ActionResult UploadCV(HttpPostedFileBase CV)
        {

            // Fetch the user
            using (Db db = new Db())
            {
                string username = User.Identity.Name;

                UsersDTO user = db.Users.Where(x => x.Username == username).SingleOrDefault();
                int id = user.Id;

                #region Upload Passport

                // Create necessary directories
                var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

                var pathString1 = Path.Combine(originalDirectory.ToString(), "CVs");
                var pathString2 = Path.Combine(originalDirectory.ToString(), "CVs\\" + id.ToString());

                if (!Directory.Exists(pathString1))
                    Directory.CreateDirectory(pathString1);

                if (!Directory.Exists(pathString2))
                    Directory.CreateDirectory(pathString2);



                // Check if a file was uploaded
                if (CV != null && CV.ContentLength > 0)
                {
                    // Check if a file has been uploaded before
                    if (!String.IsNullOrEmpty(user.CV))
                    {
                        string existPath = Request.MapPath("~/Images/Uploads/CVs/" + user.Id + "/" + user.CV);
                        if (System.IO.File.Exists(existPath))
                        {
                            System.IO.File.Delete(existPath);
                        }
                    }
                    // Get file extension
                    string ext = CV.ContentType.ToLower();

                    //  Init image name
                    string fileName = CV.FileName;

                    // Save image name to DTO
                    user.CV = fileName;
                    db.SaveChanges();

                    // Set original and thumb image path
                    var path = string.Format("{0}\\{1}", pathString2, fileName);

                    //  Save original
                    CV.SaveAs(path);

                        
                }
                #endregion
            }

            TempData["SM"] = "CV uploaded successfully";
            return RedirectToAction("my-profile");
        }

        public FileResult DownloadCV(string slug)
        {
            using (Db db = new Db())
            {
                // fetch the user
                UsersDTO user = db.Users.Where(x => x.Username == slug).SingleOrDefault();

                var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
                var path1 = Path.Combine(originalDirectory.ToString(), "CVs\\" + user.Id.ToString() + "\\" + user.CV);

                byte[] fileBytes = System.IO.File.ReadAllBytes(path1);
                string fileName = user.CV;

                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
        }

        public JsonResult FetchStates()
        {
            List<StateData> states = new List<StateData>();
            using (StreamReader sr = new StreamReader(Server.MapPath("~/Content/States.json")))
            {
                states = JsonConvert.DeserializeObject<List<StateData>>(sr.ReadToEnd());
            }
            return this.Json(states, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FetchJobs()
        {
            FetchJobs fetchJobs = new FetchJobs();
            fetchJobs.JobsSelected = new List<int>();

            using (Db db = new Db())
            {
                // Fetch all the jobs
                fetchJobs.Jobs = db.Jobs.ToArray().Select(x => new JobsVM(x)).ToList();

                // Fetch the user
                string username = User.Identity.Name;
                UsersDTO user = db.Users.Where(x => x.Username == username).SingleOrDefault();

                // Fetch all the selected jobs
                List<SeekerJobDTO> selects = db.SeekerJobs.Where(x => x.UserId == user.Id).ToList();

                // if you have selected before, then add it to jobselected List
                if (selects.Any())
                {
                    foreach (var item in selects)
                    {
                        fetchJobs.JobsSelected.Add(item.JobId);
                    }
                }
            }
            return Json(fetchJobs, JsonRequestBehavior.AllowGet);
        }


    }
}