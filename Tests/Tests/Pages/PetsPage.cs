using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tests.Pages.PagesElements;
using Framework.Extensions;
using Framework.Base.WebDriverData;
using Framework.Models;
using System.Text.RegularExpressions;
using System.Threading;

namespace Tests.Pages
{
    public class PetsPage : BasePage
    {
        protected override string URL { get { return AppConfigProvider.AppConfigInstance.EnvironmentURL + "pets"; } }
        public PetsPageElements PetsPageElements { get; }

        public PetsPage(IWebDriver driver) : base(driver)
        {
            PetsPageElements = new PetsPageElements(driver);
        }

        internal void FilterPets(string gender = null, string type = null)
        {
            if (gender != null) SetGenderComboBox(gender.ToLower());
            if (type != null) SetTypeComboBox(type.ToLower());
            PetsPageElements.ApplyBtn.Click();
            Driver.WaitForAngularLoad();
        }

        private Card FindCardOfDesiredPet(Pet likedPet)
        {
            Card likedPetCard = null;
            int i = 0;
            while (i < PetsPageElements.PageNumbers.Count)
            {
                Driver.FindElements(PetsPageElements.PageNumbers[i].BySelector)[i].Click();
                Driver.WaitForAngularLoad();
                likedPetCard = PetsPageElements.PetCards.FirstOrDefault(card => Regex.Match(card.Title.GetText(), @"^\w+,").Value.Trim(',').Equals(likedPet.Name));
                if (likedPetCard != null) break;
                i++;
            }
            return likedPetCard;
        }

        internal List<Pet> GetPetsFromCards()
        {
            List<Pet> pets = new List<Pet>();
            int i = 0;
            foreach (var page in PetsPageElements.PageNumbers)
            {
                Driver.FindElements(page.BySelector)[i].Click();
                Wait.Until(Driver => PetsPageElements.PageNumbers[i].GetAttribute("class").Contains("active"));
                Driver.WaitForAngularLoad();
                foreach (var card in PetsPageElements.PetCards)
                {
                    string titleText = card.Title.GetText();
                    var pet = new Pet()
                    {
                        Name = Regex.Match(titleText, @"^\w+,").Value.Trim(','),
                        Age = int.Parse(Regex.Match(titleText, @"\d+$").Value),
                        City = card.Footer.GetText()
                    };
                    pets.Add(pet);
                }
                i++;
            }
            return pets;
        }

        private void InvokeActionOnCard(Card likedPetCard, CardElement cardElement)
        {
            switch (cardElement)
            {
                case CardElement.Image:
                    likedPetCard.Image.Click();
                    break;
                case CardElement.LikeBtn:
                    likedPetCard.LikeBtn.Click();
                    break;
                default:
                    break;
            }
        }

        internal bool IsLikeBtnAtListVisible(Pet pet)
        {
            var foundCard = FindCardOfDesiredPet(pet);
            return foundCard.LikeBtn != null;
        }

        internal void LikePet(Pet likedPet)
        {
            var foundCard = FindCardOfDesiredPet(likedPet);
            InvokeActionOnCard(foundCard, CardElement.LikeBtn);
            Driver.WaitForAngularLoad();
        }

        public PetsPage NavigateToPetsPage()
        {
            Driver.Url = URL;
            WaitTillPageIsVisible();
            return this;
        }

        internal void RemoveFilter()
        {
            PetsPageElements.ResetFilterBtn.Click();
            Driver.WaitForAngularLoad();
        }

        private void SetGenderComboBox(string gender)
        {
            PetsPageElements.GendersFilterComboBox.SelectByText(gender.Replace(gender[0], gender.ToUpper()[0]));
        }

        private void SetTypeComboBox(string type)
        {
            PetsPageElements.TypesFilterComboBox.SelectByText(type.ToLower().Replace(type[0], type.ToUpper()[0]));
        }

        protected override void WaitTillPageIsVisible()
        {
            Wait.Until(CustomExpectedConditions.ElementIsVisible(PetsPageElements.PetsPageHeader));
        }

        internal void VisitPetProfile(Pet likedPet)
        {
            var foundCard = FindCardOfDesiredPet(likedPet);
            InvokeActionOnCard(foundCard, CardElement.Image);
            Driver.WaitForAngularLoad();
        }

        private enum CardElement { Image, LikeBtn }
    }
}
