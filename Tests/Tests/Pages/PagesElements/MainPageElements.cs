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

        public WebElement MainPageHeader => driver.FindWebElement(By.Id("mainPageHeader"));
        public WebElement RegistrationBtn => driver.FindWebElement(By.Id("registrationBtn"));
        public WebElement UsernameField => driver.FindWebElement(By.CssSelector("#username_Registration"));
        public WebElement PasswordField => driver.FindWebElement(By.CssSelector("#password_Registration"));
        public WebElement ConfirmPasswordField => driver.FindWebElement(By.CssSelector("#confirmPassword_Registration"));
        public List<WebElement> GendersRadioBtns => driver.FindWebElements(By.CssSelector("#genderRadioBtn_Registration"));
        public List<WebElement> CityField => driver.FindWebElements(By.CssSelector("#city_Registration"));
        public WebElement RegisterBtn => driver.FindWebElement(By.Id("registerBtn"));
        public WebElement CancelBtn => driver.FindWebElement(By.Id("cancelRegistrationBtn"));
        public SelectElement TypesComboBox { get; }

        public MainPageElements(IWebDriver driver)
        {
            this.driver = driver;
            TypesComboBox = new SelectElement(driver.FindElement(By.XPath("//*[@id='animalType_Registration']")));
        }
    }
}