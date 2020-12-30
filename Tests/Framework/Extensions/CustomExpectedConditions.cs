using Framework.SeleniumWrappers;
using OpenQA.Selenium;
using System;

namespace Framework.Extensions
{
    public static class CustomExpectedConditions
    {
        public static Func<IWebDriver, bool> ElementExists(By locator)
        {
            return driver =>
            {
                IWebElement element = driver.FindElement(locator);
                return element.Displayed;
            };
        }

        public static Func<IWebDriver, bool> ElementExists(WebElement element)
        {
            return driver => element.Displayed;
        }

        public static Func<IWebDriver, bool> ElementToBeClickable(WebElement element)
        {
            return driver => element.Displayed && element.Enabled;
        }

        public static Func<IWebDriver, bool> ElementToBeClickable(By locator)
        {
            return driver =>
            {
                IWebElement element = driver.FindElement(locator);
                return element.Displayed && element.Enabled;
            };
        }

        public static Func<IWebDriver, bool> InvisibilityOfElementWithText(By locator, string text)
        {
            return driver =>
            {
                try
                {
                    IWebElement element = driver.FindElement(locator);
                    if (element.Text.Equals(text))
                    {
                        return !element.Displayed ? true : false;
                    }
                }
                catch (NoSuchElementException)
                {

                }
                return true;
            };
        }

        public static Func<IWebDriver, bool> ElementIsVisible(By locator)
        {
            return driver =>
            {
                try
                {
                    IWebElement element = driver.FindElement(locator);
                    int elementHeight = element.Size.Height;
                    int elementWidth = element.Size.Width;
                    return element.Displayed && (elementHeight > 0 && elementWidth > 0);
                }
                catch (NoSuchElementException)
                {

                }
                return false;
            };
        }

        public static Func<IWebDriver, bool> ElementIsVisible(WebElement element)
        {
            return driver =>
            {
                int elementHeight = element.Size.Height;
                int elementWidth = element.Size.Width;
                return element.Displayed && (elementHeight > 0 && elementWidth > 0);
            };
        }

        public static Func<IWebDriver, bool> InvisibilityOfElement(WebElement element)
        {
            return driver =>
            {
                int elementHeight = element.Size.Height;
                int elementWidth = element.Size.Width;
                return (!element.Displayed) || elementHeight == 0 && elementWidth == 0;
            };
        }

        public static Func<IWebDriver, bool> InvisibilityOfElement(By locator)
        {
            return driver =>
            {
                try
                {
                    IWebElement element = driver.FindElement(locator);
                    int elementHeight = element.Size.Height;
                    int elementWidth = element.Size.Width;
                    return (!element.Displayed) || elementHeight == 0 && elementWidth == 0;
                }
                catch (NoSuchElementException)
                {

                }
                return true;
            };

        }
    }
}
