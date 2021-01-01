using BoDi;
using Framework.Base;
using Framework.Base.WebDriverData;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using Tests.Pages;

namespace Tests.Tests.UITests.Steps
{
    [Binding]
    public sealed class WebDriverSetupHook
    {
        private IWebDriver Driver;

        private readonly IObjectContainer objectContainer;

        public WebDriverSetupHook(IObjectContainer objectContainer)
        {
            this.objectContainer = objectContainer;
        }

        private void SetupWebDriver()
        {
            AppConfig appConfig = AppConfigProvider.AppConfigInstance;
            IWebDriver Driver = WebDriverFactory.GetWebDriver(appConfig);
            objectContainer.RegisterInstanceAs<IWebDriver>(Driver); 
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            AppConfig appConfig = AppConfigProvider.AppConfigInstance;
            IWebDriver Driver = WebDriverFactory.GetWebDriver(appConfig);
            objectContainer.RegisterInstanceAs<IWebDriver>(Driver);
            Driver.Url = appConfig.EnvironmentURL;
        }

        [AfterScenario]
        public void AfterScenario()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                Driver.Quit();
                Driver = null;
            }
        }

        [AfterFeature]
        public void AfterFeature()
        {
            if (Driver != null)
            {
                Driver.Quit();
                Driver = null;
            }
        }
    }
}
