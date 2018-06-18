using System.Linq;
using System.Web.Mvc;
using ProfileSelect.Models;
using ProfileSelect.ViewModels;

namespace ProfileSelect.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(DepartmentViewModel departmentViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(departmentViewModel);
            }
            using (var dbCotext = new ApplicationDbContext())
            {
                dbCotext.Departments.Add(new Department
                {
                    FullName = departmentViewModel.FullName,
                    ShortName = departmentViewModel.ShortName,
                    IsCompany = departmentViewModel.IsCompany,
                    IsDeleted = false
                });
                dbCotext.SaveChanges();
                return RedirectToAction("Departments", "Home", new { Area = "Admin" });
            }
        }

        public ActionResult Edit(int id)
        {
            using (var dbCotext = new ApplicationDbContext())
            {
                var department = dbCotext.Departments.First(d => d.Id == id);
                return View(new DepartmentViewModel
                {
                    Id = department.Id,
                    FullName = department.FullName,
                    ShortName = department.ShortName,
                    IsCompany = department.IsCompany
                });
            }
        }

        [HttpPost]
        public ActionResult Edit(DepartmentViewModel departmentViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(departmentViewModel);
            }
            using (var dbCotext = new ApplicationDbContext())
            {
                var department = dbCotext.Departments.First(d => d.Id == departmentViewModel.Id);
                department.FullName = departmentViewModel.FullName;
                department.ShortName = departmentViewModel.ShortName;
                department.IsCompany = departmentViewModel.IsCompany;
                dbCotext.SaveChanges();
                return RedirectToAction("Departments", "Home", new { Area = "Admin" });
            }
        }

        public ActionResult Delete(int id)
        {
            using (var dbCotext = new ApplicationDbContext())
            {
                var department = dbCotext.Departments.First(d => d.Id == id);
                department.IsDeleted = true;
                dbCotext.SaveChanges();
                return RedirectToAction("Departments", "Home", new { Area = "Admin" });
            }
        }
    }
}
