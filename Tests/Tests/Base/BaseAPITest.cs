using Framework.API;
using Framework.Helpers.SqlHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Base
{
    public class BaseAPITest
    {
        private SqlHelper _sqlHelper;
        protected SqlHelper SqlHelper
        {
            get
            {
                if (_sqlHelper == null)
                    return _sqlHelper = new SqlHelper();
                return _sqlHelper;
            }
        }

        private AuthorizationAPI _authorizationAPI;
        protected AuthorizationAPI AuthorizationAPI
        {
            get
            {
                if (_authorizationAPI == null)
                    return _authorizationAPI = new AuthorizationAPI();
                return _authorizationAPI;
            }
        }

        private PetsAPI _petsAPI;
        protected PetsAPI PetsAPI
        {
            get
            {
                if (_petsAPI == null)
                    return _petsAPI = new PetsAPI();
                return _petsAPI;
            }
        }
    }
}
