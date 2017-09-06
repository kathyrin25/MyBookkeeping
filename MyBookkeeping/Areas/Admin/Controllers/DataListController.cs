using MyBookkeeping.Repositories;
using MyBookkeeping.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBookkeeping.Areas.Admin.ViewModels;
using System.Net;
using MyBookkeeping.Models;
using PagedList;

namespace MyBookkeeping.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DataListController : Controller
    {
        private readonly BookkeepingService _BookkeepingSvc;
        private readonly BookkeepingLogService _LogSvc;

        public DataListController()
        {
            var unitOfWork = new EFUnitOfWork();
            _BookkeepingSvc = new BookkeepingService(unitOfWork);
            _LogSvc = new BookkeepingLogService(unitOfWork);
        }
        // GET: Admin/DataList
        public ActionResult Index(DateTime? StartDate, DateTime? EndDate, int? Page)
        {
            if (Page.HasValue)
            {
                ViewData["CurrentPage"] = Page;
            }

            //預設帶最近一個月的日期
            if (!StartDate.HasValue || !EndDate.HasValue)
            {
                StartDate = DateTime.Today.AddMonths(-1);
                EndDate = DateTime.Today;
            }
            
            var result = new QueryDataViewModel
            {
                StartDate = StartDate.Value,
                EndDate = EndDate.Value
            };
            return View(result);
        }

        [ChildActionOnly]
        public ActionResult List(int? page, DateTime? SDate, DateTime? EDate)
        {
            int pageSize = 10;
            int currenPage = (!page.HasValue || page < 1) ? 1 : page.Value;

            var data = _BookkeepingSvc.Lookup();

            if (SDate.HasValue && EDate.HasValue)
            {
                data = data.Where(x => x.Date >= SDate && x.Date <= EDate);
            }

            //資料撈出來後放到view model裡 , 並加入分頁
            var result = data.Select(c => new ListDataViewModel()
            {
                Id = c.Id,
                Date = c.Date,
                Amount = c.Amount,
                Remark = c.Remark,
                Type = c.Type
            }).OrderByDescending(x => x.Date).ToPagedList(currenPage, pageSize);

            ViewData["SD"] = SDate;
            ViewData["ED"] = EDate;

            return View(result);
        }


        // GET: Admin/DataList/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Bookkeeping bookkeeping = _BookkeepingSvc.GetSingle(id.Value);
            if (bookkeeping == null)
            {
                return HttpNotFound();
            }
            else
            {
                EditDataViewModel EditDetail = new EditDataViewModel
                {
                    Id = bookkeeping.Id,
                    sn = bookkeeping.sn,
                    Date = bookkeeping.Date,
                    Amount = bookkeeping.Amount,
                    Type = bookkeeping.Type,
                    Remark = bookkeeping.Remark
                };

                return View(EditDetail);
            }

        }

        // POST: Admin/DataList/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,sn,Date,Type,Amount,Remark")] Bookkeeping bookkeeping)
        {
            var oldData = _BookkeepingSvc.GetSingle(bookkeeping.Id);
            if (oldData != null && ModelState.IsValid)
            {
                _BookkeepingSvc.Edit(bookkeeping, oldData);
                _BookkeepingSvc.Save();

                _LogSvc.Add(bookkeeping.Id, "Edit");
                _LogSvc.Save();

                return RedirectToAction("Index");
            }
            return View(bookkeeping);
        }

        // GET: Admin/DataList/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Bookkeeping bookkeeping = _BookkeepingSvc.GetSingle(id.Value);
            if (bookkeeping == null)
            {
                return HttpNotFound();
            }
            return View(bookkeeping);
        }

        // POST: Admin/DataList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Bookkeeping bookkeeping = _BookkeepingSvc.GetSingle(id);
            _BookkeepingSvc.Delete(bookkeeping);
            _BookkeepingSvc.Save();
            _LogSvc.Add(id, "Delete");
            _LogSvc.Save();
            return RedirectToAction("Index");
        }
    }
}
