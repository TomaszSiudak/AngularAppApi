using Framework.Constants.Enums;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace Framework.Base
{
    public class WebDriverFactory
    {
        private static string DriversDirectoryPath = "";

        public static IWebDriver GetWebDriver(AppConfig config)
        {
            return config.Environment.Equals(EnvironmentType.Local) ? GetLocalWebDriver(config.BrowserType) : GetRemoteWebDriver(config.BrowserType);
        }

        private static IWebDriver GetLocalWebDriver(BrowserType browserType)
        {
            switch (browserType)
            {
                case BrowserType.Firefox:
                    return new FirefoxDriver(WebDriverOptions.GetFirefoxOptions());
                default:
                    return new ChromeDriver(WebDriverOptions.GetChromeOptions());
            }
        }

        private static IWebDriver GetRemoteWebDriver(BrowserType browserType)
        {
            switch (browserType)
            {
                case BrowserType.Firefox:
                    return new RemoteWebDriver(WebDriverOptions.GetFirefoxOptions());
                default:
                    return new RemoteWebDriver(WebDriverOptions.GetChromeOptions());
            }
        }

    }
}
