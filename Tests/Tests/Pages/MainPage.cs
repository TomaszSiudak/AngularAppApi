using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tests.Pages.PagesElements;
using Framework.Extensions;

namespace Tests.Pages
{
    public class MainPage : BasePage
    {
        public MainPageElements MainPageElements { get; }

        public MainPage(IWebDriver driver) : base(driver)
        {
            MainPageElements = new MainPageElements(driver);
        }

        protected override void WaitTillPageIsVisible()
        {
            Wait.Until(CustomExpectedConditions.ElementIsVisible(MainPageElements.MainPageHeader));
        }
    }
}
