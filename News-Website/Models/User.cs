using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace News_Website.Models
{
    public class User : IdentityUser
    {
        [StringLength(255)]
        public string DisplayName { get; set; }
        [StringLength(255)]
        public string FirstName { get; set; }
        [StringLength(255)]
        public string LastName { get; set; }
        public virtual List<ArticleAuthor> Articles { get; set; }
    }
}
