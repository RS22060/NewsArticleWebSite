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
    public class ParagraphsController : Controller
    {
        private NewsEntities db = new NewsEntities();

        public ActionResult Index()
        {
            var paragraph = db.Paragraph.Include(p => p.Article);
            return View(paragraph.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paragraph paragraph = db.Paragraph.Find(id);
            if (paragraph == null)
            {
                return HttpNotFound();
            }
            return View(paragraph);
        }

        public ActionResult Create()
        {
            ViewBag.ArticleId = new SelectList(db.Article, "ArticleId", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ParagraphId,Text,ArticleId")] Paragraph paragraph)
        {
            if (ModelState.IsValid)
            {
                db.Paragraph.Add(paragraph);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArticleId = new SelectList(db.Article, "ArticleId", "Title", paragraph.ArticleId);
            return View(paragraph);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paragraph paragraph = db.Paragraph.Find(id);
            if (paragraph == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArticleId = new SelectList(db.Article, "ArticleId", "Title", paragraph.ArticleId);
            return View(paragraph);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ParagraphId,Text,ArticleId")] Paragraph paragraph)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paragraph).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArticleId = new SelectList(db.Article, "ArticleId", "Title", paragraph.ArticleId);
            return View(paragraph);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paragraph paragraph = db.Paragraph.Find(id);
            if (paragraph == null)
            {
                return HttpNotFound();
            }
            return View(paragraph);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Paragraph paragraph = db.Paragraph.Find(id);
            db.Paragraph.Remove(paragraph);
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
