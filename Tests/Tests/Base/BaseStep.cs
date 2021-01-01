using Framework.Base;
using Framework.Base.WebDriverData;
using Framework.Helpers.SqlHelper;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Tests.Pages;

namespace Tests.Base
{
    public class BaseStep
    {
        protected IWebDriver Driver;

        private SqlHelper _sqlHelper;
        protected SqlHelper SqlHelper
        {
            get
            {
                if (_sqlHelper == null)
                    return _sqlHelper = new SqlHelper();
                return _sqlHelper;
            }
        }

        private BasePage currentPage;

        public BasePage CurrentPage
        {
            get { return currentPage; }
            set
            {
                ScenarioContext.Current["class"] = value;
                currentPage = ScenarioContext.Current.Get<BasePage>("class"); 
            }
        }


    }
}
