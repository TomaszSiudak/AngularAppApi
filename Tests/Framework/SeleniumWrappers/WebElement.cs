﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
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

        public By BySelector => selector;
        public bool Displayed => webElement.Displayed;
        public bool Enabled => webElement.Enabled;

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

        public string GetAttribute(string attribute) => webElement.GetAttribute(attribute);
        public string GetText() => webElement.Text;

        public void TypeText(string text)
        {
            webElement.Clear();
            webElement.SendKeys(text);
        }

        private void WaitElementIsClickable(By selector, int defaultTime = 5)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(defaultTime));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(selector));
        }
    }
}