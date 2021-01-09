using Framework.Extensions;
using Framework.SeleniumWrappers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.Pages.PagesElements
{
    public class PetsPageElements
    {
        private IWebDriver driver;

        #region xpaths

        public static By PetsPageHeaderBy = By.Id("photosHeader");

        #endregion xpaths

        public WebElement PetsPageHeader => driver.FindWebElement(PetsPageHeaderBy);
        public WebElement ApplyBtn => driver.FindWebElement(By.Id("applyBtn"));
        public WebElement ResetFilterBtn => driver.FindWebElement(By.Id("resetBtn"));

        public SelectElement GendersFilterComboBox
        {
            get
            {
                return new SelectElement(driver.FindElement(By.Id("gender")));
            }
        }

        public SelectElement TypesFilterComboBox
        {
            get
            {
                return new SelectElement(driver.FindElement(By.Id("type")));
            }
        }

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
                        Image = cardElement.FindWebElement(By.CssSelector(".card-img-top")),
                        Footer = cardElement.FindWebElement(By.CssSelector(".card-footer")),
                        LikeBtn = cardElement.FindWebElements(By.CssSelector("#likeBtn")).FirstOrDefault()
                    };
                    cards.Add(card);
                }
                return cards;
            }
        }

        public List<WebElement> PageNumbers => driver.FindWebElements(By.XPath("//li[contains(@class, 'pagination-page')]"));

        public PetsPageElements(IWebDriver driver)
        {
            this.driver = driver;
        }
    }
}
