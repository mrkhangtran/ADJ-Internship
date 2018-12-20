using System;
using System.Collections.Generic;

namespace ADJ.Common
{
	public class PagedListResult<T>
	{
		public List<T> Items { get; set; }

		public int TotalCount { get; set; }

		public int PageCount { get; set; }

		public string CurrentFilter { get; set; }
	}
}
