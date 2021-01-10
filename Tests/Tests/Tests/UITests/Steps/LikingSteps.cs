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

namespace Tests.Tests.UITests.Steps
{
    [Binding]
    public sealed class LikingSteps : BaseStep
    {
        private readonly ScenarioContext scenarioContext;

        public LikingSteps(ScenarioContext scenarioContext, IWebDriver driver, MainPage mainPage, PetsPage petsPage, ProfilePage profilePage)
        {
            this.scenarioContext = scenarioContext;
            Driver = driver;
            MainPage = mainPage;
            PetsPage = petsPage;
            ProfilePage = profilePage;
        }


        [When(@"I like the profile of random user at Pets list")]
        public void WhenILikeTheProfileOfRandomUserAtPetsList()
        {
            var likedPet = SqlHelper.Pets.GetRandomPetByLikes(hasLikes: false);
            scenarioContext.Add("likedPet", likedPet);
            PetsPage.LikePet(likedPet);
            string message = PetsPage.GetToastMessage();
            scenarioContext.Add("toast", message);
        }

        [When(@"I like the visited profile")]
        public void WhenILikeTheVisitedProfile()
        {
            var likedPet = SqlHelper.Pets.GetRandomPetByLikes(hasLikes: false);
            scenarioContext.Add("likedPet", likedPet);
            PetsPage.VisitPetProfile(likedPet);
            ProfilePage.LikeVisitedPet();
            string message = PetsPage.GetToastMessage();
            scenarioContext.Add("toast", message);
        }

        [When(@"I like the profile of already liked user several times")]
        public void WhenILikeTheProfileOfAlreadyLikedUserSeveralTimes()
        {
            var likedPet = SqlHelper.Pets.GetRandomPetByLikes(hasLikes: true);
            scenarioContext.Add("likedPet", likedPet);
            PetsPage.LikePet(likedPet);
            PetsPage.GetToastMessage();
            PetsPage.LikePet(likedPet);
            string message = PetsPage.GetToastMessage();
            scenarioContext.Add("toast", message);
        }

        [When(@"I try to add like to my own account")]
        public void WhenITryToAddLikeToMyOwnAccount()
        {
            var pet = scenarioContext["pet"] as Pet;
            bool isLikeBtnAtListVisible = PetsPage.IsLikeBtnAtListVisible(pet);
            ProfilePage = MainPage.GoToMyProfile();
            bool isLikeBtnAtProfileVisible = ProfilePage.IsLikeBtnVisible;
            scenarioContext.Add("likeBtnList", isLikeBtnAtListVisible);
            scenarioContext.Add("likeBtnProfile", isLikeBtnAtProfileVisible);
        }



        [Then(@"the liked user see it on its own profile")]
        public void ThenTheLikedUserSeeItOnItsOwnProfile()
        {
            var pet = scenarioContext["pet"] as Pet;
            var likedPet = scenarioContext["likedPet"] as Pet;
            string expectedToastMessage = Messages.YouLikedTheUser + likedPet.Name;
            MainPage.Login(likedPet.Name, likedPet.Password);
            ProfilePage = MainPage.GoToMyProfile();
            var petsWhichLikedTheCurrentProfile = ProfilePage.GetPetsWhichLikedMyProfile();
            var expectedNumberOfLikesInDB = SqlHelper.Likes.GetLikesOfPetById(likedPet.Id).Count;
            var petWhichLiked = petsWhichLikedTheCurrentProfile.FirstOrDefault();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedToastMessage, scenarioContext["toast"], "Actual toast message after liking the profile should match expected");
                Assert.AreEqual(pet.Name, petWhichLiked.Name, "Actual profile Name value for user which liked the pet should match expected");
                Assert.AreEqual(pet.Age, petWhichLiked.Age, "Actual profile Age value for user which liked the pet should match expected");
                Assert.AreEqual(pet.Photos.FirstOrDefault().Url, petWhichLiked.Photos.FirstOrDefault().Url, "Actual profile URL value for user which liked the pet should match expected");
                Assert.AreEqual(expectedNumberOfLikesInDB, petsWhichLikedTheCurrentProfile.Count, "The number of users which liked the current pet at UI should match expected number in DB");
            });
        }

        [Then(@"the like is not added")]
        public void ThenTheLikeIsNotAdded()
        {
            var pet = scenarioContext["pet"] as Pet;
            var likedPet = scenarioContext["likedPet"] as Pet;
            string expectedToastMessage = Messages.PetWasAlreadyLiked;
            MainPage.Login(likedPet.Name, likedPet.Password);
            ProfilePage = MainPage.GoToMyProfile();
            var petsWhichLikedTheCurrentProfile = ProfilePage.GetPetsWhichLikedMyProfile();
            var expectedNumberOfLikesInDB = SqlHelper.Likes.GetLikesOfPetById(likedPet.Id).Count;
            var petWhichLikedNumberOfCards = petsWhichLikedTheCurrentProfile.Where(p => p.Name.Equals(pet.Name)).Count();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedToastMessage, scenarioContext["toast"], "Actual toast message after liking the profile once again should match expected");
                Assert.AreEqual(1, petWhichLikedNumberOfCards, "There should be only one card of the user which liked pet because like can be added only once");
                Assert.AreEqual(expectedNumberOfLikesInDB, petsWhichLikedTheCurrentProfile.Count, "The number of users which liked the current pet at UI should match expected number in DB");
            });
        }

        [Then(@"the buttons are NOT present")]
        public void ThenTheButtonsAreNOTPresent()
        {
            Assert.Multiple(() =>
            {
                Assert.IsFalse((bool)scenarioContext["likeBtnList"], "Like btn at Pets list should NOT be visible for its own account");
                Assert.IsFalse((bool)scenarioContext["likeBtnProfile"], "Like btn at Profile should NOT be visible for its own account");
            });
        }

    }
}
