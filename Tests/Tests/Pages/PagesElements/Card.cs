using Framework.SeleniumWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
