using System.Linq;
using System.Web.Mvc;
using ProfileSelect.Models;
using ProfileSelect.ViewModels;

namespace ProfileSelect.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StatusController : Controller
    {
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(StatusViewModel statusViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(statusViewModel);
            }
            using (var dbCotext = new ApplicationDbContext())
            {
                dbCotext.Statuses.Add(new Status
                {
                    Name = statusViewModel.Name
                });
                dbCotext.SaveChanges();
                return RedirectToAction("Statuses", "Home", new { Area = "Admin" });
            }
        }

        public ActionResult Edit(int id)
        {
            using (var dbCotext = new ApplicationDbContext())
            {
                var direction = dbCotext.Statuses.First(d => d.Id == id);
                return View(new StatusViewModel
                {
                    Id = direction.Id,
                    Name = direction.Name
                });
            }
        }

        [HttpPost]
        public ActionResult Edit(StatusViewModel statusViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(statusViewModel);
            }
            using (var dbCotext = new ApplicationDbContext())
            {
                var subject = dbCotext.Statuses.First(d => d.Id == statusViewModel.Id);
                subject.Name = statusViewModel.Name;
                dbCotext.SaveChanges();
                return RedirectToAction("Statuses", "Home", new { Area = "Admin" });
            }
        }

        public ActionResult Delete(int id)
        {
            using (var dbCotext = new ApplicationDbContext())
            {
                var status = dbCotext.Statuses.First(d => d.Id == id);
                dbCotext.Statuses.Remove(status);
                dbCotext.SaveChanges();
                return RedirectToAction("Statuses", "Home", new { Area = "Admin" });
            }
        }
    }
}
