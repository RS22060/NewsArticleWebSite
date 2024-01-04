using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NewsArticleWebSite.Models;

namespace NewsArticleWebSite.Controllers
{
    public class ArticlesController : Controller
    {
        private NewsEntities db = new NewsEntities();

        public ActionResult Index()
        {
            var article = db.Article.Include(a => a.Author).Include(a => a.Category);
            return View(article.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Article.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(db.Author, "AuthorId", "Name");
            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArticleId,Title,Introduction,AuthorId,CategoryId")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Article.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorId = new SelectList(db.Author, "AuthorId", "Name", article.AuthorId);
            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "Title", article.CategoryId);
            return View(article);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Article.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorId = new SelectList(db.Author, "AuthorId", "Name", article.AuthorId);
            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "Title", article.CategoryId);
            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArticleId,Title,Introduction,AuthorId,CategoryId")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(db.Author, "AuthorId", "Name", article.AuthorId);
            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "Title", article.CategoryId);
            return View(article);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Article.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.Article.Find(id);
            db.Article.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
