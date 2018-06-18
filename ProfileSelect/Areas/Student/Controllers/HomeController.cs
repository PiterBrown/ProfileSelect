using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNet.Identity;
using Microsoft.Office.Interop.Word;
using ProfileSelect.Models;
using ProfileSelect.ViewModels;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;

namespace ProfileSelect.Areas.Student.Controllers
{
    [Authorize(Roles = "Student")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var currentUserId = User.Identity.GetUserId();
                var user = dbContext.Users.First(u => u.Id == currentUserId);
                return View(new StudentViewModel
                {
                    Id = user.Id,
                    ClaimNumber = user.ClaimNumber + 1,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Patronymic = user.Patronymic,
                    CurrentGroupName = user.CurrentGroup.Name,
                    PreviewGroupName = user.PreviewGroup != null ? user.PreviewGroup.Name : "",
                    NewGroupName = user.NewGroup != null ? user.NewGroup.Name : ""
                });
            }
        }

        public ActionResult ChangePassword()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var currentUserId = User.Identity.GetUserId();
                var user = dbContext.Users.First(u => u.Id == currentUserId);
                ViewBag.Password = user.PasswordHash;
                return View();
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(ResetStudentPasswordViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            using (var dbContext = new ApplicationDbContext())
            {
                var currentUserId = User.Identity.GetUserId();
                var user = dbContext.Users.First(u => u.Id == currentUserId);
                user.PasswordHash = registerViewModel.Password;
                dbContext.SaveChanges();
                return RedirectToAction("ChangePassword", "Home", new { Area = "Student" });
            }
        }

        public ActionResult WriteClaim()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var currentUserId = User.Identity.GetUserId();
                var user = dbContext.Users.First(u => u.Id == currentUserId);
                return View(new ClaimViewModel
                {
                    ClaimNuber = user.ClaimNumber + 1,
                    ProfilePriorities = user.ProfilePrioritys.OrderBy(pp => pp.Priority).Select(pp => new ProfilePriorityViewModel
                    {
                        Id = pp.Id,
                        DepartmentName = pp.Profile.Department.FullName,
                        ProfileName = pp.Profile.Name,
                        Priority = pp.Priority
                    }).ToList()
                });
            }
        }

        public FileContentResult GeneratePdf(ClaimViewModel claimViewModel)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var currentUserId = User.Identity.GetUserId();
                var user = dbContext.Users.First(u => u.Id == currentUserId);
                user.ClaimNumber++;
                foreach (var pp in claimViewModel.ProfilePriorities)
                {
                    var profilePrioritiy = dbContext.ProfilePrioritys.First(p => p.Id == pp.Id);
                    profilePrioritiy.Priority = pp.Priority;
                }
                dbContext.SaveChanges();

                var profilePrioritiys = user.ProfilePrioritys.ToList();
                var templatePath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "Заявление шаблон.docx");
                var tempDocxPath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "temp.docx");
                var tempPdfPath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "temp.pdf");
                using (var memoryStream = new MemoryStream())
                {
                    var fileBytes = System.IO.File.ReadAllBytes(templatePath);
                    memoryStream.Write(fileBytes, 0, fileBytes.Length);

                    using (WordprocessingDocument doc = WordprocessingDocument.Open(memoryStream, true))
                    {
                        var body = doc.MainDocumentPart.Document.Body;
                        var texts = body.Descendants<Text>().ToList();

                        var table = doc.MainDocumentPart.Document.Descendants<Table>().ToList().First();
                        var i = 1;
                        foreach (var profilePriority in profilePrioritiys)
                        {
                            var tr = new TableRow();

                            var indexCell = new TableCell();
                            indexCell.Append(new Paragraph(new Run(new Text(i.ToString()))));

                            var departmentCell = new TableCell();
                            departmentCell.Append(new Paragraph(new Run(new Text(profilePriority.Profile.Department.FullName))));

                            var profileCell = new TableCell();
                            profileCell.Append(new Paragraph(new Run(new Text(profilePriority.Profile.Name))));

                            var priorityCell = new TableCell();
                            priorityCell.Append(new Paragraph(new Run(new Text(profilePriority.Priority.ToString()))));

                            tr.Append(indexCell, departmentCell, profileCell, priorityCell);
                            table.AppendChild(tr);
                            i++;
                        }

                        texts.First(t => t.Text == "ФИО").Text = string.Format("{0} {1} {2}", user.LastName, user.FirstName, user.Patronymic);
                        texts.First(t => t.Text == "Номер_группы").Text = user.CurrentGroup.Name;//NewGroup?.Name ?? "";
                        texts.First(t => t.Text == "Номер_направления").Text = user.Direction.Code;
                        texts.First(t => t.Text == "Название_направления").Text = user.Direction.Name;
                        texts.First(t => t.Text == "Дата_заполнения").Text = DateTime.Now.ToString("d");
                    }

                    using (FileStream file = new FileStream(tempDocxPath, FileMode.Create, FileAccess.Write))
                    {
                        memoryStream.WriteTo(file);
                    }
                }

                Application appWord = new Application();
                var wordDocument = appWord.Documents.Open(tempDocxPath);
                wordDocument.ExportAsFixedFormat(tempPdfPath, WdExportFormat.wdExportFormatPDF);

                appWord.DisplayAlerts = WdAlertLevel.wdAlertsNone;
                wordDocument.Close();
                appWord.Quit();
                if (wordDocument != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wordDocument);
                if (appWord != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(appWord);
                GC.Collect();

                using (var memoryStream = new MemoryStream())
                {
                    var fileBytes = System.IO.File.ReadAllBytes(tempPdfPath);
                    memoryStream.Write(fileBytes, 0, fileBytes.Length);
                    System.IO.File.Delete(tempDocxPath);
                    System.IO.File.Delete(tempPdfPath);
                    var fileName = string.Format("Заявление №{0}.pdf", user.ClaimNumber);
                    Response.AppendHeader("Content-Disposition", "inline; filename=" + fileName);

                    return File(fileBytes, "application/pdf");
                }
            }
        }
    }
}
