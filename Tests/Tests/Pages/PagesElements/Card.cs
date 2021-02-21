using Framework.SeleniumWrappers;

namespace Tests.Pages.PagesElements
{
    public class Card
    {
        public WebElement Title { get; set; }
        public WebElement Image { get; set; }
        public WebElement Footer { get; set; }
        public WebElement LikeBtn { get; set; }
    }
}
