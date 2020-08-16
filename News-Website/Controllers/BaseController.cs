using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using News_Website.Data;
using News_Website.Models;

namespace News_Website.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ApplicationDbContext db;
        private readonly UserManager<User> _userManager;
        protected readonly ILogger<BaseController> _logger;

        protected User currentUser
        {
            get { return _userManager.GetUserAsync(this.User).Result; }
        }
        protected string currentUserId { get { return currentUser?.Id; } }
        public BaseController(ApplicationDbContext context, UserManager<User> userManager, ILogger<BaseController> logger)
        {
            db = context;
            _userManager = userManager;
            _logger = logger;
        }
    }
}