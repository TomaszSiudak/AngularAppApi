using Framework.Extensions;
using Framework.SeleniumWrappers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Pages.PagesElements
{
    public class NavigationMenuElements
    {
        private IWebDriver driver;

        public WebElement MainPage => driver.FindWebElement(By.CssSelector("#mainPageLink"));
        public WebElement Photos => driver.FindWebElement(By.CssSelector("#photosLink"));
        public WebElement MyProfile => driver.FindWebElement(By.Id("myProfileLink"));
        public WebElement RightMenu => driver.FindWebElement(By.XPath("//*[@id='dropdownMenuLink']"));
        public WebElement EditAccountBtn => driver.FindWebElement(By.Id("editAccount"));
        public WebElement LogOutBtn => driver.FindWebElement(By.Id("logOutBtn"));
        public WebElement UsernameField => driver.FindWebElement(By.CssSelector("#usernameTextBox"));
        public WebElement PasswordField => driver.FindWebElement(By.CssSelector("#passwordTextBox"));
        public WebElement LogInBtn => driver.FindWebElement(By.Id("logInBtn"));

        public NavigationMenuElements(IWebDriver driver)
        {
            this.driver = driver;
        }
    }
}
