using MyBookkeeping.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBookkeeping.ViewModels
{
    public class AddRecordViewModel
    {
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "日期")]
        public DateTime Date { get; set; }

        [Display(Name = "類別")]
        public BookType Type { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "金額")]
        public int Amount { get; set; }

        [Required(AllowEmptyStrings = true)]
        [Display(Name = "備註")]
        [StringLength(500)]
        public string Remark { get; set; }
    }
}