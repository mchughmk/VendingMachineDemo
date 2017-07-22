using Excella.Vending.DAL;
using Excella.Vending.Domain;
using Excella.Vending.Machine;
using Excella.Vending.Web.UI.Controllers;
using NUnit.Framework;
using System.Transactions;
using System.Web.Mvc;
using Excella.Vending.Web.UI.Models;
using Xania.AspNet.Simulator;

namespace Tests.Integration.Excella.Vending.Web.UI
{
    public class HomeControllerTests
    {
        private TransactionScope _transactionScope;
        private VendingMachineController _controller;


        [OneTimeSetUp]
        public void FixtureSetup()
        {
        }

        [SetUp]
        public void Setup()
        {
            _transactionScope = new TransactionScope();
            var efDao = new EFPaymentDAO();
            var paymentProcessor = new CoinPaymentProcessor(efDao);
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
            // Act
            _controller.Index();

            // Assert
            var vm = _controller.ViewData.Model as VendingMachineViewModel;
            Assert.That(vm.Balance, Is.EqualTo(0));
        }

        [Test]
        public void InsertCoin_WhenCalledOnce_Expect25Balance()
        {
            // Act
            _controller.InsertCoin();

            // Assert
            var result = _controller.ViewData.Model as VendingMachineViewModel;
            Assert.AreEqual(25, result.Balance);
        }

        [Test]
        public void ReleaseChange_WithMoneyEntered_ReturnsChange()
        {
            // Arrange
            _controller.InsertCoin();

            // Act
            _controller.ReleaseChange();

            // Assert
            var vm = _controller.ViewData.Model as VendingMachineViewModel;
            Assert.AreEqual(25, vm.ReleasedChange);
        }

        [Test]
        public void ReleaseChange_WithMoneyEntered_SetsBalanceToZero()
        {
            // Arrange
            _controller.InsertCoin();

            // Act
            _controller.ReleaseChange();

            var vm = _controller.ViewData.Model as VendingMachineViewModel;
            Assert.AreEqual(0, vm.Balance);
        }
    }
}
