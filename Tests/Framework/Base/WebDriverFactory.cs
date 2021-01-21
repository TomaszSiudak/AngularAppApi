using Framework.Constants.Enums;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.Extensions;
using System;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace Framework.Base
{
    public class WebDriverFactory
    {
        public static IWebDriver GetWebDriver(AppConfig config)
        {
            return config.Environment.Equals(EnvironmentType.Local) ? GetLocalWebDriver(config.BrowserType) : GetRemoteWebDriver(config.BrowserType, config);
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
                    return WrapDriverInEventFiringWebDriver(new ChromeDriver(WebDriverOptions.GetChromeOptions()));
                default:
                    throw new Exception("Unspecified browser");
            }
        }

        private static IWebDriver GetRemoteWebDriver(BrowserType browserType, AppConfig config)
        {
            switch (browserType)
            {
                case BrowserType.Firefox:
                    new DriverManager().SetUpDriver(new FirefoxConfig());
                    return new RemoteWebDriver(WebDriverOptions.GetFirefoxOptions());
                case BrowserType.Chrome:
                    new DriverManager().SetUpDriver(new ChromeConfig());
                    return new RemoteWebDriver(new Uri($"{config.GridUrl}/wd/hub"), WebDriverOptions.GetChromeOptions());
                default:
                    throw new Exception("Unspecified browser");
            }
        }


        private static IWebDriver WrapDriverInEventFiringWebDriver(IWebDriver webDriver)
        {
            EventFiringWebDriver eventDriver = new EventFiringWebDriver(webDriver);
            EventHandler<WebElementEventArgs> BeforeElementClickedHandler = (object sender, WebElementEventArgs args) =>
            {
                try
                {
                    webDriver.ExecuteJavaScript("arguments[0].setAttribute('style', arguments[1]);", args.Element, "border: 6px solid red");
                }
                catch (StaleElementReferenceException)
                {

                }          
            };
            eventDriver.ElementClicked += new EventHandler<WebElementEventArgs>(BeforeElementClickedHandler);
            return eventDriver;
        }
    }
}
