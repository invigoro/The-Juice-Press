using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace News_Website.Models
{
    public class Article
    {
        public int ArticleId { get; set; }
        [StringLength(55)]
        //public string UrlShortCode { get; set; }
        //[StringLength(1000)]
        public string Title { get; set; }
        public string Content { get; set; }
        public string DraftContent { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime EditedOn { get; set; } = DateTime.UtcNow;
        public DateTime? PublishedOn { get; set; } = DateTime.UtcNow;
        public bool Published { get; set; } = false;
        [NotMapped]
        public User PrimaryAuthor
        {
            get
            {
                return this.ArticleAuthors?.FirstOrDefault(x => x.IsPrimaryAuthor)?.User;
            }
        }
        [NotMapped]
        public bool ToPublish { get; set; }
        public virtual List<ArticleAuthor> ArticleAuthors { get; set; }
        public int TotalViews { get; set; }
        [NotMapped]
        public string DraftContentEncoded { get; set; }
    }

    public class ArticleAuthor
    {
        public virtual Article Article { get; set; }
        public int ArticleId { get; set; }
        public virtual User User { get; set; }
        public string UserId { get; set; }
        public bool IsPrimaryAuthor { get; set; } = false;
    }
}
