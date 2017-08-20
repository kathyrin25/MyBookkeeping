using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyBookkeeping.Models
{
    public class BookkeepingLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sn { get; set; }

        public Guid fk_Id { get; set; }

        [StringLength(10)]
        public string Action { get; set; }

        [StringLength(100)]
        public string UpdateBy { get; set; }
        
        public DateTime UpdateDT { get; set; }
        
    }
}