using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace News_Website.Models
{
    public class Article : AContent
    {
        public int ArticleId { get; set; }
        
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
        [NotMapped]
        public bool FromAjax { get; set; }
        public virtual List<ArticleAuthor> ArticleAuthors { get; set; }
        [NotMapped]
        public string DraftContentEncoded { get; set; }
        public ArticleCategory? Category { get; set; }

        [Display(Name = "Cover Photo")]
        public virtual BlobFile DraftCoverImage { get; set; }
        [NotMapped]
        [Display(Name = "Upload New Cover Image")]
        public virtual IFormFile CoverImageUpload { get; set; }
        public virtual List<ArticleBlobFile> ArticleBlobFiles { get; set; }
        [NotMapped]
        public List<int> CurrentBlobFiles { get; set; }
        [NotMapped]
        public bool DeleteCoverImage { get; set; } = false;
    }

    public enum ArticleCategory
    {
        [Display(Name = "Politics")]
        Politics = 1000,
        [Display(Name = "Sports")]
        Sports = 2000,
        [Display(Name = "Entertainment")]
        Entertainment = 3000,
        [Display(Name = "Opinion")]
        Opinion = 4000,
    }

    public class ArticleBlobFile
    {
        public virtual Article Article { get; set; }
        public int ArticleId { get; set; }
        public virtual BlobFile BlobFile { get; set; }
        public int BlobFileId { get; set; }
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
