using Excella.Vending.DAL;
using Excella.Vending.Domain;
using Excella.Vending.Machine;
using Excella.Vending.Web.UI.Controllers;
using NUnit.Framework;
using System.Transactions;
using System.Web.Mvc;
using Excella.Vending.Web.UI.Models;
using Tests.Integration.Excella.Vending.Machine;

namespace Tests.Integration.Excella.Vending.Web.UI
{
    public class VendingMachineControllerTestsADO
    {
        private TransactionScope _transactionScope;
        private VendingMachineController _controller;
        private readonly IPaymentDAO _paymentDao = new ADOPaymentDAO();

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            _paymentDao.ClearPayments();
        }

        [SetUp]
        public void Setup()
        {
            _transactionScope = new TransactionScope();
            var paymentProcessor = new CoinPaymentProcessor(_paymentDao);
            var vendingMachine = new VendingMachine(paymentProcessor);
            _controller = new VendingMachineController(vendingMachine);
        }

        [TearDown]
        public void Teardown()
        {
            _transactionScope.Dispose();
        }

        [Test]
        public void Index_WhenFirstLoad_ExpectNoBalance()
        {
            _controller.Index();

            var vm = _controller.ViewData.Model as VendingMachineViewModel;
            Assert.That(vm.Balance, Is.EqualTo(0));
        }

        [Test]
        public void InsertCoin_WhenCalledOnce_Expect25Balance()
        {
            _controller.InsertCoin();
            _controller.Index();

            var result = _controller.ViewData.Model as VendingMachineViewModel;
            var balance = result.Balance;

            Assert.That(balance, Is.EqualTo(25));
        }

        [Test]
        public void ReleaseChange_WithMoneyEntered_ReturnsChange()
        {
            _controller.InsertCoin();

            var result = _controller.ReleaseChange() as RedirectToRouteResult;
            var actionName = result.RouteValues["action"];
            var releasedChange = result.RouteValues["ReleasedChange"];

            Assert.That(releasedChange, Is.EqualTo(25));
        }

        [Test]
        public void ReleaseChange_WithMoneyEntered_SetsBalanceToZero()
        {
            _controller.InsertCoin();

            _controller.ReleaseChange();

            _controller.Index();
            var vm = _controller.ViewData.Model as VendingMachineViewModel;
            var balance = vm.Balance;

            Assert.That(balance, Is.EqualTo(0));
        }
    }
}
