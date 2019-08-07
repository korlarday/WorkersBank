using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkersBank.Areas.Admin.Models;
using WorkersBank.Models.Data;
using WorkersBank.Models.ViewModel;

namespace WorkersBank.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomersController : Controller
    {
        // GET: Admin/Customers
        public ActionResult Index()
        {
            List<UsersVM> users;
            using(Db db = new Db())
            {
                users = db.Users.ToArray().Select(x => new UsersVM(x)).ToList();
            }
            return View(users);
        }


        [ActionName("manage-users")]
        public ActionResult ManageUsers()
        {
            // Declare your model
            ManageUsersVM model = new ManageUsersVM();
            model.Administrators = new List<UsersVM>();
            model.Nonadministrators = new List<UsersVM>();

            using (Db db = new Db())
            {
                // Fetch all the users
                List<UsersVM> customers = db.Users.ToArray().Select(x => new UsersVM(x)).ToList();

                List<int> adminIds = db.UserRoles.Where(x => x.RoleId == 3).Select(x => x.UserId).ToList();

                foreach (var item in customers)
                {
                    if (adminIds.Contains(item.Id))
                    {
                        model.Administrators.Add(item);
                    }
                    else
                    {
                        model.Nonadministrators.Add(item);
                    }
                }
            }

            return View("ManageUsers", model);
        }

        [ActionName("view-previledges")]
        public ActionResult ViewPreviledges(string slug)
        {
            // Declare your model
            AuthorizeVM model = new AuthorizeVM();

            using (Db db = new Db())
            {
                UsersDTO customer = db.Users.Where(x => x.Username == slug).SingleOrDefault();

                model.Customer = new UsersVM(customer);

                // fetch all roles
                List<int> roles = db.UserRoles.Where(x => x.UserId == customer.Id).Select(x => x.RoleId).ToList();

                if (roles.Contains(3))
                {
                    model.Admin = true;
                }
            }
            return View("ViewPreviledges", model);
        }

        [HttpPost]
        public string SetPrevideges(SetAuthorization model)
        {
            using (Db db = new Db())
            {

                // fetch all roles
                List<int> roles = db.UserRoles.Where(x => x.UserId == model.Id).Select(x => x.RoleId).ToList();


                if (model.Admin)
                {
                    if (!roles.Contains(3))
                    {
                        UserRolesDTO role = new UserRolesDTO();
                        role.RoleId = 3;
                        role.UserId = model.Id;
                        db.UserRoles.Add(role);
                        db.SaveChanges();
                    }
                }
                else
                {
                    if (roles.Contains(3))
                    {
                        UserRolesDTO role = db.UserRoles.Where(x => x.UserId == model.Id && x.RoleId == 3).SingleOrDefault();
                        db.UserRoles.Remove(role);
                        db.SaveChanges();
                    }
                }
            }
            TempData["SM"] = "Settings was saved successfully";
            return "success";
        }
    }
}