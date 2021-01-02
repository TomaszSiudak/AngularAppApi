using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tests.Pages.PagesElements;
using Framework.Extensions;
using Framework.Base.WebDriverData;

namespace Tests.Pages
{
    public class MainPage : BasePage
    {
        private static string URL = AppConfigProvider.AppConfigInstance.EnvironmentURL;
        public MainPageElements MainPageElements { get; }

        public MainPage(IWebDriver driver) : base(driver)
        {
            MainPageElements = new MainPageElements(driver);
        }


        public MainPage GoToMainPage()
        {
            Driver.Url = URL;
            WaitTillPageIsVisible();
            return this;
        }

        protected override void WaitTillPageIsVisible()
        {
            Wait.Until(CustomExpectedConditions.ElementIsVisible(MainPageElements.MainPageHeader));
        }
    }
}
