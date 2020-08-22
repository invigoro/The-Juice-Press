using System;
using System.Collections.Generic;
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
    public abstract class BaseController : Controller
    {
        protected readonly ApplicationDbContext db;
        protected readonly UserManager<User> _userManager;
        protected readonly ILogger<BaseController> _logger;
        protected readonly ICloudStorage _cloudStorage;
        //protected readonly BlobStorageService _blobStorage;

        protected User currentUser
        {
            get { return _userManager.GetUserAsync(this.User).Result; }
        }
        protected string currentUserId { get { return currentUser?.Id; } }
        public BaseController(ApplicationDbContext context, UserManager<User> userManager, ILogger<BaseController> logger, ICloudStorage cloudStorage)
        {
            db = context;
            _userManager = userManager;
            _logger = logger;
            //_cloudStorage = cloudStorage;
            _cloudStorage = cloudStorage;
        }
    }
}