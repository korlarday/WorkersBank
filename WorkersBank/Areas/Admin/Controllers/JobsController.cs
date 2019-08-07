using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkersBank.Models.Data;
using WorkersBank.Models.ViewModel;

namespace WorkersBank.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class JobsController : Controller
    {
        // GET: Admin/Jobs
        public ActionResult Index()
        {
            List<JobsVM> jobs;
            using(Db db = new Db())
            {
                jobs = db.Jobs.ToArray().Select(x => new JobsVM(x)).ToList();
            }
            return View(jobs);
        }

        // POST: Admin/Jobs/AddNewJob
        [HttpPost]
        public JsonResult AddNewJob(string jobName)
        {
            JobsVM newJob;
            using (Db db = new Db())
            {
                // Check if the deptName is unique
                if (db.Jobs.Any(x => x.JobName == jobName))
                {
                    newJob = new JobsVM();
                    newJob.JobName = "taken";
                    newJob.Id = 0;
                    return Json(newJob);
                }


                newJob = new JobsVM();
                newJob.JobName = jobName;

                JobsDTO dto = new JobsDTO()
                {
                    JobName = jobName
                };
                db.Jobs.Add(dto);
                db.SaveChanges();
                newJob.Index = db.Jobs.Count();
                newJob.Id = dto.Id;
            }

            return Json(newJob);
        }

        // POST: /Admin/Jobs/EditJob
        [HttpPost]
        public JsonResult EditJob(string jobName, int editId)
        {
            JobsVM newJob;
            int jobId = editId;
            using (Db db = new Db())
            {
                // Check if the deptName is unique
                if (db.Jobs.Where(x => x.Id != jobId).Any(x => x.JobName == jobName))
                {
                    newJob = new JobsVM();
                    newJob.JobName = "taken";
                    newJob.Id = 0;
                    return Json(newJob);
                }



                JobsDTO dto = db.Jobs.Find(jobId);
                dto.JobName = jobName;
                db.SaveChanges();

                newJob = new JobsVM()
                {
                    JobName = jobName,
                    Id = jobId
                };
            }

            return Json(newJob);
        }

        //Post: /Admin/Jobs/DeleteJob
        public String DeleteJob(int id)
        {
            using (Db db = new Db())
            {
                // first check other table for the job
                List<SeekerJobDTO> jobs = db.SeekerJobs.Where(x => x.JobId == id).ToList();
                foreach (var item in jobs)
                {
                    db.SeekerJobs.Remove(item);
                    db.SaveChanges();
                }

                JobsDTO dto = db.Jobs.Find(id);
                db.Jobs.Remove(dto);
                db.SaveChanges();
            }

            return "success";
        }
    }
}