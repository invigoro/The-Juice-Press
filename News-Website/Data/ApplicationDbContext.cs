using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using News_Website.Models;

namespace News_Website.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleAuthor> ArticleAuthors { get; set; }
        public DbSet<BlobFile> BlobFiles { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QuizAuthor> QuizAuthors { get; set; }
        public DbSet<QuizResult> QuizResults { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        public DbSet<QuizQuestionAnswer> QuizQuestionAnswers { get; set; }
        public DbSet<AnswerResultWeight> AnswerResultWeights { get; set; }

        public DbSet<QuizResponse> QuizResponses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ArticleAuthor>()
                .HasKey(s => new { s.ArticleId, s.UserId });
            builder.Entity<QuizAuthor>()
                .HasKey(s => new { s.QuizId, s.UserId });
            builder.Entity<ArticleAuthor>()
                .HasOne(x => x.User)
                .WithMany(x => x.Articles)
                .HasForeignKey(x => x.UserId);
            builder.Entity<AnswerResultWeight>()
                .HasKey(s => new { s.QuizQuestionAnswerId, s.QuizResultId });
            builder.Entity<AnswerResultWeight>()
                .HasOne(x => x.QuizQuestionAnswer)
                .WithMany(x => x.AnswerWeights)
                .HasForeignKey(x => x.QuizQuestionAnswerId);
            builder.Entity<AnswerResultWeight>()
                .HasOne(x => x.QuizResult)
                .WithMany(x => x.ResultWeights)
                .HasForeignKey(x => x.QuizResultId);
            builder.Entity<QuizAuthor>()
                .HasOne(x => x.User)
                .WithMany(x => x.Quizzes)
                .HasForeignKey(x => x.UserId);
            builder.Entity<ArticleBlobFile>()
                .HasKey(s => new { s.ArticleId, s.BlobFileId });
            builder.Entity<QuizBlobFile>()
                .HasKey(s => new { s.QuizId, s.BlobFileId });
            builder.Entity<ArticleBlobFile>()
                .HasOne(x => x.Article)
                .WithMany(x => x.ArticleBlobFiles)
                .HasForeignKey(x => x.ArticleId);
            builder.Entity<QuizBlobFile>()
                .HasOne(x => x.Quiz)
                .WithMany(x => x.QuizBlobFiles)
                .HasForeignKey(x => x.QuizId);
            builder.Entity<Article>()
                .HasAlternateKey(x => x.UrlShortCode);
            //builder.Entity<IdentityRole>().HasData(
            //    new IdentityRole("Admin"),
            //    new IdentityRole("SuperAdmin"),
            //    new IdentityRole("User"),
            //    new IdentityRole("Author")
            // );
        }
    }
}
