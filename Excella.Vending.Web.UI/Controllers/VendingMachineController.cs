using Excella.Vending.Machine;
using System.Web.Mvc;
using Excella.Vending.Web.UI.Models;

namespace Excella.Vending.Web.UI.Controllers
{
    public class VendingMachineController : Controller
    {
        private IVendingMachine vendingMachine;

        public VendingMachineController(IVendingMachine vendingMachine)
        {
            this.vendingMachine = vendingMachine;
        }

        public ActionResult Index()
        {
            var viewModel = new VendingMachineViewModel
            {
                Balance = vendingMachine.Balance,
                ReleasedChange = 0
            };

            return View(viewModel);
        }

        public ActionResult IndexWithChange(int releasedChange)
        {
            var viewModel = new VendingMachineViewModel
            {
                Balance = vendingMachine.Balance,
                ReleasedChange = releasedChange
            };

            return View("Index", viewModel);
        }

        public ActionResult InsertCoin()
        {
            vendingMachine.InsertCoin();
            return RedirectToAction("Index");
        }

        public ActionResult ReleaseChange()
        {
            var releasedChange = vendingMachine.ReleaseChange();

            return RedirectToAction("IndexWithChange", new { ReleasedChange = releasedChange });
        }
    }
}