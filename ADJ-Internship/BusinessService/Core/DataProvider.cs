using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ADJ.Common;
using ADJ.DataModel.Core;
using ADJ.Repository.Core;
using Microsoft.EntityFrameworkCore;

namespace ADJ.BusinessService.Core
{
	public class DataProvider<T> : IDataProvider<T> where T : EntityBase
	{
		private readonly IRepository<T> _repo;

		public DataProvider(IRepository<T> repo)
		{
			_repo = repo;
		}

		public async Task<T> GetByIdAsync(int id)
		{
			var entity = await _repo.GetByIdAsync(id, true);
			return entity;
		}

		public async Task<PagedListResult<T>> ListAsync(Expression<Func<T, bool>> filter = null, string sortData = null, bool includeDependents = false, int? pageIndex = null, int? pageSize = null)
		{
			List<T> entities;

			// filtering
			var query = filter != null ? _repo.Query(filter, includeDependents) : _repo.Query(includeDependents);

			// order by
			if (sortData != null)
			{
					//var orderBy = TransformOrderByClause(sortData);
					query = query.OrderBy(sortData);				
			}

			// paging
			int totalCount;
			int pageCount = 1;
			if (pageIndex != null && pageSize != null)
			{
				entities = await query.SelectPagingAsync(pageIndex.Value, pageSize.Value, out totalCount, out pageCount);
			}
			else
			{
				entities = await query.SelectAsync();
				totalCount = entities.Count();
			}

			// result
			var result = new PagedListResult<T>
			{
				TotalCount = totalCount,
				PageCount = pageCount,
				Items = entities
			};

			return result;
		}

		/// <summary>
		/// Constructs the order by clause based on the provided sort data. Sort terms must match items in
		/// the OrderByFieldMappings dictionary.
		/// </summary>
		/// <param name="sortData">Sort data in the format "DTOField1,asc;DTOField2,desc;DTOField3"
		/// Default sort order is asc</param>
		/// <returns>string use in dynamic LinQ in format: "Field1 asc,Field2 desc,Field3 asc"</returns>
		private string TransformOrderByClause(SortData sortData)
		{
			try
			{
				var orderByClause = new StringBuilder();
				var hasPrimaryKeyValue = sortData.OrderByFieldMappings.ContainsKey("PrimaryKey");
				var firstOrderByColumnSortOrder = string.Empty;
				var containPrimaryKeyValue = false;

				// If no sortby field is supplied, use the default.
				if (string.IsNullOrEmpty(sortData.Expression))
				{
					return sortData.OrderByFieldMappings["default"] + " asc";
				}

				var sortItems = sortData.Expression.Split(';');

				foreach (var sortItem in sortItems)
				{
					if (!string.IsNullOrEmpty(sortItem))
					{
						var splitedSortItems = sortItem.Split(',');
						var orderByColumnName = splitedSortItems[0];

						if (hasPrimaryKeyValue && string.Compare(sortData.OrderByFieldMappings[orderByColumnName], sortData.OrderByFieldMappings["PrimaryKey"], StringComparison.CurrentCultureIgnoreCase) == 0)
						{
							containPrimaryKeyValue = true;
						}

						orderByClause.Append(sortData.OrderByFieldMappings[orderByColumnName]);

						if (splitedSortItems.Length == 2)
						{
							var sortOrder = splitedSortItems[1].Equals("desc") ? " desc," : " asc,";
							orderByClause.Append(sortOrder);
							if (string.IsNullOrEmpty(firstOrderByColumnSortOrder))
							{
								firstOrderByColumnSortOrder = sortOrder;
							}
						}
						else
						{
							orderByClause.Append(" asc,");  // Default sort order is asc
						}
					}
				}

				if (hasPrimaryKeyValue && !containPrimaryKeyValue)
				{
					orderByClause.Append(sortData.OrderByFieldMappings["PrimaryKey"]);
					orderByClause.Append(!string.IsNullOrEmpty(firstOrderByColumnSortOrder)
							? firstOrderByColumnSortOrder
							: " asc,");
				}

				return orderByClause.ToString().TrimEnd(',');
			}
			catch (Exception)
			{
				throw new AppException("Invalid sort parameters: {0}", sortData.Expression);
			}
		}
	}
}
