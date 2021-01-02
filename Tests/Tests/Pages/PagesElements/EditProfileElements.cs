using System.Collections.Generic;
using Framework.Extensions;
using Framework.SeleniumWrappers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Tests.Pages.PagesElements
{
    public class EditProfileElements
    {
        private IWebDriver driver;

        //profile section
        public WebElement EditProfileHeader => driver.FindWebElement(By.CssSelector("#editProfileHeader"));
        public WebElement EditProfileBtn => driver.FindWebElement(By.CssSelector("#editProfileBtn"));
        public WebElement NameField => driver.FindWebElement(By.Id("editProfileName"));
        public WebElement AgeField => driver.FindWebElement(By.Id("editProfileAge"));
        public SelectElement GenderComboBox { get { return  new SelectElement(driver.FindElement(By.Id("editProfileGender"))); } }
        public WebElement CityField => driver.FindWebElement(By.Id("editProfileCity"));
        public WebElement DescriptionTextArea => driver.FindWebElement(By.Id("#editProfileDescription"));
        public WebElement CancelBtn => driver.FindWebElement(By.CssSelector("#backBtn"));
        public WebElement SaveBtn => driver.FindWebElement(By.CssSelector("#saveBtn"));


        //photos section
        public WebElement PhotosTab => driver.FindWebElement(By.XPath("//a[@aria-controls='photosTab']"));
        public List<EditPhotosCard> Photos
        {
            get
            {
                List<EditPhotosCard> photos = new List<EditPhotosCard>();
                foreach (var cardElement in driver.FindWebElements(By.XPath("//tab[@id='photosTab']//div[@class='row']/div")))
                {
                    var photo = new EditPhotosCard()
                    {
                        Image = cardElement.FindWebElement(By.CssSelector(".image-thumbnail")),
                        SetMainPhotoBtn = cardElement.FindWebElement(By.CssSelector("#setPhotoBtn")),
                        DeletePhotoBtn = cardElement.FindWebElement(By.CssSelector("#deletePhotoBtn"))
                    };
                    photos.Add(photo);
                }
                return photos;
            }
        }
        public WebElement SelectFileInput => driver.FindWebElement(By.CssSelector("#selectPhoto"));
        public WebElement UploadPhotoBtn => driver.FindWebElement(By.CssSelector("#uploadPhoto"));
        public WebElement CancelUploadBtn => driver.FindWebElement(By.CssSelector("#cancelUploadPhoto"));

        public EditProfileElements(IWebDriver driver)
        {
            this.driver = driver;
        }
    }
}
