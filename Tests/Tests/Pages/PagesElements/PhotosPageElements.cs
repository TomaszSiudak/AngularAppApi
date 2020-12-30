﻿using Framework.Extensions;
using Framework.SeleniumWrappers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Pages.PagesElements
{
    public class PhotosPageElements
    {
        private IWebDriver driver;

        public WebElement PhotosPageHeader => driver.FindWebElement(By.Id("photosHeader"));
        public WebElement ApplyBtn => driver.FindWebElement(By.Id("applyBtn"));
        public WebElement ResetFilterBtn => driver.FindWebElement(By.Id("resetBtn"));
        public SelectElement GendersFilterComboBox { get; }
        public SelectElement TypesFilterComboBox { get; }

        public List<Card> PetCards
        {
            get
            {
                List<Card> cards = new List<Card>();
                foreach (var cardElement in driver.FindWebElements(By.XPath("//*[@class='card']")))
                {
                    var card = new Card()
                    {
                        Title = cardElement.FindWebElement(By.CssSelector(".card-title")),
                        Image = cardElement.FindWebElement(By.CssSelector(".card-image")),
                        Footer = cardElement.FindWebElement(By.CssSelector(".card-footer")),
                        LikeBtn = cardElement.FindWebElement(By.CssSelector("#likeBtn"))
                    };
                    cards.Add(card);
                }
                return cards;
            }
        }

        public PhotosPageElements(IWebDriver driver)
        {
            this.driver = driver;
            GendersFilterComboBox = new SelectElement(driver.FindElement(By.Id("genderFilter")));
            TypesFilterComboBox = new SelectElement(driver.FindElement(By.Id("typeFilter")));
        }
    }
}