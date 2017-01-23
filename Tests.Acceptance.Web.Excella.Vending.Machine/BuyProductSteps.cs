using System;
using NUnit.Framework;
using TechTalk.SpecFlow;
// ReSharper disable UnusedMember.Global -- test methods are said to be unused which isn't correct. -SK

namespace Tests.Acceptance.Web.Excella.Vending.Machine
{
    [Binding]
    public class BuyProductSteps
    {
        private HomePage _homePage;

        [BeforeFeature]
        public static void BeforeFeature()
        {
            IISExpressTestManager.StartIISExpress();
        }

        [AfterFeature]
        public static void AfterFeature()
        {
            IISExpressTestManager.StopIISExpress();
        }

        [BeforeScenario]
        public void Setup()
        {

            if (!IISExpressTestManager.IsIISExpressRunning())
            {
                throw new Exception("IIS Express must be running for this test to work");
            }
            _homePage = new HomePage();
            _homePage.Go();
        }

        [AfterScenario]
        public void Teardown()
        {
            _homePage?.ReleaseChangeButton().Click();
            _homePage?.Close();
        }

        [When(@"I insert a Quarter")]
        public void WhenIInsertAQuarter()
        {
            InsertQuarter();
        }

        [Then(@"The balance should be (.*) cents")]
        public void TheBalanceShouldBe(int cents)
        {
            var balance = _homePage.Balance();

            Assert.That(balance, Is.EqualTo(cents));
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

        [When("I do not purchase a product")]
        public void WhenIDoNotPurchaseAProduct()
        {
            //No-op -- doing nothing here
        }

        [When(@"I purchase a product")]
        public void WhenIPurchaseAProduct()
        {
            // TODO 
        }

        [When(@"I release the change")]
        public void WhenIReleaseTheChange()
        {
            _homePage.ReleaseChangeButton().Click();
        }

        [Then(@"I should receive (.*) cents in change")]
        public void ThenIShouldReceiveXCentsInChange(int cents)
        {
            var releasedChange = _homePage.ReleasedChange();

            Assert.That(releasedChange, Is.EqualTo(cents));
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
