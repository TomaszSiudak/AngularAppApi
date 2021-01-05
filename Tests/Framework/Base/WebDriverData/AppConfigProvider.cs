using Framework.Constants.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Framework.Base.AppConfig;

namespace Framework.Base.WebDriverData
{
    public class AppConfigProvider
    {
        private static AppConfig _appConfig;
        public static AppConfig AppConfigInstance 
        {
            get
            {
                if (_appConfig != null) return _appConfig;
                _appConfig = new AppConfig()
                {
                    BrowserType = (BrowserType)Enum.Parse(typeof(BrowserType), ConfigurationManager.AppSettings["browserType"]),
                    Environment = (EnvironmentType)Enum.Parse(typeof(EnvironmentType), ConfigurationManager.AppSettings["environment"]),
                    EnvironmentURL = ConfigurationManager.AppSettings["environmentURL"],
                    GridUrl = ConfigurationManager.AppSettings["gridURL"],
                    ScreenshotsDirPath = ConfigurationManager.AppSettings["screenshotsDir"]
                };
                return _appConfig;
            } 
        }
        private AppConfigProvider() { }
    }
}
