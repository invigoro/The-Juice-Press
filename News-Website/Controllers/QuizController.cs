using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using News_Website.Data;
using News_Website.Models;
using News_Website.Services;

namespace News_Website.Controllers
{
    public class QuizController : BaseController
    {
        //private readonly ApplicationDbContext db;

        public QuizController(ApplicationDbContext context, 
            UserManager<User> userManager, 
            ILogger<BaseController> logger,
            ICloudStorage cloudStorage) : base(context, userManager, logger, cloudStorage)
        {
        }

        // GET: Quizzes
        public async Task<IActionResult> Index(string id)
        {
            List<Quiz> quizzes;
            if(currentUser == null) { quizzes = await db.Quizzes?.Where(x => x.Published)?.ToListAsync(); }
            else {
                var roles = await _userManager.GetRolesAsync(currentUser);
                if (roles?.Count() > 0)
                {
                    quizzes = await db.Quizzes?.ToListAsync();
                }
                else
                {
                    quizzes = await db.Quizzes?.Where(x => x.Published)?.ToListAsync();
                }
            }
            return View(quizzes);
        }

        public async Task<IActionResult> List(string id)
        {
            id = id?.ToLower();
            List<Quiz> quizzes = await db.Quizzes?.ToListAsync();
            //var categories = EnumHelper<QuizCategory>.GetDisplayValues(QuizCategory.Entertainment)?.Select(x => x.ToLower());
            if(id == "latest") { quizzes = quizzes.OrderByDescending(x => x.PublishedOn)?.ToList();
                ViewBag.ResultsTitle = $"Latest Quizzes";
            }
            //else if (categories.Contains(id)) { 
            //    quizzes = quizzes.Where(x => x.Category != null)?.ToList()?.Where(x => EnumHelper<QuizCategory>.GetDisplayValue((QuizCategory)x.Category).ToLower() == id)?.ToList();
            //    ViewBag.ResultsTitle = $"Latest {char.ToUpper(id[0]) + id.Substring(1)}";
            //}
            if (currentUser == null || !((await _userManager.GetRolesAsync(currentUser))?.Count()  > 0)) { quizzes = quizzes?.Where(x => x.Published)?.ToList(); }
            quizzes = quizzes.Take(50)?.ToList();
            return View(nameof(Index), quizzes);
        }

        public async Task<IActionResult> Search(string id)
        {
            var searchWords = id?.ToLower()?.Split(" ");
            var quizzes = db.Quizzes?.ToList();
            if (currentUser == null || !((await _userManager.GetRolesAsync(currentUser))?.Count() > 0)) { quizzes = quizzes?.Where(x => x.Published)?.ToList(); }
            if (!String.IsNullOrEmpty(id) && searchWords?.Count() > 0)
            {
                quizzes = quizzes.Where(x => !String.IsNullOrEmpty(x.Title) && x.Title.ToLower().ContainsAll(searchWords))?.ToList();
            }
            ViewBag.ResultsTitle = $"Search results for <i>{id}</i>";
            return View(nameof(Index), quizzes);
        }

        // GET: Quizzes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quiz = await db.Quizzes
                .FirstOrDefaultAsync(m => m.QuizId == id);
            if (quiz == null)
            {
                return NotFound();
            }


            if(currentUser == null && quiz.Published) //add views for non author users
            {
                quiz.TotalViews++;
                await db.SaveChangesAsync();
            }
            if (quiz.CoverImage != null) ViewData["CoverImage"] = quiz.CoverImage.Url;
            ViewData["Title"] = quiz.Title;
            ViewData["IsArticle"] = true;
            if (quiz.Published)
            {
                return View(quiz);
            }
            if (currentUser != null && User.IsInAnyRole())
            {
                return View(quiz);
            }
            else
            {
                return LocalRedirect($"/Identity/Account/Login?returnUrl={HttpContext.Request.Path.ToUriComponent()}");
            }
        }


        // GET: Quizzes/Edit/5
        [Authorize(Roles = "Editor, Admin, SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {

            var quiz = await db.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                return View(new Quiz());
            }
            
            return View(quiz);
        }
        [Authorize(Roles = "Editor, Admin, SuperAdmin")]
        public async Task<IActionResult> _Edit(int? id)
        {

            var quiz = await db.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                return View(new Quiz());
            }

            return View(quiz);
        }


        [Authorize(Roles = "Editor, Admin, SuperAdmin")]
        public async Task<IActionResult> _Results(int id)
        {
            var quiz = await db.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        [Authorize(Roles = "Editor, Admin, SuperAdmin")]
        public async Task<IActionResult> _Questions(int id)
        {
            var quiz = await db.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        [Authorize(Roles = "Editor, Admin, SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> _EditResult(int? id, int? quizid)
        {
            var quiz = await db.Quizzes.FindAsync(quizid);
            var result = quiz?.Results?.FirstOrDefault(x => x.Id == id);
            if (quiz == null)
            {
                return NotFound();
            }
            if(result == null)
            {
                result = new QuizResult
                {
                    Quiz = quiz,
                    QuizId = quiz.QuizId,
                    Id = 0,
                };

            }
            return View(result);
        }
        [Authorize(Roles = "Editor, Admin, SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> _EditResult(QuizResult r)
        {
            var quiz = await db.Quizzes.FindAsync(r.QuizId);
            var result = quiz?.Results?.FirstOrDefault(x => x.Id == r.Id);
            if(quiz == null)
            {
                return NotFound();
            }
            if (result == null)
            {
                await db.QuizResults.AddAsync(r);
                await db.SaveChangesAsync();
                foreach(var a in quiz.Questions?.SelectMany(x => x.Answers)?.ToList() ?? new List<QuizQuestionAnswer>())
                {
                    await db.AnswerResultWeights.AddAsync(new AnswerResultWeight
                    {
                        QuizQuestionAnswerId = a.Id,
                        QuizQuestionAnswer = a,
                        QuizResultId = r.Id,
                        QuizResult = r,
                        Weight = 50,
                    });
                }
                await db.SaveChangesAsync();
            }
            else
            {
                result.Title = r.Title;
                result.Content = r.Content;
                await db.SaveChangesAsync();
            }
            return Ok();
        }


        [Authorize(Roles = "Editor, Admin, SuperAdmin")]
        [HttpDelete]
        public async Task<IActionResult> _DeleteResult(int id)
        {
            var result = await db.QuizResults.FindAsync(id);
            var quiz = result?.Quiz;
            if (result == null) return NotFound();
            var weights = db.AnswerResultWeights.Where(x => x.QuizResultId == id);
            db.AnswerResultWeights.RemoveRange(weights);
            db.QuizResults.Remove(result);
            await db.SaveChangesAsync();
            return Ok();
        }

        // POST: Quizzes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Editor, Admin, SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuizId,Title,DraftTitle,Category,Content,DraftContent,CreatedOn,EditedOn,PublishedOn,Published,CoverImageUpload,DeleteCoverImage,ToPublish,FromAjax")] Quiz quiz)
        {

            var a = await db.Quizzes.FindAsync(quiz.QuizId);
            if(a == null)
            {
                string shortCode = "";
                do
                {
                    var generatedShortCode = Helpers.RandomString(10);
                    if (db.Quizzes.FirstOrDefault(x => x.UrlShortCode == generatedShortCode) == null) shortCode = generatedShortCode;
                } while (String.IsNullOrEmpty(shortCode)); //make sure the shortcode is not a dupe

                a = new Quiz()
                {
                    CreatedOn = DateTime.UtcNow,
                    UrlShortCode = shortCode,
                    QuizAuthors = new List<QuizAuthor>
                    {
                        new QuizAuthor
                        {
                            User = currentUser,
                            Quiz = quiz,
                            IsPrimaryAuthor = true
                        }
                    }
                };
                db.Quizzes.Add(a);
            }
            a.DraftTitle = quiz.DraftTitle;
            a.DraftContent = quiz.DraftContent;
            a.EditedOn = DateTime.UtcNow;
            if(quiz.DeleteCoverImage || (quiz.CoverImageUpload != null && a.CoverImage != null))
            {
                try
                {
                    await _cloudStorage.DeleteFileAsync(a.CoverImage.StorageName);
                }
                catch (Exception e)
                {

                }
                var toRemove = a.CoverImage;
                a.CoverImage = null;
                db.BlobFiles.Remove(toRemove);
            }
            if(quiz.CoverImageUpload != null)
            {
                a.CoverImageUpload = quiz.CoverImageUpload;
                await UploadFile(a);
            }

            //var roles = await _userManager.GetRolesAsync(currentUser);


            a.Published = await _userManager.IsInAnyRoleAsync(currentUser, "Publisher")/*roles.Contains("SuperAdmin") || roles.Contains("Admin") */? quiz.Published : a.Published;
            
            if(quiz.ToPublish && User.IsInRole("Overwriter"))
            {
                if(a.PublishedOn == null) a.PublishedOn = DateTime.UtcNow;
                a.Content = quiz.DraftContent;
                a.Title = quiz.DraftTitle;
                a.OverwrittenOn = DateTime.UtcNow;
            }

            if (ModelState.IsValid)
            {


                try
                {
                    //db.Update(quiz);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuizExists(quiz.QuizId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (!quiz.FromAjax)
                {
                    return RedirectToAction("Details", new { id = a.QuizId });
                }
                else
                {
                    return Ok(new { id = a.QuizId });
                }
            }
            return View(quiz);
        }

        [HttpPost]
        [Authorize(Roles = "Editor, Admin, SuperAdmin")]
        public async Task<JsonResult> UploadFiles(int? id, List<IFormFile> files)
        {
            return new JsonResult(new { result = "success" });
        }

        // GET: Quizzes/Delete/5
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quiz = await db.Quizzes
                .FirstOrDefaultAsync(m => m.QuizId == id);
            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        // POST: Quizzes/Delete/5
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quiz = await db.Quizzes.FindAsync(id);
            db.Quizzes.Remove(quiz);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuizExists(int id)
        {
            return db.Quizzes.Any(e => e.QuizId == id);
        }

        //private async Task UploadFile(Quiz quiz)
        //{
        //    string fileNameForStorage = FormFileName(quiz.Title, quiz.CoverImageUpload.FileName);
        //    quiz.CoverImage = await _blobStorage.UploadFileToBlobAsync(quiz.CoverImageUpload, fileNameForStorage);
        //    await db.SaveChangesAsync();
        //}


        private async Task<BlobFile> UploadBlobFile(IFormFile file, string title)
        {
            string fileNameForStorage = FormFileName(title, file.FileName);
            return await _cloudStorage.UploadFileToBlobAsync(file, fileNameForStorage);
        }


        private async Task UploadFile(Quiz quiz)
        {
            string fileNameForStorage = FormFileName(quiz.Title, quiz.CoverImageUpload.FileName);
            quiz.CoverImage = await _cloudStorage.UploadFileToBlobAsync(quiz.CoverImageUpload, fileNameForStorage);
            await db.SaveChangesAsync();
        }

        private static string FormFileName(string title, string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var fileNameForStorage = $"{title.Replace(" ", "-")}-{DateTime.Now.ToString("yyyyMMddHHmmss")}{fileExtension}";
            return fileNameForStorage;
        }
    }
}
