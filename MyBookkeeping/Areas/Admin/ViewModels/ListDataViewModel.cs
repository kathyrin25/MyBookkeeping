using MyBookkeeping.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBookkeeping.Areas.Admin.ViewModels
{
    public class ListDataViewModel
    {
        [Key]
        public Guid Id { get; set; }   
        
        public DateTime Date { get; set; }
       
        public BookType Type { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]        
        public int Amount { get; set; }

        [Display(Name = "說明")]
        public string Remark { get; set; }
    }
}