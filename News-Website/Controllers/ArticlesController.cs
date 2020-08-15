﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using News_Website.Data;
using News_Website.Models;

namespace News_Website.Controllers
{
    public class ArticlesController : BaseController
    {
        //private readonly ApplicationDbContext db;

        public ArticlesController(ApplicationDbContext context, UserManager<User> userManager) : base(context, userManager)
        {
        }

        // GET: Articles
        public async Task<IActionResult> Index(string id)
        {
            return View(await db.Articles.ToListAsync());
        }

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await db.Articles
                .FirstOrDefaultAsync(m => m.ArticleId == id);
            if (article == null)
            {
                return NotFound();
            }

            if(currentUser == null) //add views for non author users
            {
                article.TotalViews++;
                await db.SaveChangesAsync();
            }
            ViewData["Title"] = article.Title;
            return View(article);
        }

        [Authorize]
        // GET: Articles/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        // POST: Articles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArticleId,Title,Content,DraftContent,CreatedOn,EditedOn,PublishedOn,Published")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Add(article);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(article);
        }

        // GET: Articles/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {

            var article = await db.Articles.FindAsync(id);
            if (article == null)
            {
                return View(new Article());
            }
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArticleId,Title,Content,DraftContent,CreatedOn,EditedOn,PublishedOn,Published")] Article article)
        {
            if (id != article.ArticleId || id == 0) //creating a new article
            {
                if (ModelState.IsValid)
                {
                    db.Add(article);
                    await db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(article);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(article);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.ArticleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(article);
        }

        // GET: Articles/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await db.Articles
                .FirstOrDefaultAsync(m => m.ArticleId == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Articles/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await db.Articles.FindAsync(id);
            db.Articles.Remove(article);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return db.Articles.Any(e => e.ArticleId == id);
        }
    }
}
