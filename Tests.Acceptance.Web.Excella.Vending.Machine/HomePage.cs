using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Constraints;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.IE;

namespace Tests.Acceptance.Web.Excella.Vending.Machine
{
    public class HomePage
    {
        private readonly IWebDriver _browser;


        public HomePage()
        {
            _browser = new ChromeDriver();
        }

        public void Close()
        {
            _browser.Close();
            _browser.Dispose();
        }
        const string HOME_PAGE_URL = "http://localhost:8484/";
        public void Go()
        {
            _browser.Navigate().GoToUrl(HOME_PAGE_URL);
        }

        public IWebElement InsertCoinButton()
        {
            IWebElement button = _browser.FindElement(By.Id("insertCoin"));
            return button;
        }

        public int Balance()
        {
            try
            {
                return int.Parse(_browser.FindElement(By.Id("balanceAmount")).Text);
            }
            catch (StaleElementReferenceException)
            {
                return Balance();
            }
        }

        public IWebElement ReleaseChangeButton()
        {
            IWebElement button = _browser.FindElement(By.Id("releaseChange"));
            return button;
        }

        public int ReleasedChange()
        {
            try
            {
                var element = _browser.FindElement(By.Id("releasedChangeAmount")).Text;

                int changeAmt;
                if (int.TryParse(element, out changeAmt))
                {
                    return changeAmt;
                }

                return 0;
            }
            catch (StaleElementReferenceException)
            {
                return ReleasedChange();
            }
        }
    }
}
