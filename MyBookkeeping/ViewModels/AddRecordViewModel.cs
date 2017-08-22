using MyBookkeeping.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBookkeeping.ViewModels
{
    public class AddRecordViewModel
    {
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "日期")]
        [Required]
        [RegularExpression(@"^([0-9]{4})[./]{1}([0-9]{2})[./]{1}([0-9]{2})$", ErrorMessage = "日期格式請輸入yyyy/MM/dd")]  
        [Remote("CheckDate", "Validate")]
        public DateTime Date { get; set; }

      
        [Display(Name = "類別")]
        [Required]
        public BookType Type { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "金額")]
        [Required]
        //[RegularExpression(@"^\+?[1-9][0-9]*$", ErrorMessage = "只能輸入大於0的正整數")] 
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "只能輸入正整數")]  /*只能輸入正整數*/
        public int Amount { get; set; }      

        [Required]
        [Display(Name = "備註")]       
        [StringLength(100, ErrorMessage = "輸入字串長度不可超過{1}個字元")]
        public string Remark { get; set; }
    }
}