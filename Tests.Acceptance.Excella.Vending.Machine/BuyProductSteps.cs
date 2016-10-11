using Excella.Vending.DAL;
using Excella.Vending.Domain;
using Excella.Vending.Machine;
using NUnit.Framework;
using System;
using System.Data.SqlClient;
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
            transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew);

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
            try
            {
                product = vendingMachine.BuyProduct();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("Product purchase failed: {0}", e.Message);
            }
        }

        [Then(@"I should receive the product")]
        public void ThenIShouldReceiveTheProduct()
        {
            Assert.IsNotNull(product);
        }

        [Given(@"I have not inserted a quarter")]
        public void GivenIHaveNotInsertedAQuarter()
        {
            // Not calling insert coin
        }

        [Then(@"I should not receive a product")]
        public void ThenIShouldNotReceiveAProduct()
        {
            Assert.IsNull(product);
        }


        private SqlConnection GetConnection()
        {
            var connectionString = "Server=.;Database=VendingMachine;Trusted_Connection=True;";

            return new SqlConnection(connectionString);
        }
    }
}
