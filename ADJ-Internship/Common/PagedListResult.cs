using System;
using System.Collections.Generic;

namespace ADJ.Common
{
	public class PagedListResult<T>
	{
		public List<T> Items { get; set; }

		public int TotalCount { get; set; }

		public int PageCount { get; set; }

		public int PageIndex { get; set; }

		public int TotalPages => (int)Math.Ceiling(decimal.Divide(TotalCount, PageCount));

   }
}
