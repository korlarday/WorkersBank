using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkersBank.Areas.Admin.Models;
using WorkersBank.Models.Data;

namespace WorkersBank.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class DashboardController : Controller
    {
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            //DashboardVM model = new DashboardVM();
            using(Db db = new Db())
            {
                // fetch the number of customers
                ViewBag.NumberOfCustomers = db.Users.Count();

                // fetch the number of hirers
                ViewBag.NumberOfHirer = db.Users.Where(x => x.UserType == "Hirer").Count();

                // fetch the number of job seekers
                ViewBag.NumberOfSeeker = db.Users.Where(x => x.UserType == "Seeker").Count();

            }
            return View();
        }

        // GET: Admin/Dashoard/UserInfoPartial
        public ActionResult UserInfoPartial()
        {
            // Get name
            string username = User.Identity.Name;

            // Declare model
            UserNavPartialVM model;

            using (Db db = new Db())
            {
                // Get the user
                UsersDTO dto = db.Users.SingleOrDefault(x => x.Username == username);

                // Build the model
                model = new UserNavPartialVM()
                {
                    Id = dto.Id,
                    FirstName = dto.Firstname,
                    LastName = dto.Surname,
                    Username = dto.Username,
                    PassportName = dto.PassportName,
                    Email = dto.Email
                };
            }

            // Return partial view with model
            return PartialView(model);
        }
    }
}