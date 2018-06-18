using System.Linq;
using System.Web.Mvc;
using ProfileSelect.Models;
using ProfileSelect.ViewModels;

namespace ProfileSelect.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DirectionController : Controller
    {
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(DirectionViewModel departmentViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(departmentViewModel);
            }
            using (var dbCotext = new ApplicationDbContext())
            {
                dbCotext.Directions.Add(new Direction
                {
                    Name = departmentViewModel.Name,
                    Code = departmentViewModel.Code,
                    IsDeleted = false
                });
                dbCotext.SaveChanges();
                return RedirectToAction("Directions", "Home", new { Area = "Admin" });
            }
        }

        public ActionResult Edit(int id)
        {
            using (var dbCotext = new ApplicationDbContext())
            {
                var direction = dbCotext.Directions.First(d => d.Id == id);
                return View(new DirectionViewModel
                {
                    Id = direction.Id,
                    Name = direction.Name,
                    Code = direction.Code
                });
            }
        }

        [HttpPost]
        public ActionResult Edit(DirectionViewModel departmentViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(departmentViewModel);
            }
            using (var dbCotext = new ApplicationDbContext())
            {
                var direction = dbCotext.Directions.First(d => d.Id == departmentViewModel.Id);
                direction.Name = departmentViewModel.Name;
                direction.Code = departmentViewModel.Code;
                dbCotext.SaveChanges();
                return RedirectToAction("Directions", "Home", new { Area = "Admin" });
            }
        }

        public ActionResult Delete(int id)
        {
            using (var dbCotext = new ApplicationDbContext())
            {
                var direction = dbCotext.Directions.First(d => d.Id == id);
                direction.IsDeleted = true;
                dbCotext.SaveChanges();
                return RedirectToAction("Directions", "Home", new { Area = "Admin" });
            }
        }
    }
}
