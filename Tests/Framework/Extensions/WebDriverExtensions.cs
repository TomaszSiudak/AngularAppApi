﻿using Framework.SeleniumWrappers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Extensions
{
    public static class WebDriverExtensions
    {
        public static WebElement FindWebElement(this IWebDriver webDriver, By selector, int waitTime = 5)
        {
            WebDriverWait Wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(waitTime));
            IWebElement element = Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(selector));
            return new WebElement(webDriver, element, selector);
        }

        public static List<WebElement> FindWebElements(this IWebDriver webDriver, By selector, int waitTime = 5)
        {
            List<WebElement> webElements = new List<WebElement>();
            WebDriverWait Wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(waitTime));
            IReadOnlyCollection<IWebElement> elements = Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.PresenceOfAllElementsLocatedBy(selector));

            foreach (var element in elements)
            {
                webElements.Add(new WebElement(webDriver, element, selector));
            }
            return webElements;
        }

        public static void TakeScreenshot(IWebDriver driver, string testName)
        {
            string dirPath = "";

            ((ITakesScreenshot)driver)
                .GetScreenshot()
                .SaveAsFile($"{dirPath}\\{testName}_{DateTime.Now:HH-mm}.jpg", ScreenshotImageFormat.Jpeg);
        }

        public static void WaitForAjax(this IWebDriver driver, int timeout = 20)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            wait.Until(d => (bool)((IJavaScriptExecutor)d).ExecuteScript("return jQuery.active == 0"));
        }
    }
}
