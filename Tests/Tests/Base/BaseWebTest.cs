using Framework.Base;
using Framework.Base.WebDriverData;
using NUnit.Framework;
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

    }
}
