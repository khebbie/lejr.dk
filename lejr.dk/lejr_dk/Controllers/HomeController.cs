using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace lejr_dk.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private bool IsMobileDevice(string strUserAgent)
        {
            strUserAgent = strUserAgent.ToLower();
            if (//Request.Browser.IsMobileDevice == true ||
                    strUserAgent.Contains("iphone")
                   || strUserAgent.Contains("ipod")
                   || strUserAgent.Contains("android")
                // || strUserAgent.Contains("mobile")
                   || strUserAgent.Contains("blackberry")
                   || strUserAgent.Contains("windows ce")
                   || strUserAgent.Contains("opera mini")
                   || strUserAgent.Contains("palm")
                   || strUserAgent.Contains("ipad")
                    )
            {
                return true;
            }
            return false;
        }

        private bool IgnoreMobile(string ignoreMobileFromUrl)
        {
            if(string.IsNullOrEmpty(ignoreMobileFromUrl))
                return false;
            if (ignoreMobileFromUrl.ToLower() == "true")
                return true;
            return false;
        }

        public ActionResult Index()
        {
            string ignoreMobile = Request["ignoreMobile"];
            string mobileUrl = "http://m.lejr.dk";
            if (!IgnoreMobile(ignoreMobile) && IsMobileDevice(Request.UserAgent))
                return Redirect(mobileUrl);
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Bestyrelsen()
        {
            return View();
        }
    }
}
