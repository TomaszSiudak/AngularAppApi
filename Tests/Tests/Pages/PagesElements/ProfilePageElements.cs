﻿using Framework.Extensions;
using Framework.SeleniumWrappers;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace Tests.Pages.PagesElements
{
    public class ProfilePageElements
    {
        private IWebDriver driver;

        #region xpaths

        public static By ProfilePageHeaderBy = By.CssSelector("#profileHeader");
        public static By LikeBtnBy = By.CssSelector("#profileLikeBtn");

        #endregion xpaths

        public WebElement ProfileHeader => driver.FindWebElement(By.CssSelector("#profileHeader"));
        public WebElement EditProfileBtn => driver.FindWebElement(By.CssSelector("#editProfileBtn"));
        public WebElement Name => driver.FindWebElement(By.Id("profileName"));
        public WebElement Age => driver.FindWebElement(By.Id("profileAge"));
        public WebElement Gender => driver.FindWebElement(By.Id("profileGender"));
        public WebElement City => driver.FindWebElement(By.Id("profileCity"));
        public WebElement Description => driver.FindWebElement(By.Id("profileDescription"));
        public WebElement MainPhoto => driver.FindWebElement(By.CssSelector(".prof-page-info img"));
        public WebElement LikeBtn => driver.FindWebElement(By.CssSelector("#profileLikeBtn"));
        public WebElement PhotosTab => driver.FindWebElement(By.XPath("//a[@aria-controls='photosTab']"));
        public WebElement LikesTab => driver.FindWebElement(By.XPath("//a[@aria-controls='likersTab']"));

        public List<Card> LikersCards
        {
            get
            {
                List<Card> cards = new List<Card>();
                foreach (var cardElement in driver.FindWebElements(By.XPath("//tab[@id='likersTab']//div[@class='row']/div")))
                {
                    var card = new Card()
                    {
                        Title = cardElement.FindWebElement(By.CssSelector("#likerCardTitle")),
                        Image = cardElement.FindWebElement(By.CssSelector("#likerCardImage")),
                    };
                    cards.Add(card);
                }
                return cards;
            }
        }


        public ProfilePageElements(IWebDriver driver)
        {
            this.driver = driver;
        }
    }
}
