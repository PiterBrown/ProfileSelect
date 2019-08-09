using System.Linq;
using System.Web.Mvc;
using ProfileSelect.Models;
using ProfileSelect.ViewModels;
using Constants = ProfileSelect.Migrations.Constants;

namespace ProfileSelect.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GroupsController : Controller
    {
        public ActionResult Add()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var departments = dbContext.Departments.Where(d => !d.IsDeleted).Select(d => new DepartmentViewModel
                {
                    Id = d.Id,
                    FullName = d.FullName,
                    ShortName = d.ShortName
                }).ToList();
                var directions = dbContext.Directions.Where(d => !d.IsDeleted).Select(d => new DirectionViewModel
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();
                //var statuses = dbContext.Statuses.Select(s => new StatusViewModel
                //{
                //    Id = s.Id,
                //    Name = s.Name
                //}).ToList();

                return View(new GroupViewModel
                {
                    Departments = departments,
                    Directions = directions,
                    //Statuses = statuses
                });
            }
        }

        [HttpPost]
        public ActionResult Add(GroupViewModel groupViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(groupViewModel);
            }
            using (var dbCotext = new ApplicationDbContext())
            {
                var department = dbCotext.Departments.First(d => d.Id == groupViewModel.DepartmentId);
                var direction = dbCotext.Directions.First(d => d.Id == groupViewModel.DirectionId);
                var profile = dbCotext.Profiles.First(d => d.Id == groupViewModel.ProfileId);
                //var status = dbCotext.Statuses.First(d => d.Id == groupViewModel.StatusId);

                dbCotext.Groups.Add(new Group
                {
                    Name = groupViewModel.Name,
                    Count = groupViewModel.Count,
                    Department = department,
                    Direction = direction,
                    //Status = status
                });
                dbCotext.SaveChanges();
                return RedirectToAction("Groups", "Home", new { Area = "Admin" });
            }
        }

        public ActionResult Edit(int id)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var departments = dbContext.Departments.Where(d => !d.IsDeleted).Select(d => new DepartmentViewModel
                {
                    Id = d.Id,
                    FullName = d.FullName,
                    ShortName = d.ShortName
                }).ToList();
                var directions = dbContext.Directions.Where(d => !d.IsDeleted).Select(d => new DirectionViewModel
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();
                var statuses = dbContext.Statuses.Select(s => new StatusViewModel
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();

                var groups = dbContext.Groups.First(d => d.Id == id);
                return View(new GroupViewModel
                {
                    Id = groups.Id,
                    Name = groups.Name,
                    DepartmentId = groups.Department.Id,
                    DirectionId = groups.Direction.Id,
                    //StatusId = groups.Status.Id,
                    Departments = departments,
                    Directions = directions,
                    Statuses = statuses
                });
            }
        }

        public ActionResult ChangeNumbers()
        {
            using (var dbCotext = new ApplicationDbContext())
            {
                //var department = dbCotext.Departments.First(d => d.Id == groupViewModel.DepartmentId);
                var direction = dbCotext.Directions.Where(d=>!d.IsDeleted).ToList();
                var students = dbCotext.Users.Where(s => !s.IsDeleted && s.IsActive).ToList();
                var groups = dbCotext.Groups.Where(g => !g.IsDeleted).ToList();
                //var status = dbCotext.Statuses.First(d => d.Id == groupViewModel.StatusId);
                foreach (var d in direction)
                {
                    var studentsCount = students.Where(s => s.Direction.Id == d.Id).Count();
                    var groupCount = groups.Where(g => g.Direction.Id == d.Id && !g.IsDistr).Count();
                    var numbers = studentsCount / groupCount;
                   
                    var balanceNumbers = studentsCount % groupCount;
                    foreach (var g in groups.Where(q=>q.Direction==d))
                    {
                        if (!g.IsDistr)
                        {
                            g.Count = numbers;
                        }
                        else
                        {
                            g.Count = 0;
                        }
                    }
                    if (balanceNumbers!=0)
                    {
                        for (var i=0;i<balanceNumbers;i++)
                        {
                            groups.Where(g=>g.Direction==d).ElementAt(i).Count++;
                        }
                    }
                }

                dbCotext.SaveChanges();
                return RedirectToAction("Groups", "Home", new { Area = "Admin" });
            }
        }

        public ActionResult D()
        {
            using (var dbContext = new ApplicationDbContext())
            {

                var direction = dbContext.Directions.Where(d => !d.IsDeleted).ToList();
                var role = dbContext.Roles.First(r => r.Name == Constants.RolesConstants.Student.Name);
                var students = dbContext.Users.Where(s=> s.Roles.Any(r => r.RoleId == role.Id)).ToList();
                var profiles = dbContext.Profiles.Where(s => !s.IsDeleted).ToList();
                var blocks = dbContext.Blocks.Where(s => !s.IsDelete).ToList();
                var profilePriorities = dbContext.ProfilePrioritys.OrderByDescending(p => p.Id).ToList();
                var blockpriorities = dbContext.BlockPrioritys.ToList();
                int ss = 0;

                foreach (var s in students)
                {
                    var profilesCount = profiles.Where(p => p.Direction==s.Direction).Count();
                    var blocksCount = blocks.Where(b => s.CurrentGroup.Direction.Id == b.Profile.Direction.Id).Count();
                    var tempProfilePriorities = profilePriorities.Where(p => p.Student.Id == s.Id).OrderByDescending(p=>p.Id).ToList();
                    var tempProfilePrioritiesCount = tempProfilePriorities.Count();
                    var tempBlockPriorities = blockpriorities.Where(b => b.Student.Id == s.Id).OrderByDescending(b=>b.Id).ToList();
                    var tempBlockPrioritiesCount = tempBlockPriorities.Count();
                    if (tempProfilePriorities.Count() > profilesCount)
                    {
                        for (var i = profilesCount; i < tempProfilePrioritiesCount; i++)
                        {
                            tempProfilePriorities.ElementAt(i).IsDelete = true;
                        }
                    }
                    if (tempBlockPrioritiesCount > blocksCount)
                    {
                        for (var i = blocksCount; i < tempBlockPrioritiesCount; i++)
                        {
                            tempBlockPriorities.ElementAt(i).IsDelete = true;
                        }
                    }
                    ss++;
                    dbContext.SaveChanges();
                }
                //dbContext.SaveChanges();

                return RedirectToAction("ProfileSubjectSelect", "Home", new { Area = "Admin" });
            }
        }

        [HttpPost]
        public ActionResult Edit(GroupViewModel groupViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(groupViewModel);
            }
            using (var dbCotext = new ApplicationDbContext())
            {
                var department = dbCotext.Departments.First(d => d.Id == groupViewModel.DepartmentId);
                var direction = dbCotext.Directions.First(d => d.Id == groupViewModel.DirectionId);
                //var status = dbCotext.Statuses.First(d => d.Id == groupViewModel.StatusId);
                var gropus = dbCotext.Groups.First(d => d.Id == groupViewModel.Id);
                gropus.Name = groupViewModel.Name;
                gropus.Count = groupViewModel.Count;
                gropus.Department = department;
                gropus.Direction = direction;
                //gropus.Status = status;

                dbCotext.SaveChanges();
                return RedirectToAction("Groups", "Home", new { Area = "Admin" });
            }
        }

        public ActionResult Delete(int id)
        {
            using (var dbCotext = new ApplicationDbContext())
            {
                var groups = dbCotext.Groups.First(d => d.Id == id);
                groups.IsDeleted = true;
                dbCotext.SaveChanges();
                return RedirectToAction("Groups", "Home", new { Area = "Admin" });
            }
        }

        [HttpPost]
        public JsonResult SetIsBusy(int id, bool isDistr)
        {
            using (var dbCotext = new ApplicationDbContext())
            {
                var group = dbCotext.Groups.First(s => s.Id == id);
                group.IsDistr = isDistr;
                dbCotext.SaveChanges();

                var message = string.Format("Статус {0} для {1} сохранен", isDistr ? "Не участвует" : "Участвует",
                    group.Name);
                return Json(new
                {
                    Message = message
                });
            }
        }

        [HttpPost]
        public JsonResult SetCountText(int id, int Number)
        {
            using (var dbCotext = new ApplicationDbContext())
            {
                var groups = dbCotext.Groups.First(d => d.Id == id);
                groups.Count = Number;
                dbCotext.SaveChanges();

                var message = string.Format("Численность для группы {0} сохранена", groups.Name);
                return Json(new
                {
                    Message = message
                });
            }
        }
    }
}
