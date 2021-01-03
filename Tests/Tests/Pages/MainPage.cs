using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tests.Pages.PagesElements;
using Framework.Extensions;
using Framework.Base.WebDriverData;
using Framework.Models;

namespace Tests.Pages
{
    public class MainPage : BasePage
    {
        private static string URL = AppConfigProvider.AppConfigInstance.EnvironmentURL;
        public MainPageElements MainPageElements { get; }

        public MainPage(IWebDriver driver) : base(driver)
        {
            MainPageElements = new MainPageElements(driver);
        }

        internal void FillTheRegistrationForm(Pet pet)
        {
            MainPageElements.UsernameField.TypeText(pet.Name);
            MainPageElements.PasswordField.TypeText(pet.Password);
            MainPageElements.ConfirmPasswordField.TypeText(pet.ConfirmPassword);
            MainPageElements.GendersRadioBtns.First(el => el.GetText().Equals(pet.Gender)).Click();
            MainPageElements.TypesComboBox.SelectByText(pet.Type);
            MainPageElements.CityField.TypeText(pet.City);
        }

        public MainPage GoToMainPage()
        {
            Driver.Url = URL;
            WaitTillPageIsVisible();
            return this;
        }

        internal void OpenRegistrationForm()
        {
            MainPageElements.RegistrationBtn.Click();
            Wait.Until(CustomExpectedConditions.ElementIsVisible(MainPageElements.UsernameField));
        }

        protected override void WaitTillPageIsVisible()
        {
            Wait.Until(CustomExpectedConditions.ElementIsVisible(MainPageElements.MainPageHeader));
        }
    }
}
