using System.Collections.Generic;
using System.Web.Mvc;
using lejr_dk.Models;

namespace lejr_dk.Controllers
{
    public class LejrController : Controller
    {
        private readonly ICampService _cs;

        public LejrController(ICampService cs)
        {
            _cs = cs;
        }

        public ActionResult Junior()
        {
            //IList<Camp> camps = _cs.GetActiveCamps(CampType.Junior);
            //return View(camps);
            return View();
        }

        public ActionResult Putte()
        {
            return View();
        }

        public ActionResult Boerne()
        {
            return View();
        }

        public ActionResult Teen()
        {
            return View();
        }
    }
}