using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBookAPI
{
    public class PetParameters
    {
		private int Max { get; set; } = 12;
		public int CurrentPage { get; set; } = 1;
		private int pageSize;

		public int PageSize
		{
			get { return pageSize; }
			set { pageSize = (value > Max) ? Max : value; }
		}

	}
}
