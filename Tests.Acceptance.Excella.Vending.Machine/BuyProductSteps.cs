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
        private IVendingMachine _vendingMachine;
        private TransactionScope _transactionScope;
        private Product _product;
        private int _changeReleased;

        [BeforeFeature]
        public static void BeforeFeature()
        {
            var efDao = new EFPaymentDAO();
            var paymentProcessor = new CoinPaymentProcessor(efDao);
            paymentProcessor.ClearPayments();
        }

        [BeforeScenario]
        public void Setup()
        {
            _product = null;
            _changeReleased = 0;
            _transactionScope = new TransactionScope();
            var efDao = new EFPaymentDAO();
            var paymentProcessor = new CoinPaymentProcessor(efDao);
            _vendingMachine = new VendingMachine(paymentProcessor);
        }

        [AfterScenario]
        public void Teardown()
        {
            _transactionScope.Dispose();
        }

        [Given(@"I have inserted a quarter")]
        public void GivenIHaveInsertedAQuarter()
        {
            _vendingMachine.InsertCoin();
        }

        [When(@"I purchase a product")]
        public void WhenIPurchaseAProduct()
        {
            _product = _vendingMachine.BuyProduct();
        }

        [Then(@"I should receive no change")]
        public void ThenIShouldReceiveNoChange()
        {
            Assert.That(_changeReleased, Is.EqualTo(0));
        }

        [Then(@"I should receive a quarter")]
        public void ThenIShouldReceiveAQuarter()
        {
            Assert.That(_changeReleased, Is.EqualTo(25));
        }

        [Then(@"I should receive 75 cents")]
        public void ThenIShouldReceive75Cents()
        {
            Assert.That(_changeReleased, Is.EqualTo(75));
        }

        [Then(@"I should receive the product")]
        public void ThenIShouldReceiveTheProduct()
        {
            Assert.That(_product, Is.Not.Null);
        }

        [Given(@"I have not inserted a quarter")]
        public void GivenIHaveNotInsertedAQuarter()
        {
            // Not calling insert coin
        }

        [When(@"I release the change")]
        public void WhenIReleaseTheChange()
        {
            _changeReleased = _vendingMachine.ReleaseChange();
        }

        [Then(@"I should not receive a product")]
        public void ThenIShouldNotReceiveAProduct()
        {
            Assert.That(_product, Is.Null);
        }
    }
}