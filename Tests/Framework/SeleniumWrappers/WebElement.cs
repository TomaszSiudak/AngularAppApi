using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.SeleniumWrappers
{
    public class WebElement
    {
        private IWebDriver driver;
        private IWebElement webElement;
        private By selector;

        public WebElement(IWebDriver driver, IWebElement webElement, By bySelector)
        {
            this.driver = driver;
            this.webElement = webElement;
            selector = bySelector;
        }

        public void Click()
        {
            webElement.Click();
        }

        public void TypeText(string text)
        {
            webElement.Clear();
            webElement.SendKeys(text);
        }
    }
}
