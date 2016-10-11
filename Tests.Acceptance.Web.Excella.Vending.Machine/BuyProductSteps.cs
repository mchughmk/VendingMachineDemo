using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Tests.Acceptance.Web.Excella.Vending.Machine
{
    [Binding]
    public class BuyProductSteps
    {

        private HomePage _homePage;
        private int _previousBalance;

        [BeforeScenario]
        public void Setup()
        { 
            _homePage = new HomePage();
            _homePage.Go();
        }

        [AfterScenario]
        public void Teardown()
        {
            _homePage.Close();
        }

        [When(@"I insert a Quarter")]
        public void WhenIInsertAQuarter()
        {
            InsertQuarter();
        }

        [Then(@"The balance should increase by 25 cents")]
        public void TheBalanceShouldIncreaseBy25Cents()
        {
            var balance = _homePage.Balance();

            Assert.That(balance, Is.GreaterThan(_previousBalance));
            Assert.That(balance, Is.EqualTo(_previousBalance + 25));
        }
        private void InsertQuarter()
        {
            _previousBalance = _homePage.Balance();
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
    }
}
