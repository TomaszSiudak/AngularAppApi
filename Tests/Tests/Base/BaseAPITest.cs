using Framework.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Base
{
    public class BaseAPITest
    {
        private AuthorizationAPI _authorizationAPI;
        public AuthorizationAPI AuthorizationAPI
        {
            get
            {
                if (_authorizationAPI == null)
                    return _authorizationAPI = new AuthorizationAPI();
                return _authorizationAPI;
            }
        }
    }
}
