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
using News_Website.Models.Admin;
using News_Website.Services;

namespace News_Website.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class AdminController : BaseController
    {
        private RoleManager<IdentityRole> _roleManager { get; set; }
        public AdminController(ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<BaseController> logger,
            ICloudStorage cloudStorage,
            RoleManager<IdentityRole> roleManager) : base(context, userManager, logger, cloudStorage)
        {
            _roleManager = roleManager;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var users = db.Users/*.Where(x => x.Id != currentUserId)*/?.ToList();
            return View(users);
        }
        [Authorize(Roles="SuperAdmin")]
        public async Task<IActionResult> EditRoles(string id)
        {
            var user = db.Users?.Find(id);
            if (user == null) return NotFound();
            var roles = await _userManager.GetRolesAsync(user);
            var u = new SetUserRolesViewModel
            {
                User = user,
                UserId = id,
                Viewer = roles.Contains("Viewer"),
                Editor = roles.Contains("Editor"),
                Overwriter = roles.Contains("Overwriter"),
                Publisher = roles.Contains("Publisher"),
                Admin = roles.Contains("Admin"),
                SuperAdmin = roles.Contains("SuperAdmin"),

            };
            return View(u);
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> EditRoles(string id, [Bind("UserId, Viewer, Editor, Overwriter, Publisher, Admin, SuperAdmin")]SetUserRolesViewModel roles)
        {
            var user = db.Users?.Find(id);
            if (user == null) return NotFound();

            List<string> newRoles = new List<string>();
            if (roles.Viewer) newRoles.Add("Viewer");
            if (roles.Editor) newRoles.Add("Editor");
            if (roles.Overwriter) newRoles.Add("Overwriter");
            if (roles.Publisher) newRoles.Add("Publisher");
            if (roles.Admin) newRoles.Add("Admin");
            if (User.IsInRole("SuperAdmin") && roles.SuperAdmin) newRoles.Add("SuperAdmin");


            var currentRoles = await _userManager.GetRolesAsync(user);

            var toRemove = currentRoles.Where(x => !newRoles.Contains(x));
            var toAdd = newRoles.Where(x => !currentRoles.Contains(x));

            await _userManager.RemoveFromRolesAsync(user, toRemove);
            await _userManager.AddToRolesAsync(user, toAdd);

            return RedirectToAction(nameof(Index));
        }
    }
}
