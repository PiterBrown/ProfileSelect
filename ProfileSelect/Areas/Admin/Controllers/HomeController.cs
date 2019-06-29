using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OfficeOpenXml;
using ProfileSelect.Models;
using ProfileSelect.ViewModels;
using Translate;
using Constants = ProfileSelect.Migrations.Constants;

namespace ProfileSelect.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var role = dbContext.Roles.First(r => r.Name == Constants.RolesConstants.Student.Name);
                var users = dbContext.Users.Where(u => !u.IsDeleted && u.Roles.Any(r => r.RoleId == role.Id)).ToList();
                var groups = dbContext.Groups.ToList();
                var statuses = dbContext.Statuses.ToList();
                return View(new AdminViewModel
                {
                    Students = users.Select(s => new StudentViewModel
                    {
                        Id = s.Id,
                        UserName = s.UserName,
                    }).ToList(),
                    Groups = groups.Select(g => new GroupViewModel
                    {
                        Id = g.Id,
                        Name = g.Name
                    }).ToList(),
                    Status = statuses.Select(s => new StatusViewModel
                    {
                        Id = s.Id,
                        Name = s.Name

                    }).ToList()
                });
            }
        }

        public ActionResult Students()
        {

            using (var dbContext = new ApplicationDbContext())
            {
                var errors = TempData["errors"] as List<string> ?? new List<string>();
                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                ViewBag.HasErrors = errors.Any();

                var role = dbContext.Roles.First(r => r.Name == Constants.RolesConstants.Student.Name);
                var students = dbContext.Users.Where(u => !u.IsDeleted && u.Roles.Any(r => r.RoleId == role.Id)).Select(s => new StudentViewModel
                {
                    Id = s.Id,
                    IsActive = s.IsActive,
                    UserName = s.UserName,
                    Password = s.PasswordHash,
                    ValidUntil = s.ValidUntil,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Patronymic = s.Patronymic,
                    FullName = s.FullName,
                    Number = s.Number,
                    StatusComm = s.StatusComm,
                    StatusName = s.Status.Name,
                    CurrentGroupName = s.CurrentGroup.Name,
                    PreviewGroupName = s.PreviewGroup.Name,
                    NewGroupName = s.NewGroup.Name,
                    DirectionName = s.Direction.Name,
                    NewProfileName = s.NewProfile.Name,
                    AverageScore = s.AverageScore,
                    Score = s.Score,
                    IsBusy = s.IsBusy,
                    BusyReason = s.BusyReason
                }).ToList();

                return View(students);
            }

        }

        public ActionResult Departments()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var departments = dbContext.Departments.Where(d => !d.IsDeleted).Select(d => new DepartmentViewModel
                {
                    Id = d.Id,
                    FullName = d.FullName,
                    ShortName = d.ShortName,
                    IsCompany = d.IsCompany
                }).ToList();
                return View(departments);
            }
        }

        public ActionResult Directions()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var directions = dbContext.Directions.Where(d => !d.IsDeleted).Select(x => new DirectionViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code
                }).ToList();
                return View(directions);
            }
        }

        public ActionResult Profiles()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var profiles = dbContext.Profiles.Where(d => !d.IsDeleted).Select(p => new ProfileViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    DepartmentName = p.Department.FullName,
                    BaseDepartmentName = p.BaseDepartment.FullName,
                    DirectionName = p.Direction.Name
                }).ToList();
                return View(profiles);
            }
        }

        public ActionResult Subjects()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var subjects = dbContext.Subjects.Where(s => !s.IsDeleted).Select(s => new SubjectViewModel
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();
                return View(subjects);
            }
        }

        public ActionResult Statuses()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var statuses = dbContext.Statuses.Select(s => new StatusViewModel
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();
                return View(statuses);
            }
        }

        public ActionResult Groups()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var groups = dbContext.Groups.Where(g => !g.IsDeleted).Select(g => new GroupViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    Count = g.Count,
                    DepartmentName = g.Department.FullName,
                    DirectionName = g.Direction.Name,
                    IsDist = g.IsDistr
                    //StatusName = g.Status.Name
                }).ToList();
                return View(groups);
            }
        }

        public ActionResult ProfileSubjectSelect()
        {
            using (var dbContext = new ApplicationDbContext())
            {
               return View(new ProfileSubjectSelectViewModel
                {
                    ProfilePriority = dbContext.ProfilePrioritys.Select(g => new ProfilePriorityViewModel
                    {
                        Id = g.Id,
                        Priority = g.Priority,
                        ProfileName = g.Profile.Name,
                        StudentId = g.Student.Id,
                        DirectionId=g.Profile.Direction.Id
                    }).OrderByDescending(g=>g.Id).ToList(),
                    Student = dbContext.Users.Where(g => g.IsActive).Select(g => new StudentViewModel
                    {
                        Id = g.Id,
                        FirstName = g.FirstName,
                        LastName = g.LastName,
                        Patronymic = g.Patronymic,
                        Number = g.Patronymic,
                        DirectionId=g.Direction.Id,
                        CurrentGroupName=g.CurrentGroup.Name,
                        IsParc=g.IsParc
                    }).ToList(),
                    BlockPriority = dbContext.BlockPrioritys.Select(g => new BlockPriorityViewModel
                    {
                        Id = g.Id,
                        Priority = g.Priority,
                        DepartmentName = g.Block.Department.ShortName,
                        ProfileName=g.Block.Profile.Name,
                        StudentId=g.Student.Id,
                        BlockId=g.Block.Id
                    }).OrderByDescending(g => g.Id).ToList()
                });
            }
        }

        [HttpPost]
        public JsonResult SetIsParc(string id, bool isParc)
        {
            using (var dbCotext = new ApplicationDbContext())
            {
                var student = dbCotext.Users.First(s => s.Id == id);
                student.IsParc = isParc;
                dbCotext.SaveChanges();

                var message = string.Format("Статус {0} для {1} сохранен", isParc ? "Участвует" : "Не участвует",
                    student.FullName);
                return Json(new
                {
                    Message = message
                });
            }
        }

        [HttpPost]
        public ActionResult UploadStudentInfoFile(HttpPostedFileBase file, AdminViewModel adminViewModel)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var store = new UserStore<ApplicationUser>(dbContext);
                var manager = new UserManager<ApplicationUser>(store);
                using (var excelPackage = new ExcelPackage(file.InputStream))
                {
                    var usersWorksheet = excelPackage.Workbook.Worksheets.First();
                    var rowsCount = usersWorksheet.Dimension.End.Row;
                    var colCount = usersWorksheet.Dimension.End.Column;
                    var lnameCol = 0;
                    var nameCol = 0;
                    var patCol = 0;
                    var numbCol = 0;
                    var groupCol = 0;
                    var baseCol = 0;
                    var baseNameCol = 0;
                    for (int c = 1; c <= colCount; c++)
                    {
                        switch (usersWorksheet.Cells[1, c].Text)
                        {
                            case "Фамилия": lnameCol = c; break;
                            case "Имя": nameCol = c; break;
                            case "Отчество": patCol = c; break;
                            case "Личный номер": numbCol = c; break;
                            case "Группа": groupCol = c; break;
                            case "Целевик": baseCol = c; break;
                            case "База": baseNameCol = c; break;
                        }
                    }
                    for (int r = 2; r <= rowsCount; r++)
                    {
                        var фамиилия = usersWorksheet.Cells[r, lnameCol].Text;
                        if (string.IsNullOrEmpty(фамиилия))
                        {
                            break;
                        }

                        var имя = usersWorksheet.Cells[r, nameCol].Text;
                        var отчество = usersWorksheet.Cells[r, patCol].Text;
                        var личныйНомер = usersWorksheet.Cells[r, numbCol].Text;
                        var статус = dbContext.Statuses.First(s => s.Id == adminViewModel.StatusSelectId);//usersWorksheet.Cells[r, 5].Text;
                        var статусКом = статус.Name == "Академический отпуск" || статус.Name == "МО" ? статус.Name : null;
                        //var мо = usersWorksheet.Cells[r, 6].Text;
                        var группа = usersWorksheet.Cells[r, groupCol].Text;
                        var целевик = usersWorksheet.Cells[r, baseCol].Text;
                        var база = usersWorksheet.Cells[r, baseNameCol].Text;
                        bool неБеспокоить = статус.Name == "Академический отпуск" || статус.Name == "МО" || целевик == "Да" ? true : false;
                        if (целевик == "Да")
                        {
                            статус = dbContext.Statuses.First(s => s.Name == "Целевик");

                        }
                        if (статусКом != null && база != "")
                        {
                            статусКом = статусКом + "; " + база;

                        }
                        if (статусКом == null && база != "")
                        {
                            статусКом = база;

                        }
                        var group = dbContext.Groups.FirstOrDefault(g => g.Name == группа);
                        var student = dbContext.Users.Where(d => !d.IsDeleted).FirstOrDefault(s => s.Number == личныйНомер);
                        if (student == null)
                        {
                            var login = фамиилия.ToLower();
                            if (имя.Any())
                            {
                                login += имя.ToLower()[0];
                            }
                            if (отчество.Any())
                            {
                                login += отчество.ToLower()[0];
                            }
                            login = Transliteration.CyrillicToLatin(login);
                            var loginCount = dbContext.Users.Count(s => s.UserName.Contains(login));
                            if (loginCount > 0)
                            {
                                login += loginCount;
                            }

                            student = new ApplicationUser
                            {
                                CreateDate = DateTime.Now,
                                PasswordHash = Membership.GeneratePassword(6, 1),
                                ValidUntil = DateTime.Now.AddMonths(4),
                                UserName = login,
                                SecurityStamp = "",
                                FirstName = имя,
                                LastName = фамиилия,
                                Patronymic = отчество,
                                FullName = String.Format("{0} {1} {2}", фамиилия, имя, отчество),
                                Number = личныйНомер,
                                IsActive = true,
                                CurrentGroup = group,
                                Status = статус,
                                StatusComm = статусКом,
                                IsBusy = неБеспокоить,
                                Direction = group.Direction
                                //ClaimNumber = 1
                            };
                            dbContext.Users.Add(student);
                        }
                        else
                        {
                            student.FirstName = имя;
                            student.LastName = фамиилия;
                            student.Patronymic = отчество;
                            student.FullName = String.Format("{0} {1} {2}", фамиилия, имя, отчество);
                            student.Number = личныйНомер;
                            student.IsActive = true;
                            student.CurrentGroup = group;
                            student.Status = статус;
                            student.StatusComm = статусКом;
                            //dbContext.ProfilePrioritys.RemoveRange(student.ProfilePrioritys);
                            dbContext.SaveChanges();
                        }



                        dbContext.SaveChanges();
                        manager.AddToRole(student.Id, Constants.RolesConstants.Student.Name);
                    }
                }
            }
            return RedirectToAction("Students", "Home", new { Area = "Admin" });
        }

        [HttpPost]
        public ActionResult UploadStudentProfilesFile(HttpPostedFileBase file)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var store = new UserStore<ApplicationUser>(dbContext);
                var manager = new UserManager<ApplicationUser>(store);
                using (var excelPackage = new ExcelPackage(file.InputStream))
                {
                    var usersWorksheet = excelPackage.Workbook.Worksheets.First();
                    var rowsCount = usersWorksheet.Dimension.End.Row;
                    for (int r = 2; r <= rowsCount; r++)
                    {
                        var фамиилия = usersWorksheet.Cells[r, 2].Text;
                        if (string.IsNullOrEmpty(фамиилия))
                        {
                            break;
                        }

                        var имя = usersWorksheet.Cells[r, 3].Text;
                        var отчество = usersWorksheet.Cells[r, 4].Text;
                        var номерБилета = usersWorksheet.Cells[r, 5].Text;
                        var группа = usersWorksheet.Cells[r, 6].Text;
                        var ИППО = usersWorksheet.Cells[r, 9].Text;
                        var ВТ = usersWorksheet.Cells[r, 10].Text;
                        var ПИ = usersWorksheet.Cells[r, 11].Text;
                        var МОСИТ = usersWorksheet.Cells[r, 12].Text;
                        var КИС = usersWorksheet.Cells[r, 13].Text;
                        var ПМ = usersWorksheet.Cells[r, 14].Text;
                        var ППИ = usersWorksheet.Cells[r, 15].Text;
                        var БК239 = usersWorksheet.Cells[r, 16].Text;
                        var БК244 = usersWorksheet.Cells[r, 17].Text;
                        var БК248 = usersWorksheet.Cells[r, 18].Text;
                        var БК250 = usersWorksheet.Cells[r, 19].Text;
                        var БК254 = usersWorksheet.Cells[r, 20].Text;
                        var БК256 = usersWorksheet.Cells[r, 21].Text;

                        var group = dbContext.Groups.FirstOrDefault(g => g.Name == группа);
                        var student = dbContext.Users.Where(d => !d.IsDeleted).FirstOrDefault(s => s.FirstName == имя && s.LastName == фамиилия && s.Patronymic == отчество);//добавить личный номер

                        var profiles = dbContext.Profiles.ToList();
                        var groups = dbContext.Groups.ToList();
                        var departments = dbContext.Departments.ToList();
                        if (student != null)
                        {
                            if (!string.IsNullOrEmpty(ИППО) && ИППО != "0")
                            {
                                var direction = groups.First(d => d.Name == группа).Direction;
                                var department = departments.First(d => d.ShortName == "ИППО");
                                var i = Convert.ToInt32(ИППО);
                                var profile = profiles.First(p => p.Direction.Id == direction.Id && p.Department.Id == department.Id);
                                dbContext.ProfilePrioritys.Add(new ProfilePriority
                                {
                                    Priority = i,
                                    Profile = profile,
                                    Student = student
                                });
                            }
                            if (!string.IsNullOrEmpty(ВТ) && ВТ != "0")
                            {
                                var direction = groups.First(d => d.Name == группа).Direction;
                                var department = departments.First(d => d.ShortName == "ВТ");
                                var i = Convert.ToInt32(ВТ);
                                var profile = profiles.First(p => p.Direction.Id == direction.Id && p.Department.Id == department.Id);
                                dbContext.ProfilePrioritys.Add(new ProfilePriority
                                {
                                    Priority = i,
                                    Profile = profile,
                                    Student = student
                                });
                            }
                            if (!string.IsNullOrEmpty(ПИ) && ПИ != "0")
                            {
                                var direction = groups.First(d => d.Name == группа).Direction;
                                var department = departments.First(d => d.ShortName == "ПИ");
                                var i = Convert.ToInt32(ПИ);
                                var profile = profiles.First(p => p.Direction.Id == direction.Id && p.Department.Id == department.Id);
                                dbContext.ProfilePrioritys.Add(new ProfilePriority
                                {
                                    Priority = i,
                                    Profile = profile,
                                    Student = student
                                });
                            }
                            if (!string.IsNullOrEmpty(МОСИТ) && МОСИТ != "0")
                            {
                                var direction = groups.First(d => d.Name == группа).Direction;
                                var department = departments.First(d => d.ShortName == "МОСИТ");
                                var i = Convert.ToInt32(МОСИТ);
                                var profile = profiles.First(p => p.Direction.Id == direction.Id && p.Department.Id == department.Id);
                                dbContext.ProfilePrioritys.Add(new ProfilePriority
                                {
                                    Priority = i,
                                    Profile = profile,
                                    Student = student
                                });
                            }
                            if (!string.IsNullOrEmpty(КИС) && КИС != "0")
                            {
                                var direction = groups.First(d => d.Name == группа).Direction;
                                var department = departments.First(d => d.ShortName == "КИС");
                                var i = Convert.ToInt32(КИС);
                                var profile = profiles.First(p => p.Direction.Id == direction.Id && p.Department.Id == department.Id);
                                dbContext.ProfilePrioritys.Add(new ProfilePriority
                                {
                                    Priority = i,
                                    Profile = profile,
                                    Student = student
                                });
                            }
                            if (!string.IsNullOrEmpty(ПМ) && ПМ != "0")
                            {
                                var direction = groups.First(d => d.Name == группа).Direction;
                                var department = departments.First(d => d.ShortName == "ПМ");
                                var i = Convert.ToInt32(ПМ);
                                var profile = profiles.First(p => p.Direction.Id == direction.Id && p.Department.Id == department.Id);
                                dbContext.ProfilePrioritys.Add(new ProfilePriority
                                {
                                    Priority = i,
                                    Profile = profile,
                                    Student = student
                                });
                            }
                            if (!string.IsNullOrEmpty(ППИ) && ППИ != "0")
                            {
                                var direction = groups.First(d => d.Name == группа).Direction;
                                var department = departments.First(d => d.ShortName == "ППИ");
                                var i = Convert.ToInt32(ППИ);
                                var profile = profiles.First(p => p.Direction.Id == direction.Id && p.Department.Id == department.Id);
                                dbContext.ProfilePrioritys.Add(new ProfilePriority
                                {
                                    Priority = i,
                                    Profile = profile,
                                    Student = student
                                });
                            }

                            if (!string.IsNullOrEmpty(БК239) && БК239 != "0")
                            {
                                var direction = groups.First(d => d.Name == группа).Direction;
                                var department = departments.First(d => d.ShortName.Replace(" ", string.Empty) == "БК239");
                                var i = Convert.ToInt32(БК239);
                                var profile = profiles.First(p => p.Direction.Id == direction.Id && p.Department.Id == department.Id);
                                dbContext.ProfilePrioritys.Add(new ProfilePriority
                                {
                                    Priority = i,
                                    Profile = profile,
                                    Student = student
                                });
                            }

                            #region не используемые направления
                            if (!string.IsNullOrEmpty(БК244) && БК244 != "0")
                            {
                                var i = Convert.ToInt32(БК244);
                                var profile = profiles[i - 1];
                                dbContext.ProfilePrioritys.Add(new ProfilePriority
                                {
                                    Priority = i,
                                    Profile = profile,
                                    Student = student
                                });
                            }
                            if (!string.IsNullOrEmpty(БК248) && БК248 != "0")
                            {
                                var i = Convert.ToInt32(БК248);
                                var profile = profiles[i - 1];
                                dbContext.ProfilePrioritys.Add(new ProfilePriority
                                {
                                    Priority = i,
                                    Profile = profile,
                                    Student = student
                                });
                            }
                            if (!string.IsNullOrEmpty(БК250) && БК250 != "0")
                            {
                                var i = Convert.ToInt32(БК250);
                                var profile = profiles[i - 1];
                                dbContext.ProfilePrioritys.Add(new ProfilePriority
                                {
                                    Priority = i,
                                    Profile = profile,
                                    Student = student
                                });
                            }
                            if (!string.IsNullOrEmpty(БК254) && БК254 != "0")
                            {
                                var i = Convert.ToInt32(БК254);
                                var profile = profiles[i - 1];
                                dbContext.ProfilePrioritys.Add(new ProfilePriority
                                {
                                    Priority = i,
                                    Profile = profile,
                                    Student = student
                                });
                            }
                            #endregion

                            if (!string.IsNullOrEmpty(БК256) && БК256 != "0")
                            {
                                var direction = groups.First(d => d.Name == группа).Direction;
                                var department = departments.First(d => d.ShortName.Replace(" ", string.Empty) == "БК256");
                                var i = Convert.ToInt32(БК256);//ОШИБКА! БК256
                                var profile = profiles.First(p => p.Direction.Id == direction.Id && p.Department.Id == department.Id);
                                dbContext.ProfilePrioritys.Add(new ProfilePriority
                                {
                                    Priority = i,
                                    Profile = profile,
                                    Student = student
                                });
                            }
                            dbContext.SaveChanges();

                            if (!dbContext.ProfilePrioritys.Any(pp => pp.Student.Id == student.Id))
                            {
                                var studentProfiles = dbContext.Profiles.Where(p => p.Direction.Id == group.Direction.Id).Take(4).ToList();
                                foreach (var studentProfile in studentProfiles)
                                {
                                    dbContext.ProfilePrioritys.Add(new ProfilePriority
                                    {
                                        Priority = 0,
                                        Profile = studentProfile,
                                        Student = student
                                    });
                                }
                                dbContext.SaveChanges();
                            }


                        }
                    }
                }

                var LStudents = dbContext.Users.Where(s => !s.ProfilePrioritys.Any() && s.CurrentGroup != null);
                var Lprofiles = dbContext.Profiles.ToArray();
                foreach (var loststudents in LStudents)
                {
                    if (loststudents.CurrentGroup.Direction.Id == 1)
                    {
                        dbContext.ProfilePrioritys.Add(new ProfilePriority
                        {
                            Priority = 0,
                            Profile = Lprofiles.FirstOrDefault(l => l.Department.ShortName == "ВТ" && l.Direction == loststudents.CurrentGroup.Direction),
                            Student = loststudents
                        });

                        dbContext.ProfilePrioritys.Add(new ProfilePriority
                        {
                            Priority = 0,
                            Profile = Lprofiles.FirstOrDefault(l => l.Department.ShortName == "ПИ" && l.Direction == loststudents.CurrentGroup.Direction),
                            Student = loststudents
                        });

                        dbContext.ProfilePrioritys.Add(new ProfilePriority
                        {
                            Priority = 0,
                            Profile = Lprofiles.FirstOrDefault(l => l.Department.ShortName == "МОСИТ" && l.Direction == loststudents.CurrentGroup.Direction),
                            Student = loststudents
                        });

                        dbContext.ProfilePrioritys.Add(new ProfilePriority
                        {
                            Priority = 0,
                            Profile = Lprofiles.FirstOrDefault(l => l.Department.ShortName == "ПМ" && l.Direction == loststudents.CurrentGroup.Direction),
                            Student = loststudents
                        });
                    }

                    if (loststudents.CurrentGroup.Direction.Id == 2)
                    {
                        dbContext.ProfilePrioritys.Add(new ProfilePriority
                        {
                            Priority = 0,
                            Profile = Lprofiles.FirstOrDefault(l => l.Department.ShortName == "ППИ" && l.Direction == loststudents.CurrentGroup.Direction),
                            Student = loststudents
                        });

                        dbContext.ProfilePrioritys.Add(new ProfilePriority
                        {
                            Priority = 0,
                            Profile = Lprofiles.FirstOrDefault(l => l.Department.ShortName == "БК 239" && l.Direction == loststudents.CurrentGroup.Direction),
                            Student = loststudents
                        });

                        dbContext.ProfilePrioritys.Add(new ProfilePriority
                        {
                            Priority = 0,
                            Profile = Lprofiles.FirstOrDefault(l => l.Department.ShortName == "БК 256" && l.Direction == loststudents.CurrentGroup.Direction),
                            Student = loststudents
                        });
                    }

                    if (loststudents.CurrentGroup.Direction.Id == 3)
                    {
                        dbContext.ProfilePrioritys.Add(new ProfilePriority
                        {
                            Priority = 0,
                            Profile = Lprofiles.FirstOrDefault(l => l.Department.ShortName == "ИППО" && l.Direction == loststudents.CurrentGroup.Direction),
                            Student = loststudents
                        });

                        dbContext.ProfilePrioritys.Add(new ProfilePriority
                        {
                            Priority = 0,
                            Profile = Lprofiles.FirstOrDefault(l => l.Department.ShortName == "ВТ" && l.Direction == loststudents.CurrentGroup.Direction),
                            Student = loststudents
                        });

                        dbContext.ProfilePrioritys.Add(new ProfilePriority
                        {
                            Priority = 0,
                            Profile = Lprofiles.FirstOrDefault(l => l.Department.ShortName == "МОСИТ" && l.Direction == loststudents.CurrentGroup.Direction),
                            Student = loststudents
                        });

                        dbContext.ProfilePrioritys.Add(new ProfilePriority
                        {
                            Priority = 0,
                            Profile = Lprofiles.FirstOrDefault(l => l.Department.ShortName == "КИС" && l.Direction == loststudents.CurrentGroup.Direction),
                            Student = loststudents
                        });
                    }


                }
                dbContext.SaveChanges();
                return RedirectToAction("Students", "Home", new { Area = "Admin" });
            }
        }


        [HttpPost]
        public ActionResult UploadStudentPointsInfoFile(HttpPostedFileBase file)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                using (var excelPackage = new ExcelPackage(file.InputStream))
                {

                    var usersWorksheetCount = excelPackage.Workbook.Worksheets.Count();
                    for (int w = 0; w < usersWorksheetCount; w++)
                    {
                        var usersWorksheet = excelPackage.Workbook.Worksheets.ElementAt(w);
                        var rowsCount = usersWorksheet.Dimension.End.Row;
                        int c = 3;
                        var columnCount = 0;
                        //string[] columnName = new string[150];
                        while (usersWorksheet.Cells[6, c].Text != "Всего оценок")
                        {
                            //columnName[columnCount] = usersWorksheet.Cells[7, c].Text;
                            //columnCount++;
                            c++;
                        }
                        //while (usersWorksheet.Cells[6, c].Text != "Средний балл")
                        //{
                        //    c++;
                        //}

                        for (int r = 8; r <= rowsCount; r++)
                        {
                            var ФИО = usersWorksheet.Cells[r, 2].Text;
                            int q = 3, scoreCount = 0;
                            if (string.IsNullOrEmpty(ФИО))
                            {
                                break;
                            }
                            while (usersWorksheet.Cells[r, q].Text != "" && columnCount != scoreCount)
                            {
                                scoreCount++;
                                q++;
                            };
                            string[] score = new string[scoreCount];
                            for (int i = 0; i < scoreCount; i++)
                            {
                                score[i] = usersWorksheet.Cells[r, i + 3].Text;
                            }
                            var student = dbContext.Users.Where(d => !d.IsDeleted).First(s => s.FullName == ФИО);
                            var errors = new List<string>();
                            var avSc = CalculateAvgScore(score, ФИО, errors, student.CurrentGroup);
                            TempData["errors"] = errors;


                            student.AverageScore = avSc;
                            //float scr;
                            //if (float.TryParse(usersWorksheet.Cells[r, c].Text, out scr))
                            //{ student.Score = float.Parse(usersWorksheet.Cells[r, c].Text); }

                            dbContext.SaveChanges();
                        }
                    }
                }
            }
            return RedirectToAction("Students", "Home", new { Area = "Admin" });
        }

        public float CalculateAvgScore(string[] s, string name, List<string> errors, Group group)
        {
            float sc = 0;

            for (int i = 0; i < s.Count(); i++)
            {
                switch (s[i])
                {
                    case "З": sc = sc + 3; break;
                    case "Н/У": sc = sc + 2; break;
                    case "У": sc = sc + 3; break;
                    case "Х": sc = sc + 4; break;
                    case "О": sc = sc + 5; break;


                    //switch (s[i])
                    //{
                    //    case "Н/З": sc++; break;
                    //    case "З": sc = sc + 4; break;
                    //    case "Н/У": sc++; break;
                    //}
                    //break;

                    //case "Физическая культура и спорт (элективная дисциплина) 2/6, З":
                    //    switch (s[i])
                    //    {
                    //        case "Н/З": sc++; break;
                    //        case "З": sc = sc + 4; break;
                    //        case "Н/У": sc++; break;
                    //    }
                    //    break;

                    //case "Правоведение, З":
                    //    switch (s[i])
                    //    {
                    //        case "Н/З": sc++; break;
                    //        case "З": sc = sc + 3; break;
                    //        case "Н/У": sc++; break;
                    //    }
                    //    break;

                    //case "Иностранный язык 1/4, З":
                    //    switch (s[i])
                    //    {
                    //        case "Н/З": sc++; break;
                    //        case "З": sc = sc + 4; break;
                    //        case "Н/У": sc++; break;
                    //    }
                    //    break;

                    //case "Иностранный язык 2/4, З":
                    //    switch (s[i])
                    //    {
                    //        case "Н/З": sc++; break;
                    //        case "З": sc = sc + 4; break;
                    //        case "Н/У": sc++; break;
                    //    }
                    //    break;

                    //case "История, Э":
                    //    switch (s[i])
                    //    {
                    //        case "Н/У": sc++;  break;
                    //        case "У": sc=sc+3; break;
                    //        case "Х": sc = sc + 4; break;
                    //        case "О": sc = sc + 5; break;
                    //    }
                    //    break;

                    //case "Введение в профессиональную деятельность, З":
                    //    switch (s[i])
                    //    {
                    //        case "Н/З": sc++; break;
                    //        case "З": sc = sc + 4; break;
                    //        case "Н/У": sc++; break;
                    //    }
                    //    break;

                    //case "Введение в программную инженерию, З":
                    //    switch (s[i])
                    //    {
                    //        case "Н/З": sc++; break;
                    //        case "З": sc = sc + 4; break;
                    //        case "Н/У": sc++; break;
                    //    }
                    //    break;

                    //case "Инженерная графика, З":
                    //    switch (s[i])
                    //    {
                    //        case "Н/З": sc++; break;
                    //        case "З": sc = sc + 5; break;
                    //        case "Н/У": sc++; break;
                    //    }
                    //    break;

                    //case "Алгебра и геометрия 1/2, З":
                    //    switch (s[i])
                    //    {
                    //        case "Н/З": sc++; break;
                    //        case "З": sc = sc + 7; break;
                    //        case "Н/У": sc++; break;
                    //    }
                    //    break;

                    //case "Алгебра и геометрия 2/2, Э":
                    //    switch (s[i])
                    //    {
                    //        case "Н/У": sc++; break;
                    //        case "У": sc = sc + 6; break;
                    //        case "Х": sc = sc + 8; break;
                    //        case "О": sc = sc + 10; break;
                    //    }
                    //    break;

                    //case "Математический анализ 2/3, З":
                    //    switch (s[i])
                    //    {
                    //        case "Н/З": sc++; break;
                    //        case "З": sc = sc + 7; break;
                    //        case "Н/У": sc++; break;
                    //    }
                    //    break;

                    //case "Математический анализ 1/3, Э":
                    //    switch (s[i])
                    //    {
                    //        case "Н/У": sc++; break;
                    //        case "У": sc = sc + 6; break;
                    //        case "Х": sc = sc + 8; break;
                    //        case "О": sc = sc + 10; break;
                    //    }
                    //    break;

                    //case "Физика 1/2, З":
                    //    switch (s[i])
                    //    {
                    //        case "Н/З": sc++; break;
                    //        case "З": sc = sc + 6; break;
                    //        case "Н/У": sc++; break;
                    //    }
                    //    break;

                    //case "Физика 2/2, З":
                    //    switch (s[i])
                    //    {
                    //        case "Н/З": sc++; break;
                    //        case "З": sc = sc + 6; break;
                    //        case "Н/У": sc++; break;
                    //    }
                    //    break;

                    //case "Физика 1/2, Э":
                    //    switch (s[i])
                    //    {
                    //        case "Н/У": sc++; break;
                    //        case "У": sc = sc + 6; break;
                    //        case "Х": sc = sc + 8; break;
                    //        case "О": sc = sc + 10; break;
                    //    }
                    //    break;

                    //case "Физика 2/2, Э":
                    //    switch (s[i])
                    //    {
                    //        case "Н/У": sc++; break;
                    //        case "У": sc = sc + 6; break;
                    //        case "Х": sc = sc + 8; break;
                    //        case "О": sc = sc + 10; break;
                    //    }
                    //    break;

                    //case "Информатика, Э":
                    //    switch (s[i])
                    //    {
                    //        case "Н/У": sc++; break;
                    //        case "У": sc = sc + 8; break;
                    //        case "Х": sc = sc + 9; break;
                    //        case "О": sc = sc + 10; break;
                    //    }
                    //    break;

                    //case "Процедурное программирование, Э":
                    //    switch (s[i])
                    //    {
                    //        case "Н/У": sc++; break;
                    //        case "У": sc = sc + 5; break;
                    //        case "Х": sc = sc + 7; break;
                    //        case "О": sc = sc + 10; break;
                    //    }
                    //    break;

                    //case "Объектно-ориентированное программирование 1/2, КР":
                    //    switch (s[i])
                    //    {
                    //        case "Н/У": sc++; break;
                    //        case "У": sc = sc + 6; break;
                    //        case "Х": sc = sc + 7; break;
                    //        case "О": sc = sc + 10; break;
                    //    }
                    //    break;

                    //case "Объектно-ориентированное программирование 1/2, Э":
                    //    switch (s[i])
                    //    {
                    //        case "Н/У": sc++; break;
                    //        case "У": sc = sc + 7; break;
                    //        case "Х": sc = sc + 8; break;
                    //        case "О": sc = sc + 10; break;
                    //    }
                    //    break;

                    //case "Математические основы автоматизированных систем 1/2, Э":
                    //    switch (s[i])
                    //    {
                    //        case "Н/У": sc++; break;
                    //        case "У": sc = sc + 6; break;
                    //        case "Х": sc = sc + 8; break;
                    //        case "О": sc = sc + 10; break;
                    //    }
                    //    break;

                    //case "Дискретная математика, Э":
                    //    switch (s[i])
                    //    {
                    //        case "Н/У": sc++; break;
                    //        case "У": sc = sc + 6; break;
                    //        case "Х": sc = sc + 8; break;
                    //        case "О": sc = sc + 10; break;
                    //    }
                    //    break;

                    //case "Анализ сложности алгоритмов, З":
                    //    switch (s[i])
                    //    {
                    //        case "Н/З": sc++; break;
                    //        case "З": sc = sc + 8; break;
                    //        case "Н/У": sc++; break;
                    //    }
                    //    break;

                    //case "Развитие информационного общества, Э":
                    //    switch (s[i])
                    //    {
                    //        case "Н/У": sc++; break;
                    //        case "У": sc = sc + 6; break;
                    //        case "Х": sc = sc + 7; break;
                    //        case "О": sc = sc + 8; break;
                    //    }
                    //    break;

                    //case "Практика по получению первичных профессиональных умений и навыков, в том числе первичных умений и навыков научно-исследовательской деятельности, ЗД":
                    //    switch (s[i])
                    //    {
                    //        case "Н/У": sc++; break;
                    //        case "У": sc = sc + 5; break;
                    //        case "Х": sc = sc + 6; break;
                    //        case "О": sc = sc + 8; break;
                    //    }
                    //    break;

                    default:
                        {
                            errors.Add(string.Format("Для студента {0} не добавлен средний балл", name));
                            break;
                        }
                }
            }


            sc = sc / s.Count();


            return sc;
        }

        public FileResult GetStudentsList()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                using (var excelPackage = new ExcelPackage())
                {
                    var role = dbContext.Roles.First(r => r.Name == Constants.RolesConstants.Student.Name);
                    var students = dbContext.Users.Where(u => !u.IsDeleted && u.IsActive && u.Roles.Any(r => r.RoleId == role.Id)).ToList();

                    var worksheet = excelPackage.Workbook.Worksheets.Add("Список студентов");
                    worksheet.Column(1).Width = 5;
                    worksheet.Column(2).Width = 11;
                    worksheet.Column(3).Width = 11;
                    worksheet.Column(4).Width = 11;
                    worksheet.Column(5).Width = 30;
                    worksheet.Column(6).Width = 17;
                    worksheet.Column(7).Width = 17;
                    worksheet.Column(8).Width = 35;

                    worksheet.Cells[1, 1].Value = "№";
                    worksheet.Cells[1, 2].Value = "Фамилия";
                    worksheet.Cells[1, 3].Value = "Имя";
                    worksheet.Cells[1, 4].Value = "Отчество";
                    worksheet.Cells[1, 5].Value = "ФИО полностью";
                    worksheet.Cells[1, 6].Value = "Старая группа";
                    if (students.Any(s => s.PreviewGroup != null) && students.Any(s => s.NewGroup == null))
                    {
                        worksheet.Cells[1, 7].Value = "Предварительная группа";

                    }
                    if (students.Any(s => s.NewGroup != null))
                    {
                        worksheet.Cells[1, 7].Value = "Новая группа";

                    }
                    worksheet.Cells[1, 8].Value = "Полное название кафедры";
                    worksheet.Cells[1, 9].Value = "Переведенный балл";
                    worksheet.Cells[1, 10].Value = "Средний балл";


                    var i = 2;
                    foreach (var student in students)
                    {
                        worksheet.Cells[i, 1].Value = i - 1;
                        worksheet.Cells[i, 2].Value = student.LastName;
                        worksheet.Cells[i, 3].Value = student.FirstName;
                        worksheet.Cells[i, 4].Value = student.Patronymic;
                        worksheet.Cells[i, 5].Value = string.Format("{0} {1} {2}", student.LastName, student.FirstName, student.Patronymic);
                        if (student.CurrentGroup == null)
                        {
                            worksheet.Cells[i, 6].Value = "НЕТ";
                        }
                        else
                        {
                            worksheet.Cells[i, 6].Value = student.CurrentGroup.Name;
                        }
                        if (student.PreviewGroup == null)
                        {
                            worksheet.Cells[i, 7].Value = "НЕТ";
                            worksheet.Cells[i, 8].Value = "НЕТ";
                        }
                        if (student.PreviewGroup != null && student.NewGroup == null)
                        {
                            worksheet.Cells[i, 7].Value = student.PreviewGroup.Name;
                            worksheet.Cells[i, 8].Value = student.PreviewGroup.Department.FullName;
                        }
                        if (student.NewGroup != null)
                        {
                            worksheet.Cells[i, 7].Value = student.NewGroup.Name;
                            worksheet.Cells[i, 8].Value = student.NewGroup.Department.FullName;
                        }
                        worksheet.Cells[i, 9].Value = student.AverageScore;
                        worksheet.Cells[i, 10].Value = student.Score;
                        i++;
                    }

                    var bytes = excelPackage.GetAsByteArray();
                    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Экспорт студентов.xlsx");
                }
            }
        }


        [HttpPost]
        public ActionResult FillGroups(string scoreMethod)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                using (var dbContextTransaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {

                        var allStudents = dbContext.Users.ToList();
                        foreach (var student in allStudents)
                        {
                            student.PreviewGroup?.PreviewGroupStudents.Clear();
                            //student.PreviewGroup = null;
                            student.NewProfile?.Students.Clear();
                            //student.NewProfile = null;
                        }
                        dbContext.SaveChanges();

                        var specialStudentsStatuses = dbContext.Statuses.Where(s => s.Name == "МО" || s.Name == "Целевой" || s.Name == "Не беспокоить").Select(s => s.Id).ToList();
                        var specialStudents = dbContext.Users.Where(d => !d.IsDeleted && d.CurrentGroup != null).Where(s => specialStudentsStatuses.Contains(s.Status.Id)).ToList();
                        var profiles = dbContext.Profiles.Where(p => !p.IsDeleted).OrderBy(o => o.Id).ToList();
                        var groups = dbContext.Groups.ToList();
                        //foreach (var p in Profiles)
                        //{
                        //    if (p.BaseDepartment==null)
                        //    {
                        //        p.BaseDepartment = new Department() {Id=0};
                        //    }
                        //}
                        foreach (var student in specialStudents)
                        {

                            student.PreviewGroup = student.CurrentGroup;
                            student.Direction = student.CurrentGroup.Direction;
                            student.NewProfile = dbContext.Profiles.FirstOrDefault(p => p.Direction.Id == student.CurrentGroup.Direction.Id &&
                                                                                        (p.Department.Id == student.CurrentGroup.Department.Id ||
                                                                                      (p.BaseDepartment != null ? p.BaseDepartment.Id == student.CurrentGroup.Department.Id : p.Department.Id == student.CurrentGroup.Department.Id)));

                        }
                        //dbContextTransaction.Commit();
                        dbContext.SaveChanges();

                        var otherStudentsProfilePrioritys = dbContext.ProfilePrioritys
                            .Where(u => u.Student.IsActive && !u.Student.IsDeleted && u.Student.Status.Name == "Обычный" && u.Student.CurrentGroup != null)
                            .GroupBy(pp => pp.Priority)
                            .OrderBy(pp => pp.Key).ToList();
                        var users = dbContext.Users.Where(u => u.IsActive && !u.IsDeleted);


                        foreach (var otherStudentsProfilePriority in otherStudentsProfilePrioritys.Where(o => o.Key != 0))
                        {
                            var orderedOtherStudentsProfilePriority =
                                otherStudentsProfilePriority.Where(s => s.Student.NewProfile == null).OrderByDescending(o => o.Student.AverageScore);
                            if (scoreMethod == "Средний балл")
                            {
                                orderedOtherStudentsProfilePriority =
                                otherStudentsProfilePriority.Where(s => s.Student.NewProfile == null).OrderByDescending(o => o.Student.Score);
                            }

                            foreach (var profilePriority in orderedOtherStudentsProfilePriority.Where(s => s.Student.NewProfile == null))
                            {
                                var student = profilePriority.Student;
                                var profile = profilePriority.Profile;
                                var profileGroups = groups.Where(g => g.Direction.Id == profile.Direction.Id && g.Department.Id == profile.Department.Id).ToList();
                                if (profile.BaseDepartment != null)
                                {
                                    var additonalGroups = groups.Where(g => g.Direction.Id == profile.Direction.Id && g.Department.Id == profile.BaseDepartment.Id);
                                    profileGroups.AddRange(additonalGroups);
                                    profileGroups = profileGroups.Distinct().ToList();
                                }

                                var profileCount = 0;
                                foreach (var pc in profileGroups)
                                {
                                    profileCount = profileCount + pc.Count;
                                }
                                var usersCount = users.Where(u => u.NewProfileId == profile.Id).ToList();
                                var profCount = profileCount - usersCount.Count;//users.Where(u => u.NewProfileId.Value == profile.Id).Count();
                                if (profCount != 0)
                                {
                                    student.NewProfile = profile;
                                    if (student.NewProfile.Direction == student.CurrentGroup.Direction && student.NewProfile.Department == student.CurrentGroup.Department) //Добавить БК
                                    {
                                        student.PreviewGroup = student.CurrentGroup;
                                    }
                                    dbContext.SaveChanges();
                                }
                            }
                        }

                        foreach (var otherStudentsProfilePriority in otherStudentsProfilePrioritys.Where(o => o.Key == 0))
                        {
                            var orderedOtherStudentsProfilePriority =
                            otherStudentsProfilePriority.Where(s => s.Student.NewProfile == null).OrderByDescending(o => o.Student.AverageScore);
                            if (scoreMethod == "Средний балл")
                            {
                                orderedOtherStudentsProfilePriority =
                                otherStudentsProfilePriority.Where(s => s.Student.NewProfile == null).OrderByDescending(o => o.Student.Score);
                            }

                            foreach (var prof in profiles.OrderBy(o => o.Students.Count))
                            {
                                foreach (var profilePriority in orderedOtherStudentsProfilePriority.Where(p => p.Profile.Id == prof.Id && p.Student.NewProfileId == null))
                                {
                                    var student = profilePriority.Student;
                                    var profile = profilePriority.Profile;
                                    var profileGroups = groups.Where(g => g.Direction.Id == profile.Direction.Id && g.Department.Id == profile.Department.Id).ToList();
                                    if (profile.BaseDepartment != null)
                                    {
                                        var additonalGroups = groups.Where(g => g.Direction.Id == profile.Direction.Id && g.Department.Id == profile.BaseDepartment.Id);
                                        profileGroups.AddRange(additonalGroups);
                                        profileGroups = profileGroups.Distinct().ToList();
                                    }
                                    var profileCount = 0;
                                    foreach (var pc in profileGroups)
                                    {
                                        profileCount = profileCount + pc.Count;
                                    }
                                    var usersCount = users.Count(u => u.NewProfileId == profile.Id);
                                    var profCount = profileCount - usersCount;
                                    if (profCount != 0)
                                    {
                                        student.NewProfile = profile;
                                        if (student.NewProfile.Direction == student.CurrentGroup.Direction && student.NewProfile.Department == student.CurrentGroup.Department)
                                        {
                                            student.PreviewGroup = student.CurrentGroup;
                                        }
                                        dbContext.SaveChanges();
                                    }
                                }
                            }
                        }

                        foreach (var currentProfile in profiles.Where(p => p.Students.Count != 0))
                        {
                            var currentStudents = allStudents.Where(s => s.NewProfileId == currentProfile.Id && s.PreviewGroup == null).ToList();
                            var group = groups.Where(g => g.Direction.Id == currentProfile.Direction.Id && g.Department.Id == currentProfile.Department.Id).ToList();
                            if (currentProfile.BaseDepartment != null)
                            {
                                var additonalGroups = groups.Where(g => g.Direction.Id == currentProfile.Direction.Id && g.Department.Id == currentProfile.BaseDepartment.Id);
                                group.AddRange(additonalGroups);
                                group = group.Distinct().ToList();
                            }

                            foreach (var currentgroup in group)
                            {
                                do
                                {
                                    try
                                    {
                                        currentStudents = currentStudents.Where(c => c.PreviewGroup == null).ToList();
                                        for (int i = 0; i <= currentStudents.Count(); i = i + group.Count)
                                        {
                                            if (currentgroup.Count != currentgroup.PreviewGroupStudents.Count)
                                            {
                                                currentStudents.ElementAt(i).PreviewGroup = currentgroup;
                                                dbContext.SaveChanges();
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        if (currentStudents.Count == 0)
                                        {
                                            break;
                                        }
                                    }

                                } while (currentgroup.Count != currentgroup.PreviewGroupStudents.Count);
                            }
                        }
                        dbContextTransaction.Commit();
                        dbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        //dbContextTransaction.Rollback();
                    }
                }
            }

            return RedirectToAction("Students", "Home", new { Area = "Admin" });
        }

        public ActionResult ConfirmUserGroups()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var users = dbContext.Users.Where(u => !u.IsDeleted && u.PreviewGroup != null).ToList();
                foreach (var user in users)
                {
                    var newGroup = dbContext.Groups.First(g => g.Id == user.PreviewGroup.Id);
                    user.NewGroup = newGroup;
                }
                dbContext.SaveChanges();
                return RedirectToAction("Students", "Home", new { Area = "Admin" });
            }
        }

        public ActionResult PrintUserInfo(string UserId)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var user = dbContext.Users.Where(u => !u.IsDeleted).First(u => u.Id == UserId);
                return View(new StudentViewModel
                {
                    UserName = user.UserName,
                    Password = user.PasswordHash,
                    ValidUntil = user.ValidUntil
                });
            }
        }

        public FileResult PrintGroupUsersInfo()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                //var groups = dbContext.Groups.First(u => u.Id == GroupId);
                //return View(groups.NewGroupStudents.Select(s => new StudentViewModel
                //{
                //    Id = s.Id,
                //    UserName = s.UserName,
                //    Password = s.PasswordHash,
                //    ValidUntil = s.ValidUntil
                //}).ToList());

                using (var excelPackage = new ExcelPackage())
                {
                    var role = dbContext.Roles.First(r => r.Name == Constants.RolesConstants.Student.Name);
                    var students = dbContext.Users.Where(u => !u.IsDeleted && u.IsActive && u.Roles.Any(r => r.RoleId == role.Id)).OrderBy(u => u.CurrentGroup.Name).ToList();

                    var worksheet = excelPackage.Workbook.Worksheets.Add("Список студентов");
                    worksheet.Column(1).Width = 5;
                    worksheet.Column(2).Width = 11;
                    worksheet.Column(3).Width = 11;
                    worksheet.Column(4).Width = 11;
                    worksheet.Column(5).Width = 30;
                    worksheet.Column(6).Width = 17;


                    worksheet.Cells[1, 1].Value = "№";
                    worksheet.Cells[1, 2].Value = "ФИО";
                    worksheet.Cells[1, 3].Value = "Группа";
                    worksheet.Cells[1, 4].Value = "Логин";
                    worksheet.Cells[1, 5].Value = "Пароль";
                    worksheet.Cells[1, 6].Value = "Действует до";


                    var i = 2;
                    foreach (var student in students)
                    {
                        worksheet.Cells[i, 1].Value = i - 1;
                        worksheet.Cells[i, 2].Value = student.FullName;
                        worksheet.Cells[i, 3].Value = student.CurrentGroup.Name;
                        worksheet.Cells[i, 4].Value = student.UserName;
                        worksheet.Cells[i, 5].Value = student.PasswordHash;
                        worksheet.Cells[i, 6].Value = (student.ValidUntil.HasValue ? student.ValidUntil.Value.ToShortDateString() : "Бессрочно");
                        i++;
                    }

                    var bytes = excelPackage.GetAsByteArray();
                    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Пароли студентов.xlsx");
                }

            }
        }
    }
}
