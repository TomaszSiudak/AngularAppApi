using Framework.Extensions;
using Framework.SeleniumWrappers;
using OpenQA.Selenium;

namespace Tests.Pages.PagesElements
{
    public class NavigationMenuElements
    {
        private IWebDriver driver;

        #region xpaths

        public static By MyProfileBy = By.Id("myProfileLink");
        public static By RightMenuBtnBy = By.XPath("//*[@id='dropdownMenuLink']");

        #endregion xpaths

        public WebElement MainPage => driver.FindWebElement(By.CssSelector("#mainPageLink"));
        public WebElement Photos => driver.FindWebElement(By.CssSelector("#photosLink"));
        public WebElement MyProfile => driver.FindWebElement(MyProfileBy);
        public WebElement RightMenuBtn => driver.FindWebElement(RightMenuBtnBy);
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
