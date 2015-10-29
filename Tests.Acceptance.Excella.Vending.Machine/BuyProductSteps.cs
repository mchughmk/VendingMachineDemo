using Excella.Vending.DAL;
using Excella.Vending.Domain;
using Excella.Vending.Machine;
using NUnit.Framework;
using System.Transactions;
using TechTalk.SpecFlow;

namespace Tests.Acceptance.Excella.Vending.Machine
{
    [Binding]
    public class BuyProductSteps
    {
        private IVendingMachine vendingMachine;
        private TransactionScope transactionScope;
        private Product product;

        [BeforeScenario]
        public void Setup()
        {
            transactionScope = new TransactionScope();

            product = null;
            var paymentDAO = new ADOPaymentDAO();
            var paymentProcessor = new CoinPaymentProcessor(paymentDAO);
            vendingMachine = new VendingMachine(paymentProcessor);
        }

        [AfterScenario]
        public void Teardown()
        {
            transactionScope.Dispose();
        }

        [Given(@"I have inserted a quarter")]
        public void GivenIHaveInsertedAQuarter()
        {
            vendingMachine.InsertCoin();
        }

        [When(@"I purchase a product")]
        public void WhenIPurchaseAProduct()
        {
            product = vendingMachine.BuyProduct();
        }

        [Then(@"I should receive the product")]
        public void ThenIShouldReceiveTheProduct()
        {
            Assert.IsNotNull(product);
        }
    }
}
