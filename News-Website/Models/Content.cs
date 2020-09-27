using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace News_Website.Models
{
    public abstract class AContent
    {
        [StringLength(1000)]
        public string Title { get; set; }
        [StringLength(1000)]
        [Display(Name = "Title")]
        public string DraftTitle { get; set; }
        public string Content { get; set; }
        [Display(Name = "Content")]
        public string DraftContent { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime EditedOn { get; set; } = DateTime.UtcNow;
        public DateTime? PublishedOn { get; set; }
        public DateTime? OverwrittenOn { get; set; }
        [Display(Name = "Publicly Viewable")]
        public bool Published { get; set; } = false;

        public int TotalViews { get; set; } = 0;
        [Display(Name = "Cover Photo")]
        public virtual BlobFile CoverImage { get; set; }
    }
}
