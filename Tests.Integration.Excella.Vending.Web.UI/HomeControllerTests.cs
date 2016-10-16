using Excella.Vending.DAL;
using Excella.Vending.Domain;
using Excella.Vending.Machine;
using Excella.Vending.Web.UI.Controllers;
using NUnit.Framework;
using System.Transactions;
using System.Web.Mvc;
using Xania.AspNet.Simulator;

namespace Tests.Integration.Excella.Vending.Web.UI
{
    public class HomeControllerTests
    {
        private TransactionScope _transactionScope;
        private HomeController _controller;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
        }

        [SetUp]
        public void Setup()
        {
            _transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew);

            var paymentDAO = new ADOPaymentDAO();
            var paymentProcessor = new CoinPaymentProcessor(paymentDAO);
            var vendingMachine = new VendingMachine(paymentProcessor);
            _controller = new HomeController(vendingMachine);
        }

        [TearDown]
        public void Teardown()
        {
            _transactionScope.Dispose();
        }

        [Test]
        public void Index_WhenFirstLoad_ExpectNoBalance()
        {
            // Arrange
            var action = _controller.Action(c => c.Index());

            // Act
            var result = action.GetActionResult();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual(0, ((ViewResult)result).ViewBag.Balance);
        }

        [Test]
        public void InsertCoin_WhenCalledOnce_Expect25Balance()
        {
            // Arrange
            var action = _controller.Action(c => c.InsertCoin());

            // Act
            var result = action.GetActionResult();

            // Assert
            Assert.IsInstanceOf<RedirectToRouteResult>(result);

            action = _controller.Action(c => c.Index());
            result = action.GetActionResult();
            Assert.AreEqual(25, ((ViewResult)result).ViewBag.Balance);
        }
    }
}
