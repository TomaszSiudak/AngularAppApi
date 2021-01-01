using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Pages.PagesElements;
using Framework.Extensions;
using OpenQA.Selenium;

namespace Tests.Pages
{
    class EditProfilePage : BasePage
    {
        public EditProfileElements EditProfileElements { get; }

        public EditProfilePage(IWebDriver driver) : base(driver)
        {
            EditProfileElements = new EditProfileElements(driver);
        }

        protected override void WaitTillPageIsVisible()
        {
            Wait.Until(CustomExpectedConditions.ElementIsVisible(EditProfileElements.EditProfileHeader));
        }
    }
}
