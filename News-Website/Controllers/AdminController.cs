using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using News_Website.Data;
using News_Website.Models;

namespace News_Website.Controllers
{
    public class AdminController : BaseController
    {
        private RoleManager<IdentityRole> _roleManager { get; set; }
        public AdminController(ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<BaseController> logger, RoleManager<IdentityRole> roleManager ) : base(context, userManager, logger)
        {
            _roleManager = roleManager;
        }
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> SeedSuperAdmin()
        {
            var roles = db.Roles?.ToList();
            
            foreach (var r in roles)
            {
                var userrole = db.UserRoles.FirstOrDefault(x => x.RoleId == r.Id && x.UserId == currentUserId);
                if (userrole == null)
                {
                    var toAdd = new IdentityUserRole<string> { UserId = currentUserId, RoleId = r.Id };
                    db.UserRoles.Add(toAdd);
                }
            }
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
