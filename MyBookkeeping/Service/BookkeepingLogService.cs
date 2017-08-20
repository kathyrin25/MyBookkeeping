using MyBookkeeping.Models;
using MyBookkeeping.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBookkeeping.Service
{
    public class BookkeepingLogService
    {
        private readonly IRepository<BookkeepingLog> _logRep;
        private readonly IUnitOfWork _unitOfWork;

        public BookkeepingLogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _logRep = new Repository<BookkeepingLog>(unitOfWork);
        }

        public void Add(Guid RecordId, string Action)
        {
            _logRep.Create(new BookkeepingLog
            {                
                Action = Action,
                fk_Id = RecordId,
                UpdateDT = DateTime.Now
            });
        }

        public void Save()
        {
            _unitOfWork.Save();
        }
    }
}