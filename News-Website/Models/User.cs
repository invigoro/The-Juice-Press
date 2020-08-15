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
        [StringLength(255)]
        public string DisplayName { get; set; }
        [StringLength(255)]
        [PersonalData]
        public string FirstName { get; set; }
        [StringLength(255)]
        [PersonalData]
        public string LastName { get; set; }
        public virtual List<ArticleAuthor> Articles { get; set; }

        public string ProfilePicture { get; set; }
        [NotMapped]
        public string FullName { get { return FirstName + " " + LastName; } }
    }
}
