using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tests.Pages.PagesElements;
using Framework.Extensions;
using Framework.Base.WebDriverData;

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
        }

        public PetsPage GoToPhotosPage()
        {
            Driver.Url = URL;
            WaitTillPageIsVisible();
            return this;
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

    }
}
