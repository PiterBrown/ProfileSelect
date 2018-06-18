using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProfileSelect.Models;
using ProfileSelect.ViewModels;
using Constants = ProfileSelect.Migrations.Constants;

namespace ProfileSelect.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StudentController : Controller
    {
        public ActionResult Add()
        {
            using (var dbCotext = new ApplicationDbContext())
            {
                var directions = dbCotext.Directions.Where(d => !d.IsDeleted).Select(d => new DirectionViewModel
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();
                var statuses = dbCotext.Statuses.Select(s => new StatusViewModel
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();
                var groups = dbCotext.Groups.Select(g => new GroupViewModel
                {
                    Id = g.Id,
                    Name = g.Name
                }).ToList();
                var profiles = dbCotext.Profiles.Select(p => new ProfileViewModel
                {
                    Id = p.Id,
                    Name = p.Name
                }).ToList();
                return View(new StudentViewModel
                {
                    ValidUntil = DateTime.Now.AddDays(1),
                    Directions = directions,
                    Statuses = statuses,
                    Groups = groups,
                    Profiles = profiles
                });
            }
        }

        [HttpPost]
        public ActionResult Add(StudentViewModel studentViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(studentViewModel);
            }
            using (var dbCotext = new ApplicationDbContext())
            {
                if (dbCotext.Users.Any(u => u.UserName == studentViewModel.UserName))
                {
                    ModelState.AddModelError("", "Имя пользователя уже используется");
                    return View(studentViewModel);
                }

                var direction = dbCotext.Directions.First(d => d.Id == studentViewModel.DirectionId);
                var status = dbCotext.Statuses.First(d => d.Id == studentViewModel.StatusId);
                var profile = dbCotext.Profiles.First(d => d.Id == studentViewModel.NewProfileId);
                var currentGroup = dbCotext.Groups.First(d => d.Id == studentViewModel.CurrentGroupId);
                var previewGroup = dbCotext.Groups.First(d => d.Id == studentViewModel.PreviewGroupId);
                var newGroup = dbCotext.Groups.First(d => d.Id == studentViewModel.NewGroupId);

                var student = dbCotext.Users.Add(new ApplicationUser
                {
                    UserName = studentViewModel.UserName,
                    FirstName = studentViewModel.FirstName,
                    LastName = studentViewModel.LastName,
                    Patronymic = studentViewModel.Patronymic,
                    FullName = studentViewModel.FullName,
                    Number = studentViewModel.Number,
                    StatusComm = studentViewModel.StatusComm,
                    Status = status,
                    CurrentGroup = currentGroup,
                    PreviewGroup = previewGroup,
                    NewGroup = newGroup,
                    Direction = direction,
                    NewProfile = profile,
                    CreateDate = DateTime.Now,
                    ValidUntil = studentViewModel.ValidUntil,
                    EmailConfirmed = true,
                    PasswordHash = studentViewModel.Password,
                    SecurityStamp = ""
                });
                dbCotext.SaveChanges();

                var store = new UserStore<ApplicationUser>(dbCotext);
                var manager = new UserManager<ApplicationUser>(store);
                manager.AddToRole(student.Id, Constants.RolesConstants.Student.Name);
                return RedirectToAction("Students", "Home", new { Area = "Admin" });
            }
        }

        public ActionResult Edit(string id)
        {
            using (var dbCotext = new ApplicationDbContext())
            {
                var directions = dbCotext.Directions.Where(d => !d.IsDeleted).Select(d => new DirectionViewModel
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();
                var statuses = dbCotext.Statuses.Select(s => new StatusViewModel
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();
                var groups = dbCotext.Groups.Select(g => new GroupViewModel
                {
                    Id = g.Id,
                    Name = g.Name
                }).ToList();
                var profiles = dbCotext.Profiles.Select(p => new ProfileViewModel
                {
                    Id = p.Id,
                    Name = p.Name
                }).ToList();
                var student = dbCotext.Users.Where(d => !d.IsDeleted).First(u => u.Id == id);
                return View(new StudentViewModel
                {
                    UserName = student.UserName,
                    Password = student.PasswordHash,
                    ValidUntil = student.ValidUntil,
                    Directions = directions,
                    Statuses = statuses,
                    Groups = groups,
                    Profiles = profiles,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Patronymic = student.Patronymic,
                    FullName = student.FullName,
                    Number = student.Number,
                    StatusComm = student.StatusComm,
                    StatusId = student.Status?.Id ?? -1,
                    CurrentGroupId = student.CurrentGroup?.Id ?? -1,
                    PreviewGroupId = student.PreviewGroup?.Id ?? -1,
                    NewGroupId = student.NewGroup?.Id ?? -1,
                    DirectionId = student.Direction?.Id ?? -1,
                    NewProfileId = student.NewProfile?.Id ?? -1
                });
            }
        }

        [HttpPost]
        public ActionResult Edit(StudentViewModel studentViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(studentViewModel);
            }
            using (var dbCotext = new ApplicationDbContext())
            {
                var direction = dbCotext.Directions.First(d => d.Id == studentViewModel.DirectionId);
                var status = dbCotext.Statuses.First(d => d.Id == studentViewModel.StatusId);
                var profile = dbCotext.Profiles.FirstOrDefault(d => d.Id == studentViewModel.NewProfileId);
                var currentGroup = dbCotext.Groups.First(d => d.Id == studentViewModel.CurrentGroupId);
                var previewGroup = dbCotext.Groups.FirstOrDefault(d => d.Id == studentViewModel.PreviewGroupId);
                var newGroup = dbCotext.Groups.FirstOrDefault(d => d.Id == studentViewModel.NewGroupId);

                var student = dbCotext.Users.First(u => u.Id == studentViewModel.Id);
                if (student.UserName != studentViewModel.UserName && dbCotext.Users.Any(u => u.UserName == studentViewModel.UserName))
                {
                    ModelState.AddModelError("", "Имя пользователя уже используется");
                    return View(studentViewModel);
                }

                student.UserName = studentViewModel.UserName;
                student.FirstName = studentViewModel.FirstName;
                student.LastName = studentViewModel.LastName;
                student.Patronymic = studentViewModel.Patronymic;
                student.FullName = studentViewModel.FullName;
                student.Number = studentViewModel.Number;
                student.StatusComm = studentViewModel.StatusComm;
                student.Status = status;
                student.CurrentGroup = currentGroup;
                student.PreviewGroup = previewGroup;
                student.NewGroup = newGroup;
                student.Direction = direction;
                student.NewProfile = profile;
                student.NewProfileId = studentViewModel.NewProfileId;
                student.ValidUntil = studentViewModel.ValidUntil;
                student.PasswordHash = studentViewModel.Password;
                dbCotext.SaveChanges();
                return RedirectToAction("Students", "Home", new { Area = "Admin" });
            }
        }

        public ActionResult Delete(string id)
        {
            using (var dbCotext = new ApplicationDbContext())
            {
                var student = dbCotext.Users.First(s => s.Id == id);
                student.IsDeleted = true;
                dbCotext.SaveChanges();
                return RedirectToAction("Students", "Home", new { Area = "Admin" });
            }
        }
    }
}
