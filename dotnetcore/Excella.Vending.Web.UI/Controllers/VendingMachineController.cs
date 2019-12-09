using Excella.Vending.Machine;
using Excella.Vending.Web.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Excella.Vending.Web.UI.Controllers
{
    public class VendingMachineController : Controller
    {
        private readonly IVendingMachine _vendingMachine;

        public VendingMachineController(IVendingMachine vendingMachine)
        {
            _vendingMachine = vendingMachine;
        }

        public ActionResult Index()
        {
            var viewModel = new VendingMachineViewModel
            {
                Balance = _vendingMachine.Balance,
                ReleasedChange = 0
            };

            return View(viewModel);
        }

        public ActionResult IndexWithChange(int releasedChange)
        {
            var viewModel = new VendingMachineViewModel
            {
                Balance = _vendingMachine.Balance,
                ReleasedChange = releasedChange
            };

            return View("Index", viewModel);
        }

        public ActionResult InsertCoin()
        {
            _vendingMachine.InsertCoin();
            return RedirectToAction("Index");
        }

        public ActionResult ReleaseChange()
        {
            var releasedChange = _vendingMachine.ReleaseChange();

            return RedirectToAction("IndexWithChange", new { ReleasedChange = releasedChange });
        }
    }
}