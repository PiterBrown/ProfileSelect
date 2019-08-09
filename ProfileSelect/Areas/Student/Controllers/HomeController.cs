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
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Patronymic = user.Patronymic,
                    CurrentGroupName = user.CurrentGroup.Name,
                    PreviewGroupName = user.PreviewGroup != null ? user.PreviewGroup.Name : "",
                    NewGroupName = user.NewGroup != null ? user.NewGroup.Name : "",
                    NewProfileId = user.NewProfileId!=null ? user.NewProfileId : 0,
                    NewProfileName = user.NewProfile!=null ? user.NewProfile.Name : "",
                    NewDepartmentId=user.NewDepartment!=null? user.NewDepartment.Id:0,
                    NewDepartmentName=user.NewDepartment!=null?user.NewDepartment.ShortName:"",
                    DirectionId=user.Direction.Id,
                    DepartmentId=user.PreviewGroup!=null?user.PreviewGroup.Department.Id:0,
                    Groups = dbContext.Groups.Where(g => g.Direction.Id == user.Direction.Id).Select(g=>new GroupViewModel {
                        Id=g.Id,
                        Name=g.Name,
                        DirectionId=g.Direction.Id,
                        DirectionName=g.Direction.Name,
                        DepartmentId=g.Department.Id,
                        DepartmentName=g.Department.ShortName,
                        ProfileId=g.Profile.Id,
                        ProfileName=g.Profile.Name,
                        IsDelete=g.IsDeleted,
                        IsDist=g.IsDistr
                    }).ToList()
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
                    }).ToList(),
                    BlockPriorities = user.BlockPrioritys.OrderBy(cp => cp.Priority).Select(cp => new BlockPriorityViewModel
                    {
                        Id = cp.Id,
                        Priority = cp.Priority
                    }).ToList(),
                    Profiles = dbContext.Profiles.Where(p=>!p.IsDeleted).OrderBy(p => p.Direction.Id).Select(p => new ProfileViewModel
                    {
                        Id= p.Id,
                        DepartmentId = p.Department.Id,
                        DirectionId = p.Direction.Id,
                        Name = p.Name
                    }).ToList(),
                    BlockComps = dbContext.BlockComps.OrderBy(bc=>bc.Block.Profile.Id).Select(bc=> new BlockCompViewModel
                    {
                        BlockCompId = bc.Id,
                        BlockId = bc.Block.Id,
                        BlockIsDelete = bc.Block.IsDelete,
                        ProfileId = bc.Block.Profile.Id,
                        SubjectsId = bc.Subject.Id,
                        SubjectName = bc.Subject.Name,
                        DirectionId = bc.Block.Profile.Direction.Id,

                    }).ToList(),
                    Direction=user.Direction.Id
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
                    var profilePriorities = dbContext.ProfilePrioritys.Add(new ProfilePriority
                    {
                        Student = user,
                        Profile = dbContext.Profiles.First(u=>u.Id==pp.Id),
                        Priority = pp.Priority
                    });
                }

                foreach (var cp in claimViewModel.BlockPriorities)
                {
                    var blockPriorities = dbContext.BlockPrioritys.Add(new BlockPriority
                    {
                        Student = user,
                        Block = dbContext.Blocks.First(u=>u.Id==cp.Id),
                        Priority=cp.Priority
                    });
                }
                //foreach (var pp in claimViewModel.ProfilePriorities)
                //{
                //    var profilePrioritiy = dbContext.ProfilePrioritys.First(p => p.Id == pp.Id);
                //    profilePrioritiy.Priority = pp.Priority;
                //}
                dbContext.SaveChanges();

                var profilePrioritiys = user.ProfilePrioritys.ToList();
                var blockPrioritiys = user.BlockPrioritys.ToList();
                var templatePath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "Заявление шаблон.docx");
                if (user.Direction.Id == 1)
                {
                    templatePath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "Заявление шаблон090301.docx");
                };
                if (user.Direction.Id == 2)
                {
                    templatePath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "Заявление шаблон090303.docx");
                };
                if (user.Direction.Id == 3)
                {
                    templatePath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "Заявление шаблон090304.docx");
                };
                if (user.Direction.Id == 4)
                {
                    templatePath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "Заявление шаблон010304.docx");
                };
                var docId = Guid.NewGuid().ToString();
                var tempDocxPath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), $"{docId}.docx");
                var tempPdfPath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), $"{docId}.pdf");
                var tempLogPath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), $"{docId}-log.txt");
                using (var memoryStream = new MemoryStream())
                {
                    var fileBytes = System.IO.File.ReadAllBytes(templatePath);
                    memoryStream.Write(fileBytes, 0, fileBytes.Length);

                    using (WordprocessingDocument doc = WordprocessingDocument.Open(memoryStream, true))
                    {
                        var body = doc.MainDocumentPart.Document.Body;
                        var texts = body.Descendants<Text>().ToList();

                        //var table = doc.MainDocumentPart.Document.Descendants<Table>().ToList().First();
                        //var i = 1;
                        //var tr1 = new TableRow();   
                        //var tc1 = new Word.TableCell();
                        //tc1.Append(new TableCellProperties(new VerticalMerge() { Val = MergedCellValues.Restart }));
                        //var tc2 = new TableCell();
                        //tc2.Append(new TableCellProperties(new VerticalMerge() { Val = MergedCellValues.Continue }));
                        //tr1.Append(tc1,tc2);
                        //table.Append(tr1);
                        // var tr3 = new TableRow();
                        // //tr.Append(new TableRowProperties(new GridAfter() { Val = 2 }));

                        // var Cell1 = new TableCell();
                        //// tr1.Append(new TableRowProperties(new CellMerge() { Val = 2 }));
                        // Cell1.Append(new Paragraph(new Run(new Text("1"))));
                        // var Cell2 = new TableCell();
                        // Cell2.Append(new Paragraph(new Run(new Text("2"))));

                        // tr3.Append(Cell1);
                        // tr2.Append(Cell2);
                        // tr1.Append(tr2)
                        // tr3.Append(tr1, tr2);
                        // table.Append(tr3);
                        //doc.MainDocumentPart.Document.Body.Append(table);

                        
                        //doc.MainDocumentPart.Document.Save();
                        //    table.AppendChild(tr);
                        //foreach (var profilePriority in profilePrioritiys)
                        //{
                        //    var tr = new TableRow();
                        //    tr.Append(new TableRowProperties(new GridAfter() { Val=2}));

                        //    var indexCell = new TableCell();
                        //    //indexCell.Append(new TableCellProperties(new GridSpan() { Val = 2 }));
                        //    indexCell.Append(new Paragraph(new Run(new Text(i.ToString()))));

                        //    var departmentCell = new TableCell();
                        //    departmentCell.Append(new Paragraph(new Run(new Text(profilePriority.Profile.Department.FullName))));

                        //    var profileCell = new TableCell();
                        //    profileCell.Append(new Paragraph(new Run(new Text(profilePriority.Profile.Name))));

                        //    var priorityCell = new TableCell();
                        //    priorityCell.Append(new Paragraph(new Run(new Text(profilePriority.Priority.ToString()))));

                        //    tr.Append(indexCell, departmentCell, profileCell, priorityCell);
                        //    table.AppendChild(tr);
                        //    i++;
                        //}

                        texts.First(t => t.Text == "ФИО").Text = string.Format("{0} {1} {2}", user.LastName, user.FirstName, user.Patronymic);
                        texts.First(t => t.Text == "Номер_группы").Text = user.CurrentGroup.Name;//NewGroup?.Name ?? "";
                        texts.First(t => t.Text == "Номер_направления").Text = user.Direction.Code;
                        texts.First(t => t.Text == "Название_направления").Text = user.Direction.Name;
                        texts.First(t => t.Text == "Дата_заполнения").Text = DateTime.Now.ToString("d");
                        if (user.Direction.Id == 1)
                        {
                            texts.First(t => t.Text == "Проф1").Text = user.ProfilePrioritys.First(p => p.Student.Id == user.Id && p.Profile.Id == 1).Priority.ToString();
                            texts.First(t => t.Text == "Проф3").Text = user.ProfilePrioritys.First(p => p.Student.Id == user.Id && p.Profile.Id == 3).Priority.ToString();
                            texts.First(t => t.Text == "Проф4").Text = user.ProfilePrioritys.First(p => p.Student.Id == user.Id && p.Profile.Id == 4).Priority.ToString();
                            texts.First(t => t.Text == "Бл1").Text = user.BlockPrioritys.First(p => p.Student.Id == user.Id && p.Block.Id == 1).Priority.ToString();
                            texts.First(t => t.Text == "Бл2").Text = user.BlockPrioritys.First(p => p.Student.Id == user.Id && p.Block.Id == 2).Priority.ToString();
                            texts.First(t => t.Text == "Бл3").Text = user.BlockPrioritys.First(p => p.Student.Id == user.Id && p.Block.Id == 3).Priority.ToString();
                            texts.First(t => t.Text == "Бл4").Text = user.BlockPrioritys.First(p => p.Student.Id == user.Id && p.Block.Id == 4).Priority.ToString();
                            texts.First(t => t.Text == "Бл5").Text = user.BlockPrioritys.First(p => p.Student.Id == user.Id && p.Block.Id == 5).Priority.ToString();
                            texts.First(t => t.Text == "Бл6").Text = user.BlockPrioritys.First(p => p.Student.Id == user.Id && p.Block.Id == 6).Priority.ToString();
                            texts.First(t => t.Text == "Бл7").Text = user.BlockPrioritys.First(p => p.Student.Id == user.Id && p.Block.Id == 7).Priority.ToString();
                        }
                        if (user.Direction.Id == 2)
                        {
                            texts.First(t => t.Text == "Проф5").Text = user.ProfilePrioritys.First(p => p.Student.Id == user.Id && p.Profile.Id == 5).Priority.ToString();
                            texts.First(t => t.Text == "Проф10").Text = user.ProfilePrioritys.First(p => p.Student.Id == user.Id && p.Profile.Id == 10).Priority.ToString();
                            texts.First(t => t.Text == "Бл8").Text = user.BlockPrioritys.First(p => p.Student.Id == user.Id && p.Block.Id == 8).Priority.ToString();
                            texts.First(t => t.Text == "Бл9").Text = user.BlockPrioritys.First(p => p.Student.Id == user.Id && p.Block.Id == 9).Priority.ToString();
                            texts.First(t => t.Text == "Бл10").Text = user.BlockPrioritys.First(p => p.Student.Id == user.Id && p.Block.Id == 10).Priority.ToString();
                            
                        }
                        if (user.Direction.Id == 3)
                        {
                            texts.First(t => t.Text == "Проф6").Text = user.ProfilePrioritys.First(p => p.Student.Id == user.Id && p.Profile.Id == 6).Priority.ToString();
                            texts.First(t => t.Text == "Проф7").Text = user.ProfilePrioritys.First(p => p.Student.Id == user.Id && p.Profile.Id == 7).Priority.ToString();
                            texts.First(t => t.Text == "Проф8").Text = user.ProfilePrioritys.First(p => p.Student.Id == user.Id && p.Profile.Id == 8).Priority.ToString();
                            texts.First(t => t.Text == "Проф9").Text = user.ProfilePrioritys.First(p => p.Student.Id == user.Id && p.Profile.Id == 9).Priority.ToString();
                            texts.First(t => t.Text == "Бл14").Text = user.BlockPrioritys.First(p => p.Student.Id == user.Id && p.Block.Id == 14).Priority.ToString();
                            texts.First(t => t.Text == "Бл15").Text = user.BlockPrioritys.First(p => p.Student.Id == user.Id && p.Block.Id == 15).Priority.ToString();
                        }
                        if (user.Direction.Id == 4)
                        {
                            texts.First(t => t.Text == "Бл19").Text = user.BlockPrioritys.First(p => p.Student.Id == user.Id && p.Block.Id == 19).Priority.ToString();
                            texts.First(t => t.Text == "Бл20").Text = user.BlockPrioritys.First(p => p.Student.Id == user.Id && p.Block.Id == 20).Priority.ToString();
                        }
                    }
                    try
                        {
                        using (FileStream file = new FileStream(tempDocxPath, FileMode.Create, FileAccess.Write))
                        {
                            memoryStream.WriteTo(file);
                        }
                    }
                    catch (Exception ex)
                    {
                        StreamWriter file = new StreamWriter(new FileStream(tempLogPath, FileMode.OpenOrCreate, FileAccess.Write));
                            file.Write(ex.Message);
                            file.Close();
                    }
                }


                if (!System.IO.File.Exists(tempDocxPath))
                {
                    System.Threading.Thread.Sleep(5000);
                }
                try
                {
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
                }
                catch (Exception ex)
                {
                    StreamWriter file = new StreamWriter(new FileStream(tempLogPath, FileMode.OpenOrCreate, FileAccess.Write));
                            file.Write(ex.Message);
                    file.Close();
                }
                
               

                if (!System.IO.File.Exists(tempPdfPath))
                {
                    System.Threading.Thread.Sleep(2000);
                }


                

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
