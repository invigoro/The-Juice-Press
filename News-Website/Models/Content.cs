using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
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
        [StringLength(10)]
        public string UrlShortCode { get; set; }
        [Display(Name = "Cover Photo")]
        public virtual BlobFile CoverImage { get; set; }

        public string UrlTitle()
        {
            string title = null;
            if (!String.IsNullOrEmpty(Title))
            {
                Regex remainder = new Regex(@"[^a-zA-Z0-9\s]");
                title = remainder.Replace(this.Title.ToLower(), string.Empty);
                Regex spaces = new Regex(@"[\s]");
                title = spaces.Replace(title, "-");
                if(title.Length > 50)
                {
                    var splitTitle = title.Split("-");
                    if(splitTitle[0].Length > 50) //if the first word is really fucking long for some reason
                    {
                        title = splitTitle[0].Substring(0, 50);
                    }
                    else
                    {
                        title = splitTitle[0];
                        for(int i = 1; i < splitTitle.Length && (title + "-" + splitTitle[i]).Length < 50; i++)
                        {
                            title += $"-{splitTitle[i]}";
                        }
                    }
                }
                if (!String.IsNullOrEmpty(Title))
                {
                    title += $"-{this.UrlShortCode.ToLower()}";
                }
            }
            if (String.IsNullOrEmpty(title)) {
                if(this is Article a)
                {
                    title = a.ArticleId.ToString();
                }
                if(this is Quiz q)
                {
                    title = q.QuizId.ToString();
                }
            }
            return title; 
        }
        
    }
}
