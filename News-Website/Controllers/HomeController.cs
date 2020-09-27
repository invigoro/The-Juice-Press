using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using News_Website.Data;
using News_Website.Models;
using News_Website.Services;

namespace News_Website.Controllers
{
    public class HomeController : BaseController
    {
        
        public HomeController(ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<BaseController> logger,
            ICloudStorage cloudStorage) : base(context, userManager, logger, cloudStorage)
        {
        }

        public IActionResult Index(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                id = id.ToUpper();
                var article = db.Articles?.FirstOrDefault(x => x.UrlShortCode == id && x.Published);
                if(article != null)
                {
                    return RedirectToAction("Details", "Articles", new { id = article.ArticleId });
                }
            }
            var articles = db.Articles?.Where(x => x.Published && x.PublishedOn != null)?
                .OrderByDescending(x => x.PublishedOn)?
                .Take(10)
                .OrderByDescending(x => x.TotalViews)?
                .ToList();

            return View(articles);
        }

        public async Task<IActionResult> Search(string id)
        {
            var searchWords = id?.ToLower()?.Split(" ");
            var articles = db.Articles?.ToList();
            if (currentUser == null || !((await _userManager.GetRolesAsync(currentUser))?.Count() > 0)) { articles = articles?.Where(x => x.Published)?.ToList(); }
            if (!String.IsNullOrEmpty(id) && searchWords?.Count() > 0)
            {
                articles = articles.Where(x => x.Title.ToLower().ContainsAll(searchWords))?.ToList();
            }
            var quizzes = db.Quizzes?.ToList();
            if (currentUser == null || !((await _userManager.GetRolesAsync(currentUser))?.Count() > 0)) { quizzes = quizzes?.Where(x => x.Published)?.ToList(); }
            if (!String.IsNullOrEmpty(id) && searchWords?.Count() > 0)
            {
                quizzes = quizzes.Where(x => x.Title.ToLower().ContainsAll(searchWords))?.ToList();
            }
            ViewBag.ResultsTitle = $"Search results for <i>{id}</i>";

            List<AContent> AllContent = new List<AContent>();
            AllContent.AddRange(articles);
            AllContent.AddRange(quizzes);
            AllContent = AllContent.OrderByDescending(x => x.PublishedOn)?.ToList();
            return View(AllContent);
        }
        public IActionResult TermsOfUse()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Disclaimer()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Register() { return LocalRedirect("/Identity/Account/Register"); }
        public IActionResult SignUp() { return LocalRedirect("/Identity/Account/Register"); }
        public IActionResult LogIn() { return LocalRedirect("/Identity/Account/Login"); }
        public IActionResult SignIn() { return LocalRedirect("/Identity/Account/Login"); }
    }
}
