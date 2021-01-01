using Framework.Constants.Enums;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace Framework.Base
{
    public class WebDriverFactory
    {
        public static IWebDriver GetWebDriver(AppConfig config)
        {
            return config.Environment.Equals(EnvironmentType.Local) ? GetLocalWebDriver(config.BrowserType) : GetRemoteWebDriver(config.BrowserType);
        }

        private static IWebDriver GetLocalWebDriver(BrowserType browserType)
        {
            switch (browserType)
            {
                case BrowserType.Firefox:
                    new DriverManager().SetUpDriver(new FirefoxConfig());
                    return new FirefoxDriver(WebDriverOptions.GetFirefoxOptions());
                case BrowserType.Chrome:
                    new DriverManager().SetUpDriver(new ChromeConfig());
                    return new ChromeDriver(WebDriverOptions.GetChromeOptions());
                default:
                    throw new Exception("Unspecified browser");
            }
        }

        private static IWebDriver GetRemoteWebDriver(BrowserType browserType)
        {
            switch (browserType)
            {
                case BrowserType.Firefox:
                    new DriverManager().SetUpDriver(new FirefoxConfig());
                    return new RemoteWebDriver(WebDriverOptions.GetFirefoxOptions());
                case BrowserType.Chrome:
                    new DriverManager().SetUpDriver(new ChromeConfig());
                    return new RemoteWebDriver(WebDriverOptions.GetChromeOptions());
                default:
                    throw new Exception("Unspecified browser");
            }
        }

    }
}
