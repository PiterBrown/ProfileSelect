using System.Linq;
using System.Web.Mvc;
using ProfileSelect.Models;
using ProfileSelect.ViewModels;

namespace ProfileSelect.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProfileController : Controller
    {
        public ActionResult Add()
        {
            using (var dbCotext = new ApplicationDbContext())
            {
                var departments = dbCotext.Departments.Where(d => !d.IsDeleted).Select(d => new DepartmentViewModel
                {
                    Id = d.Id,
                    FullName = d.FullName,
                    ShortName = d.ShortName
                }).ToList();
                var directions = dbCotext.Directions.Where(d => !d.IsDeleted).Select(d => new DirectionViewModel
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();
                return View(new ProfileViewModel
                {
                    Departments = departments,
                    Directions = directions
                });
            }
        }

        [HttpPost]
        public ActionResult Add(ProfileViewModel profileViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(profileViewModel);
            }
            using (var dbCotext = new ApplicationDbContext())
            {
                var direction = dbCotext.Directions.Where(d => !d.IsDeleted).First(d => d.Id == profileViewModel.DirectionId);
                var department = dbCotext.Departments.Where(d => !d.IsDeleted).First(d => d.Id == profileViewModel.DepartmentId);
                dbCotext.Profiles.Add(new Profile
                {
                    Name = profileViewModel.Name,
                    Direction = direction,
                    Department = department,
                    IsDeleted = false
                });
                dbCotext.SaveChanges();
                return RedirectToAction("Profiles", "Home", new { Area = "Admin" });
            }
        }

        public ActionResult Edit(int id)
        {
            using (var dbCotext = new ApplicationDbContext())
            {
                var profile = dbCotext.Profiles.First(d => d.Id == id);
                var departments = dbCotext.Departments.Where(d => !d.IsDeleted).Select(d => new DepartmentViewModel
                {
                    Id = d.Id,
                    FullName = d.FullName,
                    ShortName = d.ShortName
                }).ToList();
                var directions = dbCotext.Directions.Where(d => !d.IsDeleted).Select(d => new DirectionViewModel
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();
                return View(new ProfileViewModel
                {
                    Id = profile.Id,
                    Name = profile.Name,
                    DepartmentId = profile.Department.Id,
                    DirectionId = profile.Direction.Id,
                    Directions = directions,
                    Departments = departments
                });
            }
        }

        [HttpPost]
        public ActionResult Edit(ProfileViewModel profileViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(profileViewModel);
            }
            using (var dbCotext = new ApplicationDbContext())
            {
                var direction = dbCotext.Directions.Where(d => !d.IsDeleted).First(d => d.Id == profileViewModel.DirectionId);
                var department = dbCotext.Departments.Where(d => !d.IsDeleted).First(d => d.Id == profileViewModel.DepartmentId);

                var profile = dbCotext.Profiles.First(p => p.Id == profileViewModel.Id);
                profile.Name = profileViewModel.Name;
                profile.Direction = direction;
                profile.Department = department;

                dbCotext.SaveChanges();
                return RedirectToAction("Profiles", "Home", new { Area = "Admin" });
            }
        }

        public ActionResult Delete(int id)
        {
            using (var dbCotext = new ApplicationDbContext())
            {
                var direction = dbCotext.Directions.First(d => d.Id == id);
                direction.IsDeleted = true;
                dbCotext.SaveChanges();
                return RedirectToAction("Profiles", "Home", new { Area = "Admin" });
            }
        }
    }
}
