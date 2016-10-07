using System.Data.SqlClient;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Tests.Acceptance.Web.Excella.Vending.Machine
{
    [Binding]
    public class BuyProductSteps
    {

        private HomePage _homePage;

        [BeforeScenario]
        public void Setup()
        {
            _homePage = new HomePage();
            _homePage.Go();
        }

        [AfterScenario]
        public void Teardown()
        {
        }

        [When(@"I insert a quarter")]
        public void WhenIInsertAQuarter()
        {
            InsertQuarter();
        }

        [Then(@"The balance should be 25 cents")]
        public void TheBalanceShouldBe25Cents()
        {
            var balance = _homePage.Balance();

            Assert.That(balance, Does.Contain("25"));
            Assert.That(balance, Does.Contain("cents"));
        }
        private void InsertQuarter()
        {
            _homePage.InsertCoinButton().Click();
        }

        [Given(@"I have inserted a quarter")]
        public void GivenIHaveInsertedAQuarter()
        {
            InsertQuarter();
        }

        [When(@"I purchase a product")]
        public void WhenIPurchaseAProduct()
        {
            //try
            //{
            //    product = vendingMachine.BuyProduct();
            //}
            //catch (InvalidOperationException e)
            //{
            //    Console.WriteLine("Product purchase failed: {0}", e.Message);
            //}
        }

        [Then(@"I should receive the product")]
        public void ThenIShouldReceiveTheProduct()
        {
            //Assert.IsNotNull(product);
        }

        [Given(@"I have not inserted a quarter")]
        public void GivenIHaveNotInsertedAQuarter()
        {
            // Not calling insert coin
        }

        [Then(@"I should not receive a product")]
        public void ThenIShouldNotReceiveAProduct()
        {
            //Assert.IsNull(product);
        }


        private SqlConnection GetConnection()
        {
            var connectionString = "Server=.;Database=VendingMachine;Trusted_Connection=True;";

            return new SqlConnection(connectionString);
        }

        private void ResetDBBalance()
        {
            var connection = GetConnection();

            using (connection)
            {
                SqlCommand command = new SqlCommand("UPDATE Payment SET Value = 0 WHERE ID = 1;", connection);
                connection.Open();

                command.ExecuteNonQuery();
            }
        }
    }
}
