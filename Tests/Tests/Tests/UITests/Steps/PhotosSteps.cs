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

        public PhotosSteps(ScenarioContext scenarioContext, EditProfilePage editProfilePage)
        {
            this.scenarioContext = scenarioContext;
            EditProfilePage = editProfilePage;
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

    }
}
