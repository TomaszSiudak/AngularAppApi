using Framework.Base;
using Framework.Base.WebDriverData;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Base
{
    [TestFixture]
    public class BaseWebTest
    {
        protected IWebDriver Driver;

        [SetUp]
        public void TestSetup()
        {
            AppConfig appConfig = AppConfigProvider.AppConfigInstance;
            Driver = WebDriverFactory.GetWebDriver(appConfig);
            Driver.Url = appConfig.EnvironmentURL;
        }

        [TearDown]
        public void TestTearDown()
        {
            if(TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                Driver.Quit();
                Driver = null;
            }
        }

        [OneTimeTearDown]
        public void ClassTearDown()
        {
            if (Driver != null)
            {
                Driver.Quit();
                Driver = null;
            }
        }

    }
}
