﻿using System;
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
            ILogger<BaseController> logger,
            ICloudStorage cloudStorage) : base(context, userManager, logger, cloudStorage)
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
            var categories = EnumHelper<ArticleCategory>.GetDisplayValues(ArticleCategory.Entertainment)?.Select(x => x.ToLower());
            if(id == "latest") { articles = articles.OrderByDescending(x => x.PublishedOn)?.ToList();
                ViewBag.ResultsTitle = $"Latest News";
            }
            else if (categories.Contains(id)) { 
                articles = articles.Where(x => x.Category != null)?.ToList()?.Where(x => EnumHelper<ArticleCategory>.GetDisplayValue((ArticleCategory)x.Category).ToLower() == id)?.ToList();
                ViewBag.ResultsTitle = $"Latest {char.ToUpper(id[0]) + id.Substring(1)}";
            }
            if (currentUser == null || !((await _userManager.GetRolesAsync(currentUser))?.Count()  > 0)) { articles = articles?.Where(x => x.Published)?.ToList(); }
            articles = articles.Take(50)?.ToList();
            return View(nameof(Index), articles);
        }

        public async Task<IActionResult> Search(string id)
        {
            var searchWords = id?.ToLower()?.Split(" ");
            var articles = db.Articles?.ToList();
            if (currentUser == null || !((await _userManager.GetRolesAsync(currentUser))?.Count() > 0)) { articles = articles?.Where(x => x.Published)?.ToList(); }
            if (!String.IsNullOrEmpty(id) && searchWords?.Count() > 0)
            {
                articles = articles.Where(x => !String.IsNullOrEmpty(x.Title) && x.Title.ToLower().ContainsAll(searchWords))?.ToList();
            }
            ViewBag.ResultsTitle = $"Search results for <i>{id}</i>";
            return View(nameof(Index), articles);
        }

        // GET: Articles/Details/5
        [HttpGet("articles/read/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            int Id;
            var validId = Int32.TryParse(id, out Id);
            if(!validId)
            {
                try
                {
                    id = id?.Trim()?.Split("-")?.Last()?.Trim()?.ToUpper() ?? "";
                    Id = (await db.Articles
                        .FirstOrDefaultAsync(x => id == x.UrlShortCode))?
                        .ArticleId ?? 0;

                }catch(Exception e)
                {
                    return NotFound();
                }
            }


            var article = await db.Articles
                .FirstOrDefaultAsync(m => m.ArticleId == Id);
            if (article == null)
            {
                return NotFound();
            }


            if(currentUser == null) //add views for non author users
            {
                article.TotalViews++;
                await db.SaveChangesAsync();
            }
            if (article.CoverImage != null) ViewData["CoverImage"] = article.CoverImage.Url;
            ViewData["Title"] = article.Title;
            ViewData["IsArticle"] = true;
            if (article.Published)
            {
                return View(article);
            }
            if (currentUser != null && User.IsInAnyRole())
            {
                return View(article);
            }
            else
            {
                return LocalRedirect($"/Identity/Account/Login?returnUrl={HttpContext.Request.Path.ToUriComponent()}");
            }
        }


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
        public async Task<IActionResult> Edit(int id, [Bind("ArticleId,Title,DraftTitle,Category,Content,DraftContent,CreatedOn,EditedOn,PublishedOn,Published,CoverImageUpload,DeleteCoverImage,ToPublish,FromAjax")] Article article)
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
            a.DraftTitle = article.DraftTitle;
            a.DraftContent = article.DraftContent;
            a.EditedOn = DateTime.UtcNow;
            a.Category = article.Category == null ? (ArticleCategory?)null : article.Category;
            if(article.DeleteCoverImage || (article.CoverImageUpload != null && a.CoverImage != null))
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
            if(article.CoverImageUpload != null)
            {
                a.CoverImageUpload = article.CoverImageUpload;
                await UploadFile(a);
            }

            //var roles = await _userManager.GetRolesAsync(currentUser);


            a.Published = await _userManager.IsInAnyRoleAsync(currentUser, "Publisher")/*roles.Contains("SuperAdmin") || roles.Contains("Admin") */? article.Published : a.Published;
            
            if(article.ToPublish && User.IsInRole("Overwriter"))
            {
                if(a.PublishedOn == null) a.PublishedOn = DateTime.UtcNow;
                a.Content = article.DraftContent;
                a.Title = article.DraftTitle;
                a.OverwrittenOn = DateTime.UtcNow;
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

        //private async Task UploadFile(Article article)
        //{
        //    string fileNameForStorage = FormFileName(article.Title, article.CoverImageUpload.FileName);
        //    article.CoverImage = await _blobStorage.UploadFileToBlobAsync(article.CoverImageUpload, fileNameForStorage);
        //    await db.SaveChangesAsync();
        //}

        private async Task UploadFile(Article article)
        {
            string fileNameForStorage = FormFileName(article.Title ?? article.DraftTitle ?? "untitled", article.CoverImageUpload.FileName);
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
