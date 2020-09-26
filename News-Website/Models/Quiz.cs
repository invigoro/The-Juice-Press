using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace News_Website.Models
{
    public class Quiz
    {
        public int QuizId { get; set; }
        [StringLength(10)]
        public string UrlShortCode { get; set; }
        [StringLength(1000)]
        public string Title { get; set; }
        public string Content { get; set; }
        [Display(Name = "Content")]
        public string DraftContent { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime EditedOn { get; set; } = DateTime.UtcNow;
        public DateTime? PublishedOn { get; set; }
        public DateTime? OverwrittenOn { get; set; }
        [Display(Name = "Publicly Viewable")]
        public bool Published { get; set; } = false;
        [NotMapped]
        public User PrimaryAuthor
        {
            get
            {
                return this.QuizAuthors?.FirstOrDefault(x => x.IsPrimaryAuthor)?.User;
            }
        }
        [NotMapped]
        public bool ToPublish { get; set; }
        [NotMapped]
        public bool FromAjax { get; set; }
        public virtual List<QuizAuthor> QuizAuthors { get; set; }
        public int TotalViews { get; set; } = 0;
        [NotMapped]
        public string DraftContentEncoded { get; set; }
        public ArticleCategory? Category { get; set; }
        [Display(Name = "Cover Photo")]
        public virtual BlobFile CoverImage { get; set; }
        [NotMapped]
        [Display(Name = "Upload New Cover Image")]
        public virtual IFormFile CoverImageUpload { get; set; }
        public virtual List<QuizBlobFile> QuizBlobFiles { get; set; }
        [NotMapped]
        public List<int> CurrentBlobFiles { get; set; }
        [NotMapped]
        public bool DeleteCoverImage { get; set; } = false;
        public virtual List<QuizResult> QuizResults { get; set; }
        public virtual List<QuizQuestion> Questions { get; set; }

        public QuizResult GetResult(QuizResponseViewModel response)
        {
            var results = new Dictionary<QuizResult, decimal>();
            foreach (var a in response?.Answers ?? new List<QuizQuestionResponseViewModel>())
            {
                var question = this.Questions?.Find(x => x.Id == a.QuizQuestionId);
                var answer = question?.Answers?.Find(x => x.Id == a.QuizQuestionAnswerId);
                if (question == null
                    || answer == null
                    || answer?.AnswerWeights?.Count() != this.QuizResults?.Count()
                    || !(this.QuizResults?.Count() > 0))
                {
                    return null;
                }
                foreach (var w in answer.AnswerWeights)
                {
                    if(!results.ContainsKey(w.QuizResult))
                    {
                        results[w.QuizResult ] = 0;
                    }
                    results[w.QuizResult] += w.Weight;
                }
            }

            return results.ToList().OrderByDescending(x => x.Value).FirstOrDefault().Key;
        }
    }

    public class QuizResult
    {
        public int Id { get; set; }
        public virtual Quiz Quiz { get; set; }
        [StringLength(1000)]
        public string Title { get; set; }

    }

    public class QuizQuestion
    {
        public QuizQuestion()
        {
            Order = Quiz?.Questions?.Count() ?? 0;
        }
        public int Id { get; set; }
        public int Order { get; set; }
        public virtual Quiz Quiz { get; set; }
        [StringLength(2000)]
        public string Question { get; set; }
        public virtual List<QuizQuestionAnswer> Answers { get; set; }
    }
    
    public class QuizQuestionAnswer
    {
        public int Id { get; set; }
        public virtual QuizQuestion Question { get; set; }
        public virtual List<AnswerResultWeight> AnswerWeights { get; set; }
    }

    public class AnswerResultWeight
    {
        public int QuizQuestionAnswerId { get; set; }
        public virtual QuizQuestionAnswer QuizQuestionAnswer {get;set;}
        public int QuizResultId { get; set; }
        public virtual QuizResult QuizResult { get; set; }
        public decimal Weight { get; set; }
    }

    public class QuizBlobFile
    {
        public virtual Quiz Quiz { get; set; }
        public int QuizId { get; set; }
        public virtual BlobFile BlobFile { get; set; }
        public int BlobFileId { get; set; }
    }

    public class QuizAuthor
    {
        public virtual Quiz Quiz { get; set; }
        public int QuizId { get; set; }
        public virtual User User { get; set; }
        public string UserId { get; set; }
        public bool IsPrimaryAuthor { get; set; } = false;
    }

    public class QuizResponseViewModel
    {
         public List<QuizQuestionResponseViewModel> Answers { get; set; }
    }
    public class QuizQuestionResponseViewModel
    {
        public int QuizQuestionId { get; set; }
        public int QuizQuestionAnswerId { get; set; }
    }
}
