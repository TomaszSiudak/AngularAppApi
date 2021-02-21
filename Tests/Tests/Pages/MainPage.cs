using OpenQA.Selenium;
using System.Linq;
using Tests.Pages.PagesElements;
using Framework.Extensions;
using Framework.Models;

namespace Tests.Pages
{
    public class MainPage : BasePage
    {
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

        public string GetRegistrationFormVisibleHintText() => Driver.FindWebElements(By.CssSelector(".invalid-feedback")).FirstOrDefault(el => el.IsVisible()).GetText();

        internal void OpenRegistrationForm()
        {
            MainPageElements.RegistrationBtn.Click();
            Wait.Until(CustomExpectedConditions.ElementIsVisible(MainPageElements.RegistrationFormBy));
        }

        internal void RegisterPet()
        {
            MainPageElements.RegisterPetBtn.Click();
            Wait.Until(CustomExpectedConditions.InvisibilityOfElement(MainPageElements.RegistrationFormBy));
        }

        internal void TryRegisterPet() => MainPageElements.RegisterPetBtn.Click();


        protected override void WaitTillPageIsVisible()
        {
            Wait.Until(CustomExpectedConditions.ElementIsVisible(MainPageElements.MainPageHeader));
        }
    }
}
