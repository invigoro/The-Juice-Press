using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News_Website.Models.Admin
{
    public class SetUserRolesViewModel
    {
        public virtual User User { get; set; }
        public string UserId { get; set; }
        public bool Viewer { get; set; }
        public bool Editor { get; set; }
        public bool Admin { get; set; }
        public bool SuperAdmin { get; set; }
    }
}
