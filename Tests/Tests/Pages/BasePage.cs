using Framework.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Pages.PagesElements;

namespace Tests.Pages
{
    public abstract class BasePage
    {
        #region xpaths

        public static string toastMessageXpath = "//*[contains(@class, 'ajs-message')]";

        #endregion xpaths
        protected IWebDriver Driver { get; }
        protected WebDriverWait Wait { get; set; }
        public NavigationMenuElements NavigationMenuElements { get; }


        protected BasePage(IWebDriver driver, int defaultWaitTime = 20)
        {
            Driver = driver;
            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(defaultWaitTime));
            NavigationMenuElements = new NavigationMenuElements(Driver);

        }

        public T As<T>() where T : BasePage
        {
            return (T)this;
        }

        public string GetToastMessage()
        {
            IWebElement toastMessageElement = Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(toastMessageXpath)));
            return toastMessageElement.Text;
        }

        protected abstract void WaitTillPageIsVisible();
    }
}
