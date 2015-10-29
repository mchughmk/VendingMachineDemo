using Autofac;
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
        private TransactionScope transactionScope;
        private IContainer container;
        private IVendingMachine vendingMachine;
        private HomeController controller;

        [SetUp]
        public void Setup()
        {
            transactionScope = new TransactionScope();

            RegisterDependencies();
            vendingMachine = container.Resolve<IVendingMachine>();
            controller = new HomeController(vendingMachine);
        }

        [TearDown]
        public void Teardown()
        {
            transactionScope.Dispose();
        }

        [Test]
        public void Index_WhenFirstLoad_ExpectNoBalance()
        {
            // Arrange
            var action = controller.Action(c => c.Index());

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
            var action = controller.Action(c => c.InsertCoin());

            // Act
            var result = action.GetActionResult();
            var context = action.GetActionExecutingContext();

            // Assert
            Assert.IsInstanceOf<RedirectToRouteResult>(result);

            action = controller.Action(c => c.Index());
            result = action.GetActionResult();
            Assert.AreEqual(25, ((ViewResult)result).ViewBag.Balance);
        }

        private void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            //Register project abstractions
            builder.RegisterType<VendingMachine>().As<IVendingMachine>();
            builder.RegisterType<CoinPaymentProcessor>().As<IPaymentProcessor>();
            builder.RegisterType<ADOPaymentDAO>().As<IPaymentDAO>();

            container = builder.Build();
        }
    }
}
