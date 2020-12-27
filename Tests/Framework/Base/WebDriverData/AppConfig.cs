using Framework.Constants.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Framework.Base.WebDriverFactory;

namespace Framework.Base
{
    public class AppConfig
    {
        public BrowserType BrowserType { get; set; }
        public EnvironmentType Environment { get; set; }
        public string EnvironmentURL { get; set; }
        public string GridUrl { get; set; }

    }
}
