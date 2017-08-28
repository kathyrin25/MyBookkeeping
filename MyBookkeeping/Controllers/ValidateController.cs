using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBookkeeping.Controllers
{
    public class ValidateController : Controller
    {
        public ActionResult CheckDate(string Date)
        {
            bool isValidate = false;
            string ErrorMsg = String.Empty;

            //if (Url.IsLocalUrl(Request.Url.AbsoluteUri))  //這個判斷式無法運作, 一樣都是localhost:，只是port不一樣
            //{
                //利用 IsLocalUrl檢查是否為網站呼叫的 , 借此忽略一些不必要的流量   
                DateTime checkDate = new DateTime();
                if (!DateTime.TryParse(Date, out checkDate))    //非正常的日期格式,加這段是因為IE前面擋不住不正常的日期
                {
                    ErrorMsg = "非日期格式，請檢查。";
                }
                else if (DateTime.Compare(checkDate, DateTime.Today) > 0)   //「日期」不得大於今天
                {
                    ErrorMsg = "日期不可大於今天。";
                }
                else
                    isValidate = true;
            //}           
            

            //最好一個驗證做一件事, 提高重覆使用性
            // Remote 驗證是使用 Get 因此要開放
            if (isValidate)
                return Json(isValidate, JsonRequestBehavior.AllowGet);  // return Json("true", JsonRequestBehavior.AllowGet);  也可以,MVC會自動轉型
            else
                return Json(ErrorMsg, JsonRequestBehavior.AllowGet);  //回傳自訂的錯誤訊息
        }


     }
}