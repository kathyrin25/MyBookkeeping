using MyBookkeeping.Models;
using MyBookkeeping.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace MyBookkeeping.Service
{
    public class BookkeepingService
    {
        private readonly IRepository<Bookkeeping> _BookkeepingRep;
        private readonly IUnitOfWork _unitOfWork;

        public IEnumerable<Bookkeeping> Lookup()
        {
            /* Day2作業,我原來的寫法是 
               return (_db.BookkeepingListViewModels.ToList()); --> 資料已抓回記憶體
               避免誤用,修改如下
             */
            return _BookkeepingRep.LookupAll();
        }

        public IEnumerable<Bookkeeping> LookupByPage(int Page, int pageSize)
        {
            //分頁顯示
            //使用 PagedList.Mvc 加上分頁

            /*ToPagedList前必須先OrderBy*/
            var result = _BookkeepingRep.LookupAll().OrderByDescending(x => x.Date).ToPagedList(Page, pageSize);
           
            return result;
        }

        public BookkeepingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _BookkeepingRep = new Repository<Bookkeeping>(unitOfWork);
        }        

        public Bookkeeping GetSingle(Guid RecordId)
        {
            return _BookkeepingRep.GetSingle(d => d.Id == RecordId);
        }

        public void Add(Bookkeeping bookkeepingList)
        {
            _BookkeepingRep.Create(bookkeepingList);
        }

        public void Edit(Bookkeeping pageData, Bookkeeping oldData)
        {
            oldData.Date = pageData.Date;
            oldData.Type = pageData.Type;
            oldData.Amount = pageData.Amount;
            oldData.Remark = pageData.Remark;
        }

        public void Delete(Bookkeeping data)
        {
            _BookkeepingRep.Remove(data);
        }

        public void Save()
        {
            _unitOfWork.Save();
        }
    }
}