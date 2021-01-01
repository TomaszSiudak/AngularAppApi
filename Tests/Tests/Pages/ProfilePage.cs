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
    public class ProfilePage : BasePage
    {
        public ProfileElements ProfileElements { get; }

        public ProfilePage(IWebDriver driver) : base(driver)
        {
            ProfileElements = new ProfileElements(driver);
        }

        protected override void WaitTillPageIsVisible()
        {
            Wait.Until(CustomExpectedConditions.ElementIsVisible(ProfileElements.ProfileHeader));
        }
    }
}
