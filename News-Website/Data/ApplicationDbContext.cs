using System;
using System.Collections.Generic;
using System.Text;
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ArticleAuthor>()
                .HasKey(s => new { s.ArticleId, s.UserId });
            builder.Entity<ArticleAuthor>()
                .HasOne(x => x.User)
                .WithMany(x => x.Articles)
                .HasForeignKey(x => x.UserId);
        }
    }
}
