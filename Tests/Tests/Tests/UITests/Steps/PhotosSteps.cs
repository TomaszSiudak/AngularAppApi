using FluentAssertions;
using FluentAssertions.Execution;
using Framework.Constants;
using Framework.Helpers;
using Framework.Models;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using Tests.Base;
using Tests.Pages;
using Tests.Pages.PagesElements;

namespace Tests.Tests.UITests.Steps
{
    [Binding]
    public sealed class PhotosSteps : BaseStep
    {
        private readonly ScenarioContext scenarioContext;

        public PhotosSteps(ScenarioContext scenarioContext, ProfilePage profilePage, EditProfilePage editProfilePage)
        {
            this.scenarioContext = scenarioContext;
            EditProfilePage = editProfilePage;
            ProfilePage = profilePage;
        }

        [When(@"I upload new photo")]
        public void WhenIUploadNewPhoto()
        {
            int numberOfPhotosBeforeUpload = EditProfilePage.GetNumberOfPhotos();
            scenarioContext.Add("numberOfPhotosBeforeUpload", numberOfPhotosBeforeUpload);
            EditProfilePage.SelectRandomPhoto();
        }

        [When(@"I save")]
        public void WhenISave()
        {
            EditProfilePage.UploadPhoto();
            scenarioContext.Add("toast", EditProfilePage.GetToastMessage());
        }

        [When(@"I cancel upload")]
        public void WhenICancelUpload()
        {
            EditProfilePage.CancelUploadingPhoto();
        }

        [When(@"I delete unwanted photo")]
        public void WhenIDeleteUnwantedPhoto()
        {
            int numberOfPhotosBeforeDelete = EditProfilePage.GetNumberOfPhotos();
            scenarioContext.Add("numberOfPhotosBeforeDelete", numberOfPhotosBeforeDelete);
            EditProfilePage.DeletePhoto(1);
            scenarioContext.Add("toast", EditProfilePage.GetToastMessage());
        }

        [When(@"I set another photo as main")]
        public void WhenISetAnotherPhotoAsMain()
        {
            pet = scenarioContext["pet"] as Pet;
            string anotherPhotoUrl = pet.Photos.FirstOrDefault(ph => ph.MainPhoto != true).Url;
            scenarioContext.Add("anotherPhotoUrl", anotherPhotoUrl);
            EditProfilePage.SetMainPhotoByUrl(anotherPhotoUrl);
            scenarioContext.Add("toast", EditProfilePage.GetToastMessage());
        }

        [Then(@"I do not see new photo added to my account")]
        public void ThenIDoNotSeeNewPhotoAddedToMyAccount()
        {
            int numberOfPhotosAfterUpload = EditProfilePage.GetNumberOfPhotos();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(Convert.ToInt16(scenarioContext["numberOfPhotosBeforeUpload"]), numberOfPhotosAfterUpload, "The actual number of photos after canceling upload should be the same as before selecting file");
            });
        }


        [Then(@"I see new photo added to my account")]
        public void ThenISeeNewPhotoAddedToMyAccount()
        {
            string expectedToastMessage = Messages.PhotoUploadSuccessfull;
            int numberOfPhotosAfterUpload = EditProfilePage.GetNumberOfPhotos();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(Convert.ToInt16(scenarioContext["numberOfPhotosBeforeUpload"]) + 1, numberOfPhotosAfterUpload, "The actual number of photos after upload should match expected");
                Assert.AreEqual(expectedToastMessage, scenarioContext["toast"], "Actual toast message after uploading photo should match expected");
            });
        }

        [Then(@"I see one photo less in my account")]
        public void ThenISeeOnePhotoLessInMyAccount()
        {
            int expectedNumberOfPhotosAfterDelete = Convert.ToInt16(scenarioContext["numberOfPhotosBeforeDelete"]) - 1;
            string expectedToastMessage = Messages.PhotoDeleted;
            int numberOfPhotosAfterDelete = EditProfilePage.GetNumberOfPhotos();
            EditProfilePage.ReturnToProfile();
            int numberOfPhotosAfterDeleteAtProfile = ProfilePage.GetNumberOfPhotos();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedNumberOfPhotosAfterDelete, numberOfPhotosAfterDelete, "The actual number of photos at profile edition page after delete should match expected");
                Assert.AreEqual(expectedNumberOfPhotosAfterDelete, numberOfPhotosAfterDeleteAtProfile, "The actual number of photos profile page after delete should match expected");
                Assert.AreEqual(expectedToastMessage, scenarioContext["toast"], "Actual toast message after deleting photo should match expected");
            });
        }

        [Then(@"I see main photo updated")]
        public void ThenISeeMainPhotoUpdated()
        {
            string expectedURLOfNewMainPhoto = scenarioContext["anotherPhotoUrl"].ToString();
            string expectedToastMessage = Messages.MainPhotoHasBeenChanged;
            EditProfilePage.ReturnToProfile();
            string mainPhotoURLAfterUpdate = ProfilePage.GetURLOfMainPhoto();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedURLOfNewMainPhoto, mainPhotoURLAfterUpdate, "Actual photo 'src' value after setting another photo as main should match expected");
                Assert.AreEqual(expectedToastMessage, scenarioContext["toast"], "Actual toast message after setting another photo as main should match expected");
            });
        }
    }
}
