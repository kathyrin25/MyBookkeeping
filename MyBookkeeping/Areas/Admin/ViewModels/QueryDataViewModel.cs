using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBookkeeping.Areas.Admin.ViewModels
{
    public class QueryDataViewModel
    {        
        [Required]
        [UIHint("DateTimeInput")]
        [Display(Name = "From")]
        public DateTime StartDate { get; set; }

        [Required]
        [UIHint("DateTimeInput")]
        [Display(Name = "To")]
        public DateTime EndDate { get; set; }
    }
}