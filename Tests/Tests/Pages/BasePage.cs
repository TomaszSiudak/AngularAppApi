using Framework.Base.WebDriverData;
using Framework.Extensions;
using Framework.Helpers;
using Framework.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Html5;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Pages.PagesElements;

namespace Tests.Pages
{
    public abstract class BasePage
    {
        protected virtual string URL { get { return AppConfigProvider.AppConfigInstance.EnvironmentURL; } }

        #region xpaths

        public static string toastMessageXpath = "//*[contains(@class, 'ajs-message')]";

        #endregion xpaths
        protected IWebDriver Driver { get; }
        protected WebDriverWait Wait { get; set; }
        public NavigationMenuElements NavigationMenuElements { get; }


        protected BasePage(IWebDriver driver, int defaultWaitTime = 20)
        {
            Driver = driver;
            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(defaultWaitTime));
            NavigationMenuElements = new NavigationMenuElements(Driver);
        }

        public void AuthenticatePet(Pet pet)
        {
            var token =  AuthenticationHelper.GetToken(pet);
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            js.ExecuteScript("localStorage.setItem(arguments[0],arguments[1])", "token", token);
        }

        public T As<T>() where T : BasePage
        {
            return (T)this;
        }

        private void FillLoginFormCredentials(string name, string password)
        {
            NavigationMenuElements.UsernameField.TypeText(name);
            NavigationMenuElements.PasswordField.TypeText(password);
            Console.WriteLine($"Name: '{name}' and Password: '{password}' was entered");
        }

        public string GetToastMessage()
        {
            IWebElement toastMessageElement = Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(toastMessageXpath)));
            string message = toastMessageElement.Text;
            Wait.Until(CustomExpectedConditions.InvisibilityOfElement(By.XPath(toastMessageXpath)));
            return message;
        }

        public void GoToMainPage()
        {
            Driver.Url = URL;
            WaitTillPageIsVisible();
        }

        public bool IsElementVisible(By selector)
        {
            bool displayed;
            try
            {
                displayed = Driver.FindElement(selector).Displayed;
            }
            catch (WebDriverException)
            {
                return false;
            }
            return displayed;
        }

        public PetsPage Login(string name, string password)
        {
            FillLoginFormCredentials(name, password);
            NavigationMenuElements.LogInBtn.Click();
            var photosPage = new PetsPage(Driver);
            photosPage.WaitTillPageIsVisible();
            return photosPage;
        }

        public void TryToLogin(string name, string password)
        {
            FillLoginFormCredentials(name, password);
            NavigationMenuElements.LogInBtn.Click();
        }


        protected abstract void WaitTillPageIsVisible();
    }
}
