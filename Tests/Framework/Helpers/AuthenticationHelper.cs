using Framework.API;
using Framework.Constants;
using Framework.Extensions;
using Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Helpers
{
    public class AuthenticationHelper
    {
        public static string GetToken(Pet pet) => new AuthorizationAPI().Login(pet == null ? Variables.DefaultPet : pet).GetJObjectFromResponse().Value<string>("token");
    }
}
