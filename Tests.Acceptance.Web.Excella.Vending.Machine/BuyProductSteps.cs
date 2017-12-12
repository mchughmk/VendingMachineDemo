using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;

namespace Tests.Acceptance.Web.Excella.Vending.Machine
{
    [Binding]
    public class BuyProductSteps
    {
        private IWebDriver _browser;
        private const string HOME_PAGE_URL = "http://localhost:8484/";

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

            _browser = new ChromeDriver();

            GoToHomePage();
        }

        [AfterScenario]
        public void Teardown()
        {
            ClickReleaseChangeButton();

            _browser.Close();
            _browser.Dispose();
        }

        [When(@"I insert a Quarter")]
        public void WhenIInsertAQuarter()
        {
            ClickInsertCoinButton();
        }

        [Then(@"The balance should be (.*) cents")]
        public void TheBalanceShouldBe(int cents)
        {
            var balance = GetBalance();

            Assert.That(balance, Is.EqualTo(cents));
        }

        [Given(@"I have inserted a quarter")]
        public void GivenIHaveInsertedAQuarter()
        {
            ClickInsertCoinButton();
        }

        [When("I do not purchase a product")]
        public void WhenIDoNotPurchaseAProduct()
        {
            //No-op -- doing nothing here
        }

        [When(@"I purchase a product")]
        public void WhenIPurchaseAProduct()
        {
            // Need to finish this 
        }

        [When(@"I release the change")]
        public void WhenIReleaseTheChange()
        {
            ClickReleaseChangeButton();
        }

        [Then(@"I should receive (.*) cents in change")]
        public void ThenIShouldReceiveXCentsInChange(int cents)
        {
            var releasedChange = GetReleasedChange();

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

        private void GoToHomePage()
        {
            _browser.Navigate().GoToUrl(HOME_PAGE_URL);
        }

        private void ClickReleaseChangeButton()
        {
            var button = _browser.FindElement(By.Id("releaseChange"));

            button.Click();
        }

        private int GetReleasedChange()
        {
            var wait = new WebDriverWait(_browser, TimeSpan.FromSeconds(10));

            var element = wait.Until(drv => drv.FindElement(By.Id("releasedChangeAmount"))).Text;
            int changeAmt;
            if (int.TryParse(element, out changeAmt))
            {
                return changeAmt;
            }
            return 0;
        }

        private void ClickInsertCoinButton()
        {
            var button = _browser.FindElement(By.Id("insertCoin"));
            button.Click();
        }

        private int GetBalance()
        {
            try
            {
                return int.Parse(_browser.FindElement(By.Id("balanceAmount")).Text);
            }
            catch (StaleElementReferenceException)
            {
                return GetBalance();
            }
        }
    }
}
