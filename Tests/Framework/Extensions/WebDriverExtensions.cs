using Framework.SeleniumWrappers;
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
    }
}
