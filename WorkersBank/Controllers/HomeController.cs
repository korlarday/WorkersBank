using System;
using System.Collections.Generic;
using System.Linq;
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

        
    }
}