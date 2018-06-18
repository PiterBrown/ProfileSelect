using System.Linq;
using System.Web.Mvc;
using ProfileSelect.Models;
using ProfileSelect.ViewModels;

namespace ProfileSelect.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SubjectController : Controller
    {
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(SubjectViewModel subjectViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(subjectViewModel);
            }
            using (var dbCotext = new ApplicationDbContext())
            {
                dbCotext.Subjects.Add(new Subject
                {
                    Name = subjectViewModel.Name,
                    IsDeleted = false
                });
                dbCotext.SaveChanges();
                return RedirectToAction("Subjects", "Home", new { Area = "Admin" });
            }
        }

        public ActionResult Edit(int id)
        {
            using (var dbCotext = new ApplicationDbContext())
            {
                var direction = dbCotext.Subjects.First(d => d.Id == id);
                return View(new SubjectViewModel
                {
                    Id = direction.Id,
                    Name = direction.Name
                });
            }
        }

        [HttpPost]
        public ActionResult Edit(SubjectViewModel subjectViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(subjectViewModel);
            }
            using (var dbCotext = new ApplicationDbContext())
            {
                var subject = dbCotext.Subjects.First(d => d.Id == subjectViewModel.Id);
                subject.Name = subjectViewModel.Name;
                dbCotext.SaveChanges();
                return RedirectToAction("Subjects", "Home", new { Area = "Admin" });
            }
        }

        public ActionResult Delete(int id)
        {
            using (var dbCotext = new ApplicationDbContext())
            {
                var subject = dbCotext.Subjects.First(d => d.Id == id);
                subject.IsDeleted = true;
                dbCotext.SaveChanges();
                return RedirectToAction("Subjects", "Home", new { Area = "Admin" });
            }
        }
    }
}
