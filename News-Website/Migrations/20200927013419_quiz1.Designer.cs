﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using News_Website.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace News_Website.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200927013419_quiz1")]
    partial class quiz1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("News_Website.Models.AnswerResultWeight", b =>
                {
                    b.Property<int>("QuizQuestionAnswerId")
                        .HasColumnType("integer");

                    b.Property<int>("QuizResultId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Weight")
                        .HasColumnType("numeric");

                    b.HasKey("QuizQuestionAnswerId", "QuizResultId");

                    b.HasIndex("QuizResultId");

                    b.ToTable("AnswerResultWeights");
                });

            modelBuilder.Entity("News_Website.Models.Article", b =>
                {
                    b.Property<int>("ArticleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("Category")
                        .HasColumnType("integer");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<int?>("CoverImageId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DraftContent")
                        .HasColumnType("text");

                    b.Property<int?>("DraftCoverImageId")
                        .HasColumnType("integer");

                    b.Property<string>("DraftTitle")
                        .HasColumnType("character varying(1000)")
                        .HasMaxLength(1000);

                    b.Property<DateTime>("EditedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("OverwrittenOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("Published")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("PublishedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Title")
                        .HasColumnType("character varying(1000)")
                        .HasMaxLength(1000);

                    b.Property<int>("TotalViews")
                        .HasColumnType("integer");

                    b.Property<string>("UrlShortCode")
                        .IsRequired()
                        .HasColumnType("character varying(10)")
                        .HasMaxLength(10);

                    b.HasKey("ArticleId");

                    b.HasAlternateKey("UrlShortCode");

                    b.HasIndex("CoverImageId");

                    b.HasIndex("DraftCoverImageId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("News_Website.Models.ArticleAuthor", b =>
                {
                    b.Property<int>("ArticleId")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<bool>("IsPrimaryAuthor")
                        .HasColumnType("boolean");

                    b.HasKey("ArticleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("ArticleAuthors");
                });

            modelBuilder.Entity("News_Website.Models.ArticleBlobFile", b =>
                {
                    b.Property<int>("ArticleId")
                        .HasColumnType("integer");

                    b.Property<int>("BlobFileId")
                        .HasColumnType("integer");

                    b.HasKey("ArticleId", "BlobFileId");

                    b.HasIndex("BlobFileId");

                    b.ToTable("ArticleBlobFile");
                });

            modelBuilder.Entity("News_Website.Models.BlobFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AltText")
                        .HasColumnType("character varying(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("Description")
                        .HasColumnType("character varying(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("StorageName")
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("BlobFiles");
                });

            modelBuilder.Entity("News_Website.Models.Quiz", b =>
                {
                    b.Property<int>("QuizId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("Category")
                        .HasColumnType("integer");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<int?>("CoverImageId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DraftContent")
                        .HasColumnType("text");

                    b.Property<string>("DraftTitle")
                        .HasColumnType("character varying(1000)")
                        .HasMaxLength(1000);

                    b.Property<DateTime>("EditedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("OverwrittenOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("Published")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("PublishedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Title")
                        .HasColumnType("character varying(1000)")
                        .HasMaxLength(1000);

                    b.Property<int>("TotalViews")
                        .HasColumnType("integer");

                    b.Property<string>("UrlShortCode")
                        .HasColumnType("character varying(10)")
                        .HasMaxLength(10);

                    b.HasKey("QuizId");

                    b.HasIndex("CoverImageId");

                    b.ToTable("Quizzes");
                });

            modelBuilder.Entity("News_Website.Models.QuizAuthor", b =>
                {
                    b.Property<int>("QuizId")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<bool>("IsPrimaryAuthor")
                        .HasColumnType("boolean");

                    b.HasKey("QuizId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("QuizAuthors");
                });

            modelBuilder.Entity("News_Website.Models.QuizBlobFile", b =>
                {
                    b.Property<int>("QuizId")
                        .HasColumnType("integer");

                    b.Property<int>("BlobFileId")
                        .HasColumnType("integer");

                    b.HasKey("QuizId", "BlobFileId");

                    b.HasIndex("BlobFileId");

                    b.ToTable("QuizBlobFile");
                });

            modelBuilder.Entity("News_Website.Models.QuizQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.Property<string>("Question")
                        .HasColumnType("character varying(2000)")
                        .HasMaxLength(2000);

                    b.Property<int?>("QuizId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("QuizId");

                    b.ToTable("QuizQuestions");
                });

            modelBuilder.Entity("News_Website.Models.QuizQuestionAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("QuestionId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuizQuestionAnswers");
                });

            modelBuilder.Entity("News_Website.Models.QuizResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("QuizId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .HasColumnType("character varying(1000)")
                        .HasMaxLength(1000);

                    b.HasKey("Id");

                    b.HasIndex("QuizId");

                    b.ToTable("QuizResults");
                });

            modelBuilder.Entity("News_Website.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<string>("Email")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<string>("LastName")
                        .HasColumnType("character varying(255)")
                        .HasMaxLength(255);

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<int?>("ProfileImageId")
                        .HasColumnType("integer");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.HasIndex("ProfileImageId");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("News_Website.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("News_Website.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("News_Website.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("News_Website.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("News_Website.Models.AnswerResultWeight", b =>
                {
                    b.HasOne("News_Website.Models.QuizQuestionAnswer", "QuizQuestionAnswer")
                        .WithMany("AnswerWeights")
                        .HasForeignKey("QuizQuestionAnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("News_Website.Models.QuizResult", "QuizResult")
                        .WithMany("ResultWeights")
                        .HasForeignKey("QuizResultId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("News_Website.Models.Article", b =>
                {
                    b.HasOne("News_Website.Models.BlobFile", "CoverImage")
                        .WithMany()
                        .HasForeignKey("CoverImageId");

                    b.HasOne("News_Website.Models.BlobFile", "DraftCoverImage")
                        .WithMany()
                        .HasForeignKey("DraftCoverImageId");
                });

            modelBuilder.Entity("News_Website.Models.ArticleAuthor", b =>
                {
                    b.HasOne("News_Website.Models.Article", "Article")
                        .WithMany("ArticleAuthors")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("News_Website.Models.User", "User")
                        .WithMany("Articles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("News_Website.Models.ArticleBlobFile", b =>
                {
                    b.HasOne("News_Website.Models.Article", "Article")
                        .WithMany("ArticleBlobFiles")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("News_Website.Models.BlobFile", "BlobFile")
                        .WithMany()
                        .HasForeignKey("BlobFileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("News_Website.Models.Quiz", b =>
                {
                    b.HasOne("News_Website.Models.BlobFile", "CoverImage")
                        .WithMany()
                        .HasForeignKey("CoverImageId");
                });

            modelBuilder.Entity("News_Website.Models.QuizAuthor", b =>
                {
                    b.HasOne("News_Website.Models.Quiz", "Quiz")
                        .WithMany("QuizAuthors")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("News_Website.Models.User", "User")
                        .WithMany("Quizzes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("News_Website.Models.QuizBlobFile", b =>
                {
                    b.HasOne("News_Website.Models.BlobFile", "BlobFile")
                        .WithMany()
                        .HasForeignKey("BlobFileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("News_Website.Models.Quiz", "Quiz")
                        .WithMany("QuizBlobFiles")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("News_Website.Models.QuizQuestion", b =>
                {
                    b.HasOne("News_Website.Models.Quiz", "Quiz")
                        .WithMany("Questions")
                        .HasForeignKey("QuizId");
                });

            modelBuilder.Entity("News_Website.Models.QuizQuestionAnswer", b =>
                {
                    b.HasOne("News_Website.Models.QuizQuestion", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId");
                });

            modelBuilder.Entity("News_Website.Models.QuizResult", b =>
                {
                    b.HasOne("News_Website.Models.Quiz", "Quiz")
                        .WithMany("QuizResults")
                        .HasForeignKey("QuizId");
                });

            modelBuilder.Entity("News_Website.Models.User", b =>
                {
                    b.HasOne("News_Website.Models.BlobFile", "ProfileImage")
                        .WithMany()
                        .HasForeignKey("ProfileImageId");
                });
#pragma warning restore 612, 618
        }
    }
}
