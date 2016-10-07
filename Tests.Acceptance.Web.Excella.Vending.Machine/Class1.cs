using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Tests.Acceptance.Web.Excella.Vending.Machine
{
    public class HomePage
    {
        private readonly ChromeDriver _browser;


        public HomePage()
        {
            _browser = new ChromeDriver();
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

        public string Balance()
        {
            IWebElement balanceDiv = _browser.FindElement(By.Id("totalAmount"));
            return balanceDiv.Text;
        }
    }
}
