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
    public class ArticlesController : BaseController
    {
        //private readonly ApplicationDbContext db;

        public ArticlesController(ApplicationDbContext context, 
            UserManager<User> userManager, 
            ILogger<BaseController> logger, ICloudStorage cloudStorage) : base(context, userManager, logger, cloudStorage)
        {
        }

        // GET: Articles
        public async Task<IActionResult> Index(string id)
        {
            List<Article> articles;
            if(currentUser == null) { articles = await db.Articles?.Where(x => x.Published)?.ToListAsync(); }
            else {
                var roles = await _userManager.GetRolesAsync(currentUser);
                if (roles?.Count() > 0)
                {
                    articles = await db.Articles?.ToListAsync();
                }
                else
                {
                    articles = await db.Articles?.Where(x => x.Published)?.ToListAsync();
                }
            }
            return View(articles);
        }

        public async Task<IActionResult> List(string id)
        {
            id = id?.ToLower();
            List<Article> articles = await db.Articles?.ToListAsync(); 
            if(id == "latest") { articles = articles.OrderByDescending(x => x.PublishedOn)?.ToList(); }
            if (currentUser == null || !((await _userManager.GetRolesAsync(currentUser))?.Count()  > 0)) { articles = articles?.Where(x => x.Published)?.ToList(); }
            articles = articles.Take(50)?.ToList();
            return View(nameof(Index), articles);
        }

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await db.Articles
                .FirstOrDefaultAsync(m => m.ArticleId == id);
            if (article == null)
            {
                return NotFound();
            }

            if(currentUser == null) //add views for non author users
            {
                article.TotalViews++;
                await db.SaveChangesAsync();
            }
            ViewData["Title"] = article.Title;
            return View(article);
        }

        //[Authorize]
        //// GET: Articles/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[Authorize]
        //// POST: Articles/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ArticleId,Title,Content,DraftContent,CreatedOn,EditedOn,PublishedOn,Published")] Article article)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Add(article);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(article);
        //}

        // GET: Articles/Edit/5
        [Authorize(Roles = "Editor, Admin, SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {

            var article = await db.Articles.FindAsync(id);
            if (article == null)
            {
                return View(new Article());
            }
            
            return View(article);
        }
        [Authorize(Roles = "Editor, Admin, SuperAdmin")]
        public async Task<IActionResult> _Edit(int? id)
        {

            var article = await db.Articles.FindAsync(id);
            if (article == null)
            {
                return View(new Article());
            }

            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Editor, Admin, SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArticleId,Title,Category,Content,DraftContent,CreatedOn,EditedOn,PublishedOn,Published,CoverImageUpload,ToPublish,FromAjax")] Article article)
        {

            var a = await db.Articles.FindAsync(article.ArticleId);
            if(a == null)
            {
                string shortCode = "";
                do
                {
                    var generatedShortCode = Helpers.RandomString(10);
                    if (db.Articles.FirstOrDefault(x => x.UrlShortCode == generatedShortCode) == null) shortCode = generatedShortCode;
                } while (String.IsNullOrEmpty(shortCode)); //make sure the shortcode is not a dupe

                a = new Article()
                {
                    CreatedOn = DateTime.UtcNow,
                    UrlShortCode = shortCode,
                    ArticleAuthors = new List<ArticleAuthor>
                    {
                        new ArticleAuthor
                        {
                            User = currentUser,
                            Article = article,
                            IsPrimaryAuthor = true
                        }
                    }
                };
                db.Articles.Add(a);
            }
            a.Title = article.Title;
            a.DraftContent = article.DraftContent;
            a.EditedOn = DateTime.UtcNow;
            a.Category = article.Category == null ? (ArticleCategory?)null : article.Category;

            if(article.CoverImageUpload != null)
            {
                a.CoverImageUpload = article.CoverImageUpload;
                if(a.CoverImage != null)
                {
                    try
                    {
                        await _cloudStorage.DeleteFileAsync(a.CoverImage.StorageName);
                    }
                    catch(Exception e)
                    {

                    }
                    var toRemove = a.CoverImage;
                    a.CoverImage = null;
                    db.BlobFiles.Remove(toRemove);
                }
                await UploadFile(a);
            }

            //var roles = await _userManager.GetRolesAsync(currentUser);


            a.Published = await _userManager.IsInAnyRoleAsync(currentUser, "SuperAdmin,Admin")/*roles.Contains("SuperAdmin") || roles.Contains("Admin") */? article.Published : a.Published;
            
            if(article.ToPublish)
            {
                a.PublishedOn = DateTime.UtcNow;
                a.Content = article.DraftContent;
            }

            if (ModelState.IsValid)
            {


                try
                {
                    //db.Update(article);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.ArticleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (!article.FromAjax)
                {
                    return RedirectToAction("Details", new { id = a.ArticleId });
                }
                else
                {
                    return Ok(new { id = a.ArticleId });
                }
            }
            return View(article);
        }

        [HttpPost]
        [Authorize(Roles = "Editor, Admin, SuperAdmin")]
        public async Task<JsonResult> UploadFiles(int? id, List<IFormFile> files)
        {
            return new JsonResult(new { result = "success" });
        }

        // GET: Articles/Delete/5
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await db.Articles
                .FirstOrDefaultAsync(m => m.ArticleId == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Articles/Delete/5
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await db.Articles.FindAsync(id);
            db.Articles.Remove(article);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return db.Articles.Any(e => e.ArticleId == id);
        }

        private async Task UploadFile(Article article)
        {
            string fileNameForStorage = FormFileName(article.Title, article.CoverImageUpload.FileName);
            article.CoverImage = await _cloudStorage.UploadFileToBlobAsync(article.CoverImageUpload, fileNameForStorage);
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
