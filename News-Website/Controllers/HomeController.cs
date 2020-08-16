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

namespace News_Website.Controllers
{
    public class HomeController : BaseController
    {
        
        public HomeController(ApplicationDbContext context, 
            UserManager<User> userManager, 
            ILogger<BaseController> logger) : base(context, userManager, logger)
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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
