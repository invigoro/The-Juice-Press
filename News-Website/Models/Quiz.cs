using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace News_Website.Models
{
    public class Quiz : AContent
    {
        public int QuizId { get; set; }
        [StringLength(10)]
        public string UrlShortCode { get; set; }
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
        [NotMapped]
        public string DraftContentEncoded { get; set; }
        [NotMapped]
        [Display(Name = "Upload New Cover Image")]
        public virtual IFormFile CoverImageUpload { get; set; }
        public virtual List<QuizBlobFile> QuizBlobFiles { get; set; }
        [NotMapped]
        public List<int> CurrentBlobFiles { get; set; }
        [NotMapped]
        public bool DeleteCoverImage { get; set; } = false;
        public virtual List<QuizResult> Results { get; set; }
        public virtual List<QuizQuestion> Questions { get; set; }
        public virtual List<QuizResponse> Responses { get; set; }

        public QuizResult GetResult(QuizResponseViewModel response)
        {
            var results = new Dictionary<QuizResult, decimal>();
            foreach (var a in response?.Answers ?? new List<QuizQuestionResponseViewModel>())
            {
                var question = this.Questions?.Find(x => x.Id == a.QuizQuestionId);
                var answer = question?.Answers?.Find(x => x.Id == a.QuizQuestionAnswerId);
                if (question == null
                    || answer == null
                    || answer?.AnswerWeights?.Count() != this.Results?.Count()
                    || !(this.Results?.Count() > 0))
                {
                    return null;
                }
                foreach (var w in answer.AnswerWeights)
                {
                    if (!results.ContainsKey(w.QuizResult))
                    {
                        results[w.QuizResult] = 0;
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
        public int QuizId { get; set; }
        [StringLength(1000)]
        public string Title { get; set; }
        [StringLength(5000)]
        [Display(Name = "Explanation of Result")]
        public string Content { get; set; }
        public virtual List<AnswerResultWeight> ResultWeights { get; set; }

    }

    public class QuizQuestion
    {
        public QuizQuestion() { }
        public QuizQuestion(Quiz quiz)
        {
            Quiz = quiz;
            Order = Quiz?.Questions?.Count() ?? 0;
        }
        public int Id { get; set; }
        public int Order { get; set; }
        public virtual Quiz Quiz { get; set; }
        public int QuizId { get; set; }
        [StringLength(2000)]
        public string Question { get; set; }
        public virtual List<QuizQuestionAnswer> Answers { get; set; }
    }

    public class QuizQuestionAnswer
    {
        public int Id { get; set; }
        public virtual QuizQuestion Question { get; set; }
        public int QuizQuestionId { get; set; }
        public virtual List<AnswerResultWeight> AnswerWeights { get; set; }

        [StringLength(2000)]
        public string Answer { get; set; }
    }

    public class AnswerResultWeight
    {
        public int QuizQuestionAnswerId { get; set; }
        public virtual QuizQuestionAnswer QuizQuestionAnswer { get; set; }
        public int QuizResultId { get; set; }
        public virtual QuizResult QuizResult { get; set; }
        [Range(0, 100)]
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

    public class QuizResponse
    {
        public int Id { get; set; }
        public string SessionId { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public int QuizId { get; set; }
        public virtual Quiz Quiz {get;set;}
        public int QuizResultId { get; set; }
        public virtual QuizResult QuizResult { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
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
