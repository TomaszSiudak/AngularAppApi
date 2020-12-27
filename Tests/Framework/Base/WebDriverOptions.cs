using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Base
{
    public class WebDriverOptions
    {
        public static ChromeOptions GetChromeOptions()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddAdditionalCapability("useAutomationExtension", false);
            chromeOptions.AddArgument("--enable-automation");
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("--disable-infobars");
            chromeOptions.AddArgument("--disable-save-password-bubble");
            chromeOptions.AddArgument("ignore-certificate-errors");
            chromeOptions.AddArgument("--start-maximized");
            chromeOptions.AddArgument($"--lang=en");

            return chromeOptions;
        }

        public static FirefoxOptions GetFirefoxOptions()
        {
            FirefoxOptions firefoxOptions = new FirefoxOptions() { AcceptInsecureCertificates = true };
            return firefoxOptions;
        }
    }
}
