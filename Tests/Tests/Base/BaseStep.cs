using Framework.Base;
using Framework.Base.WebDriverData;
using Framework.Helpers.SqlHelper;
using Framework.Models;
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
        protected MainPage MainPage;
        protected PetsPage PetsPage;
        protected ProfilePage ProfilePage;
        protected EditProfilePage EditProfilePage;
        protected Pet pet;

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

    }
}
