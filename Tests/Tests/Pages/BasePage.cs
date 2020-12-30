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
        protected IWebDriver Driver { get; }
        protected WebDriverWait Wait { get; set; }
        protected NavigationMenuElements NavigationMenuElements { get; }

        protected BasePage(IWebDriver driver, int defaultWaitTime = 20)
        {
            Driver = driver;
            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(defaultWaitTime));
            NavigationMenuElements = new NavigationMenuElements(Driver);

        }

        protected abstract void WaitTillPageIsVisible();
    }
}
