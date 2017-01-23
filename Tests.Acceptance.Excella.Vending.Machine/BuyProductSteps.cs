using Excella.Vending.DAL;
using Excella.Vending.Domain;
using Excella.Vending.Machine;
using NUnit.Framework;
using System;
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

        [BeforeScenario]
        public void Setup()
        {
            _product = null;
            _changeReleased = 0;
            _transactionScope = new TransactionScope();
            var paymentDAO = new ADOPaymentDAO();
            var paymentProcessor = new CoinPaymentProcessor(paymentDAO);
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
            try
            {
                _product = _vendingMachine.BuyProduct();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("Product purchase failed: {0}", e.Message);
            }
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
            Assert.IsNotNull(_product);
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
            Assert.IsNull(_product);
        }
    }
}
