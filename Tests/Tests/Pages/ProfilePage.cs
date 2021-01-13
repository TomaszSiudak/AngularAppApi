using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Pages.PagesElements;
using Framework.Extensions;
using OpenQA.Selenium;
using Framework.Models;
using System.Text.RegularExpressions;
using System.Threading;

namespace Tests.Pages
{
    public class ProfilePage : BasePage
    {
        public ProfilePageElements ProfileElements { get; }

        public ProfilePage(IWebDriver driver) : base(driver)
        {
            ProfileElements = new ProfilePageElements(driver);
        }

        internal bool IsLikeBtnVisible => IsElementVisible(ProfilePageElements.LikeBtnBy);

        internal Pet GetPetFromProfile()
        {
            Driver.WaitForAngularLoad();
            return new Pet()
            {
                Name = ProfileElements.Name.FindWebElement(By.TagName("span")).GetText(),
                Age = int.Parse(Regex.Match(ProfileElements.Age.GetText(), @"\d+$").Value),
                Gender = ProfileElements.Gender.FindWebElement(By.CssSelector("span")).GetText(),
                City = ProfileElements.City.FindWebElement(By.CssSelector("span")).GetText(),
                Description = ProfileElements.Description.GetText()
            };
        }

        internal List<Pet> GetPetsWhichLikedMyProfile()
        {
            List<Pet> pets = new List<Pet>();
            ProfileElements.LikesTab.Click();
            Wait.Until(CustomExpectedConditions.ElementIsVisible(By.CssSelector("#likersTab")));
            foreach (var card in ProfileElements.LikersCards)
            {
                string titleText = card.Title.GetText();
                var pet = new Pet()
                {
                    Name = Regex.Match(titleText, @"^\w+,").Value.Trim(','),
                    Age = int.Parse(Regex.Match(titleText, @"\d+$").Value),
                    Photos = new List<Photo>() { new Photo() { Url = card.Image.GetAttribute("src") } }
                };
                pets.Add(pet);
            }
            return pets;
        }


        internal EditProfilePage GoToEditPage()
        {
            ProfileElements.EditProfileBtn.Click();
            Driver.WaitForAngularLoad();
            return new EditProfilePage(Driver);
        }

        public string GetPetProfileNameHeader() => ProfileElements.ProfileHeader.FindWebElement(By.TagName("h2")).GetText();
        internal int GetNumberOfPhotos() => Driver.FindElements(By.CssSelector("a.ngx-gallery-thumbnail")).Count;
        internal string GetURLOfMainPhoto() => ProfileElements.MainPhoto.GetAttribute("src");

        internal void LikeVisitedPet() => ProfileElements.LikeBtn.Click();

        protected override void WaitTillPageIsVisible()
        {
            Wait.Until(CustomExpectedConditions.ElementIsVisible(ProfileElements.ProfileHeader));
        }
    }
}
