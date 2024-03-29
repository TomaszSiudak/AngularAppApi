﻿using System.Linq;
using Tests.Pages.PagesElements;
using Framework.Extensions;
using OpenQA.Selenium;
using Framework.Models;
using System.IO;
using System.Reflection;

namespace Tests.Pages
{
    public class EditProfilePage : BasePage
    {
        public EditProfilePageElements EditProfileElements { get; }

        public EditProfilePage(IWebDriver driver) : base(driver)
        {
            EditProfileElements = new EditProfilePageElements(driver);
        }

        internal void CancelUploadingPhoto() => EditProfileElements.CancelUploadBtn.Click();

        internal void DeletePhoto(int index) => Driver.FindElements(EditProfilePageElements.DeletePhotoBtnBy)[index].Click();

        internal void EditPet(Pet pet)
        {
            if (!string.IsNullOrEmpty(pet.Name)) EditProfileElements.NameField.TypeText(pet.Name);
            if (pet.Age != 0) EditProfileElements.AgeField.TypeText(pet.Age.ToString());
            if (!string.IsNullOrEmpty(pet.Gender)) EditProfileElements.GenderComboBox.SelectByText(pet.Gender);
            if (!string.IsNullOrEmpty(pet.City)) EditProfileElements.CityField.TypeText(pet.City);
            if (!string.IsNullOrEmpty(pet.Description)) EditProfileElements.DescriptionTextArea.TypeText(pet.Description);
        }

        internal int GetNumberOfPhotos() => Driver.FindElements(By.CssSelector(".img-thumbnail")).Count;

                internal void ReturnToProfile()
        {
            EditProfileElements.BackBtn.Click();
            Driver.WaitForAngularLoad();
        }

        internal string SaveEdition()
        {
            EditProfileElements.SaveBtn.Click();
            var message = GetToastMessage();
            EditProfileElements.BackBtn.Click();
            Driver.WaitForAngularLoad();
            return message;
        }

        internal void SelectRandomPhoto()
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"TestData\Photos\cat.png");
            Driver.FindElement(EditProfileElements.SelectFileInput.BySelector).SendKeys(path);
            Wait.Until(CustomExpectedConditions.ElementToBeClickable(EditProfileElements.UploadPhotoBtn));
        }

        internal void SetMainPhotoByUrl(string url)
        {
            var searchedPhotoCard = EditProfileElements.Photos.FirstOrDefault(ph => ph.Image.GetAttribute("src").Equals(url));
            searchedPhotoCard.SetMainPhotoBtn.ClickWithoutWait();
        }

        internal void UploadPhoto() => EditProfileElements.UploadPhotoBtn.Click();

        protected override void WaitTillPageIsVisible()
        {
            Wait.Until(CustomExpectedConditions.ElementIsVisible(EditProfileElements.EditProfileHeader));
        }
    }
}
