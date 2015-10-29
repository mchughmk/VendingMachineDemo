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
            return View();
        }

        public ActionResult InsertCoin()
        {
            vendingMachine.InsertCoin();
            return RedirectToAction("Index");
        }
    }
}