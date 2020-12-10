﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Helpers.SqlHelper
{
    public class SqlHelper
    {
        private PetsDB _pets;
        public PetsDB Pets
        {
            get
            {
                if (_pets == null)
                    return _pets = new PetsDB();
                return _pets;
            }
        }
    }
}
