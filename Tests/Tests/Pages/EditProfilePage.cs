using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Pages.PagesElements;
using Framework.Extensions;
using OpenQA.Selenium;
using Framework.Models;

namespace Tests.Pages
{
    public class EditProfilePage : BasePage
    {
        public EditProfilePageElements EditProfileElements { get; }

        public EditProfilePage(IWebDriver driver) : base(driver)
        {
            EditProfileElements = new EditProfilePageElements(driver);
        }

        internal void CancelEdition()
        {
            EditProfileElements.BackBtn.Click();
            Driver.WaitForAngularLoad();
        }

        internal void EditPet(Pet pet)
        {
            if (!string.IsNullOrEmpty(pet.Name)) EditProfileElements.NameField.TypeText(pet.Name);
            if (pet.Age != 0) EditProfileElements.AgeField.TypeText(pet.Age.ToString());
            if (!string.IsNullOrEmpty(pet.Gender)) EditProfileElements.GenderComboBox.SelectByText(pet.Gender);
            if (!string.IsNullOrEmpty(pet.City)) EditProfileElements.CityField.TypeText(pet.City);
            if (!string.IsNullOrEmpty(pet.Description)) EditProfileElements.DescriptionTextArea.TypeText(pet.Description);
        }

        internal string SaveEdition()
        {
            EditProfileElements.SaveBtn.Click();
            var message = GetToastMessage();
            EditProfileElements.BackBtn.Click();
            Driver.WaitForAngularLoad();
            return message;
        }

        protected override void WaitTillPageIsVisible()
        {
            Wait.Until(CustomExpectedConditions.ElementIsVisible(EditProfileElements.EditProfileHeader));
        }
    }
}
