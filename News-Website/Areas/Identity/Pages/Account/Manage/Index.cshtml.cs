using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using News_Website.Data;
using News_Website.Models;
using News_Website.Services;

namespace News_Website.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        //private BlobStorageService _blobStorage;
        private ICloudStorage _cloudStorage;
        private readonly ApplicationDbContext db;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            //BlobStorageService blobStorage,
            ApplicationDbContext db,
            ICloudStorage cloudStorage)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //_blobStorage = blobStorage;
            _cloudStorage = cloudStorage;
            this.db = db;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            [Required]
            [Display(Name = "Display Name")]
            [StringLength(255)]
            public string DisplayName { get; set; }
            [StringLength(255)]
            [PersonalData]
            public string FirstName { get; set; }
            [StringLength(255)]
            [PersonalData]
            public string LastName { get; set; }
            [Display(Name = "Upload New Profile Image")]
            public virtual IFormFile ProfileImageUpload { get; set; }
            [Display(Name = "Profile Image")]
            public BlobFile ProfileImage { get; set; }
            public bool DeleteProfileImage { get; set; } = false;

        }

        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                DisplayName = user.DisplayName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfileImage = user.ProfileImage,

                
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            user.DisplayName = Input.DisplayName;
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;


            if (Input.DeleteProfileImage || (Input.ProfileImageUpload != null && user.ProfileImage != null))
            {
                try
                {
                    await _cloudStorage.DeleteFileAsync(user.ProfileImage.StorageName);
                }
                catch (Exception e)
                {

                }
                var toRemove = user.ProfileImage;
                user.ProfileImage = null;
                db.BlobFiles.Remove(toRemove);
            }
                if (Input.ProfileImageUpload != null)
            {
                user.ProfileImageUpload =  Input.ProfileImageUpload;
                await UploadFile(user);
            }

            await _userManager.UpdateAsync(user);
            

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
        private async Task UploadFile(User user)
        {
            string fileNameForStorage = FormFileName(user.FullName + " Profile Image", user.ProfileImageUpload.FileName);
            user.ProfileImage = await _cloudStorage.UploadFileToBlobAsync(user.ProfileImageUpload, fileNameForStorage);
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
