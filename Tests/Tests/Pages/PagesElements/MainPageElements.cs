using Framework.Extensions;
using Framework.SeleniumWrappers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace Tests.Pages.PagesElements
{
    public class MainPageElements
    {
        private IWebDriver driver;

        #region xpaths

        public static By MainPageHeaderBy = By.Id("mainPageHeader");

        #endregion xpaths

        public WebElement MainPageHeader => driver.FindWebElement(MainPageHeaderBy);
        public WebElement RegistrationBtn => driver.FindWebElement(By.Id("registrationBtn"));
        public WebElement UsernameField => driver.FindWebElement(By.CssSelector("#username_Registration"));
        public WebElement PasswordField => driver.FindWebElement(By.CssSelector("#password_Registration"));
        public WebElement ConfirmPasswordField => driver.FindWebElement(By.CssSelector("#confirmPassword_Registration"));
        public List<WebElement> GendersRadioBtns => driver.FindWebElements(By.CssSelector("#genderRadioBtn_Registration"));
        public WebElement CityField => driver.FindWebElement(By.CssSelector("#city_Registration"));
        public WebElement RegisterPetBtn => driver.FindWebElement(By.Id("registerBtn"));
        public WebElement CancelBtn => driver.FindWebElement(By.Id("cancelRegistrationBtn"));

        public SelectElement TypesComboBox{ get { return new SelectElement(driver.FindElement(By.XPath("//*[@id='animalType_Registration']"))); } }


        public MainPageElements(IWebDriver driver)
        {
            this.driver = driver;
        }
    }
}