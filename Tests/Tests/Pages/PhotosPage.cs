using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tests.Pages.PagesElements;
using Framework.Extensions;

namespace Tests.Pages
{
    public class PhotosPage : BasePage
    {
        public PhotosPageElements PhotosPageElements { get; }

        public PhotosPage(IWebDriver driver) : base(driver)
        {
            PhotosPageElements = new PhotosPageElements(driver);
        }

        protected override void WaitTillPageIsVisible()
        {
            Wait.Until(CustomExpectedConditions.ElementIsVisible(PhotosPageElements.PhotosPageHeader));
        }
    }
}
