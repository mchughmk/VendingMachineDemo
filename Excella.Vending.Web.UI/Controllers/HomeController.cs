using Excella.Vending.Machine;
using System.Web.Mvc;

namespace Excella.Vending.Web.UI.Controllers
{
    public class HomeController : Controller
    {
        private IVendingMachine vendingMachine;

        public HomeController(IVendingMachine vendingMachine)
        {
            this.vendingMachine = vendingMachine;
        }

        public ActionResult Index()
        {
            ViewBag.Balance = vendingMachine.Balance;
            ViewBag.ReleasedChange = TempData["ReleasedChange"];
;           if (ViewBag.ReleasedChange == null)
            {
                ViewBag.ReleasedChange = 0;
            }
            return View();
        }

        public ActionResult InsertCoin()
        {
            vendingMachine.InsertCoin();
            return RedirectToAction("Index");
        }

        public ActionResult ReleaseChange()
        {
            TempData["ReleasedChange"] = vendingMachine.ReleaseChange();
            return RedirectToAction("Index");
        }
    }
}