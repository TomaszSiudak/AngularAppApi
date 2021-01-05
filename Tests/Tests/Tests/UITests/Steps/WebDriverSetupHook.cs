﻿using BoDi;
using Framework.Base;
using Framework.Base.WebDriverData;
using Framework.Extensions;
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
        private readonly ScenarioContext scenarioContext;

        public WebDriverSetupHook(IObjectContainer objectContainer, ScenarioContext scenarioContext)
        {
            this.objectContainer = objectContainer;
            this.scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            AppConfig appConfig = AppConfigProvider.AppConfigInstance;
            Driver = WebDriverFactory.GetWebDriver(appConfig);
            objectContainer.RegisterInstanceAs<IWebDriver>(Driver);
            Driver.Url = appConfig.EnvironmentURL;
        }

        [AfterScenario]
        public void AfterScenario()
        {
            if(scenarioContext.TestError != null)
            {
                Driver.TakeScreenshot(scenarioContext.ScenarioInfo.Title);
            }
            Driver.Quit();
            Driver = null;
        }
    }
}
