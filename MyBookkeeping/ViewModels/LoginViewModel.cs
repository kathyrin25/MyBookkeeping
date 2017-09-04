using MyBookkeeping.ValidateAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;
using MyBookkeeping.ValidateAttribute;
using System.Web.Security;

namespace MyBookkeeping.ViewModels
{
    public class LoginViewModel : IValidatableObject
    {
        /// <summary>
        /// 帳號
        /// </summary>
        [Display(Name = "ID")]
        [Required]
        [EmailAddress]
        [BanAccountAttribue("skilltree,demo,twMVC", ErrorMessage = "帳號不可有關鍵字")]
        public string Account { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        [Display(Name = "Password")]
        [Required]
        [MinLength(4)]
        [MaxLength(20)]
        public string Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //這個先暫時寫hard code代替
            if (!(Account == "aa@aa.com" && Password == "12345") && !(Account == "bb@bb.com" && Password == "12345"))
            {
                yield return new ValidationResult("無此帳號或密碼", new string[] { "Account" });
            }            
        }
    }
}