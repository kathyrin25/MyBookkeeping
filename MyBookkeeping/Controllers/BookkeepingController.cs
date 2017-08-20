using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyBookkeeping.Models;
using MyBookkeeping.ViewModels;

namespace MyBookkeeping.Controllers
{
    public class BookkeepingController : Controller
    {
        private BookkeepingContext db = new BookkeepingContext();

        // GET: Bookkeeping
        public ActionResult Index()
        {
            return View(db.MyBookkeepings.ToList());
        }

        // GET: Bookkeeping/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bookkeeping bookkeeping = db.MyBookkeepings.Find(id);
            if (bookkeeping == null)
            {
                return HttpNotFound();
            }
            return View(bookkeeping);
        }

        // GET: Bookkeeping/Create
        public ActionResult AddRecord()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult List()
        {            
            return View(db.MyBookkeepings.OrderBy(x=>x.sn).ToList());
        }      

        // POST: Bookkeeping/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRecord([Bind(Include = "Id,sn,Date,Type,Amount,Remark")] AddRecordViewModel bookkeepingData)
        {
            if (ModelState.IsValid)
            {
                var bookkeeping = new Bookkeeping
                {
                    Id = Guid.NewGuid(),
                    Date = bookkeepingData.Date,
                    Amount = bookkeepingData.Amount,
                    Remark = bookkeepingData.Remark,
                    Type = bookkeepingData.Type
                };
                
                db.MyBookkeepings.Add(bookkeeping);
                db.SaveChanges();
                return RedirectToAction("AddRecord");
            }

            return View(bookkeepingData);
        }

        // GET: Bookkeeping/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bookkeeping bookkeeping = db.MyBookkeepings.Find(id);
            if (bookkeeping == null)
            {
                return HttpNotFound();
            }
            return View(bookkeeping);
        }

        // POST: Bookkeeping/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,sn,Date,Type,Amount,Remark")] Bookkeeping bookkeeping)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookkeeping).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bookkeeping);
        }

        // GET: Bookkeeping/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bookkeeping bookkeeping = db.MyBookkeepings.Find(id);
            if (bookkeeping == null)
            {
                return HttpNotFound();
            }
            return View(bookkeeping);
        }

        // POST: Bookkeeping/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Bookkeeping bookkeeping = db.MyBookkeepings.Find(id);
            db.MyBookkeepings.Remove(bookkeeping);
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
