using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace News_Website.Models
{
    public class User : IdentityUser
    {
        [Display(Name = "Display Name")]
        [StringLength(255)]
        [Required]
        public string DisplayName { get; set; }
        [StringLength(255)]
        [PersonalData]
        public string FirstName { get; set; }
        [StringLength(255)]
        [PersonalData]
        public string LastName { get; set; }
        public virtual List<ArticleAuthor> Articles { get; set; }
        public virtual List<QuizAuthor> Quizzes { get; set; }
        [Display(Name = "Profile Image")]
        public virtual BlobFile ProfileImage { get; set; }
        [NotMapped]
        [Display(Name = "Upload New Profile Image")]
        public virtual IFormFile ProfileImageUpload { get; set; }

        //public string ProfilePicture { get; set; }
        [NotMapped]
        public string FullName { get { return FirstName + " " + LastName; } }
    }
}
