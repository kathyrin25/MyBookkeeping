using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyBookkeeping.Models
{
    public class Bookkeeping
    {       
        [Key]
        public Guid Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sn { get; set; }  


        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "日期")]
        public DateTime Date { get; set; }

        [Display(Name = "類別")]
        public BookType Type { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "金額")]
        public int Amount { get; set; }
        
        [Display(Name = "備註")]
        [StringLength(500)]
        public string Remark { get; set; }        

    }

    public enum BookType
    {
        收入,
        支出
    }
}