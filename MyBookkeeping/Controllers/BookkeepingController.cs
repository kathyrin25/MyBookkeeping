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
using MyBookkeeping.Service;
using MyBookkeeping.Repositories;
using MyBookkeeping.ValidateAttribute;

namespace MyBookkeeping.Controllers
{
    public class BookkeepingController : Controller
    {
        private readonly BookkeepingService _BookkeepingSvc;
        private readonly BookkeepingLogService _LogSvc;

        public BookkeepingController()
        {
            var unitOfWork = new EFUnitOfWork();
            _BookkeepingSvc = new BookkeepingService(unitOfWork);
            _LogSvc = new BookkeepingLogService(unitOfWork);
        }

        // GET: Bookkeeping
        public ActionResult Index()
        {
            return View(_BookkeepingSvc.Lookup());
        }

        // GET: Bookkeeping/Details/5
        public ActionResult Details(Guid? id)
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

        // GET: Bookkeeping/Create
        public ActionResult AddRecord(int? Page)
        {
            //這個頁面下方有list table , 是利用child action方式呈現, 所以利用CurrentPage 告訴ChildAction要呈現的頁面
            ViewData["CurrentPage"] = Page;  
            return View();
        }

        //[ChildActionOnly]  -->利用ajax回傳時, 如果設為 ChildActionOnly會出錯
        public ActionResult List(int? Page)
        {
            //使用 PagedList.Mvc 加上分頁
            int pageSize = 10;
            int currenPage = (Page == null || Page < 1) ? 1 : Page.Value;
            
            var result = _BookkeepingSvc.LookupByPage(currenPage, pageSize);
            
            return View(result);
        }      

        // POST: Bookkeeping/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRecord([Bind(Include = "Id,sn,Date,Type,Amount,Remark")] AddRecordViewModel bookkeepingData, int? Page)
        {
            //這個頁面下方有list table , 是利用child action方式呈現, 所以利用CurrentPage 告訴ChildAction要呈現的頁面
            //希望資料送出後,下方仍然停在原來頁面, 所以還是保留CurrentPage
            ViewData["CurrentPage"] = Page;
            if (ModelState.IsValid)
            {
                var recordId = Guid.NewGuid();
                var bookkeeping = new Bookkeeping
                {
                    Id = recordId,
                    Date = bookkeepingData.Date,
                    Amount = bookkeepingData.Amount,
                    Remark = bookkeepingData.Remark,
                    Type = bookkeepingData.Type
                };
                _BookkeepingSvc.Add(bookkeeping);
                _BookkeepingSvc.Save();

                _LogSvc.Add(recordId, "Create");
                _LogSvc.Save();               

                return RedirectToAction("AddRecord", new { Page = Page });
            }

            return View(bookkeepingData);
        }


        [HttpPost]
        public ActionResult AddRecordByAJAX(DateTime Date, BookType Type, int Amount, string Remark, int Page)
        {
            //利用AJAX Help新增資料  
            /*加驗證, 利用自訂驗證檢查,這樣寫很怪,MVC應該有更好的方式 >"<  */
            string errorMsg = String.Empty;
            string today = DateTime.Now.ToString("yyyy/MM/dd");
            CheckDateAttribute CheckDate = new CheckDateAttribute(today);
            if (!CheckDate.IsValid(Date))
            {
                errorMsg += "日期不可大於今天 /";
            }                

            PositiveIntegerAttribute CheckAmount = new ValidateAttribute.PositiveIntegerAttribute(0);
            if (!CheckAmount.IsValid(Amount))
            {
                errorMsg += "金額應輸入大於0的正整數 / ";
            }                

            if (Remark.Length > 300)
            {
                errorMsg += "Remark字數不可大於300字元 / ";
            }

            if (!errorMsg.Equals(""))
            {
                return Json(new
                {
                    success = false,
                    message = errorMsg
                });
            }
            

            var recordId = Guid.NewGuid();
            Bookkeeping data = new Bookkeeping
            {
                Id = recordId,
                Date = Date,
                Type = Type,
                Amount = Amount,
                Remark = Remark
            };

            _BookkeepingSvc.Add(data);
            _BookkeepingSvc.Save();

            _LogSvc.Add(recordId, "Create");
            _LogSvc.Save();


            return RedirectToAction("List", new { Page = Page });  //完成,回傳下方的list資料
        }

        // GET: Bookkeeping/Edit/5
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
                EditViewModel EditDetail = new EditViewModel
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

        // POST: Bookkeeping/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
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

                return RedirectToAction("AddRecord", new { Page = 1 });
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

            Bookkeeping bookkeeping = _BookkeepingSvc.GetSingle(id.Value);
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
            Bookkeeping bookkeeping = _BookkeepingSvc.GetSingle(id);
            _BookkeepingSvc.Delete(bookkeeping);
            _BookkeepingSvc.Save();
            _LogSvc.Add(id, "Delete");
            _LogSvc.Save();
            return RedirectToAction("AddRecord", new { Page = 1 });
        }

        
    }
}
