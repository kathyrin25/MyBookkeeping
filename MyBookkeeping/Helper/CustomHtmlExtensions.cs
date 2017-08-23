using MyBookkeeping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBookkeeping.Helper
{
    public static class CustomHtmlExtensions  /*一定要宣告為 static */
    {
        public static string ShowClass(this HtmlHelper helper, BookType _booktype)
        {
            return (_booktype == BookType.支出) ? "text-danger" : "text-primary";  //回傳Class name
        }
    }
}