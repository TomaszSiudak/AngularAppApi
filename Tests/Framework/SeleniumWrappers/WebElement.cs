using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.SeleniumWrappers
{
    public class WebElement
    {
        private IWebDriver driver;
        private IWebElement webElement;
        private By selector;

        public By BySelector => selector;
        public Size Size => webElement.Size;

        public WebElement(IWebDriver driver, IWebElement webElement, By bySelector)
        {
            this.driver = driver;
            this.webElement = webElement;
            selector = bySelector;
        }

        public void Click()
        {
            WaitElementIsClickable(selector);
            webElement.Click();
        }

        public WebElement FindWebElement(By selector)
        {
            IWebElement element = webElement.FindElement(selector);
            return new WebElement(driver, element, selector);
        }

        public string GetAttribute(string attribute) => webElement.GetAttribute(attribute);
        public string GetText()
        {
            if (webElement.TagName == "input" || webElement.TagName == "textarea")
                return GetAttribute("value");
            else
                return webElement.Text;
        }

        public bool HasAttribute(string attribute)
        {
            bool result = false;
            try
            {
                string value = GetAttribute(attribute);
                if (value != null)
                {
                    result = true;
                }
            }
            catch (Exception) { }
            return result;
        }

        public bool IsEnabled() => webElement.Enabled;
        public bool IsVisible() => webElement.Displayed;

        public void TypeText(string text)
        {
            webElement.Clear();
            for (int i = 0; i < text.Length; i++)
            {
                webElement.SendKeys($"{text[i]}");
                Thread.Sleep(50);
            }
        }

        private void WaitElementIsClickable(By selector, int defaultTime = 5)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(defaultTime));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(selector));
        }
    }
}
