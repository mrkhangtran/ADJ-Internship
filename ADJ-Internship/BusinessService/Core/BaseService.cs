//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;
//using ADJ.Common;
//using ADJ.DataModel.Core;
//using ADJ.RepositoryBase.Core;
//using AutoMapper;

//namespace ADJ.BusinessService.Core
//{
//    public abstract class BaseService<TEntity> where TEntity : EntityBase
//    {
//        protected readonly IUnitOfWork UnitOfWork;
//        protected readonly IRepository<TEntity> RepositoryBase;
//        protected readonly IMapper Mapper;
//        protected readonly ApplicationContext AppContext;

//        protected BaseService(IUnitOfWork unitOfWork, IRepository<TEntity> repo, IMapper mapper, ApplicationContext appContext)
//        {
//            UnitOfWork = unitOfWork;
//            Mapper = mapper;
//            AppContext = appContext;
//            RepositoryBase = repo;
//        }

//        public virtual async Task<TDto> GetByIdAsync<TDto>(int id)
//        {
//            var entity = await RepositoryBase.GetByIdAsync(id, true);
//            var dto = Mapper.Map<TDto>(entity);

//            return dto;
//        }

//        protected async Task<ListResultDto<TDto>> ListAsync<TDto>(Expression<Func<TEntity, bool>> filter = null, string sortData = null, bool includeDependents = false, int? pageIndex = null, int? pageSize = null)
//        {
//            IEnumerable<TEntity> entities;

//            // filtering
//            var query = filter != null ? RepositoryBase.Query(filter, includeDependents) : RepositoryBase.Query(includeDependents);

//            // order by
//            if (!string.IsNullOrEmpty(sortData))
//            {
//                var orderBy = TransformOrderByClause(sortData);
//                query = query.OrderBy(orderBy);
//            }

//            // paging
//            int totalCount;
//            int pageCount = 1;
//            if (pageIndex != null && pageSize != null)
//            {
//                entities = await query.SelectPagingAsync(pageIndex.Value, pageSize.Value, out totalCount, out pageCount);
//            }
//            else
//            {
//                entities = await query.SelectAsync();
//                totalCount = entities.Count();
//            }

//            // result
//            var dtoList = Mapper.Map<List<TDto>>(entities);
//            var result = new ListResultDto<TDto>
//            {
//                TotalCount = totalCount,
//                PageCount = pageCount,
//                Items = dtoList
//            };

//            return result;
//        }

//        /// <summary>
//        /// Constructs the order by clause based on the provided sort data. Sort terms must match items in
//        /// the OrderByFieldMappings dictionary.
//        /// </summary>
//        /// <param name="sortData">Sort data in the format "DTOField1,asc;DTOField2,desc;DTOField3"
//        /// Default sort order is asc</param>
//        /// <returns>string use in dynamic LinQ in format: "Field1 asc,Field2 desc,Field3 asc"</returns>
//        protected string TransformOrderByClause(string sortData)
//        {
//            try
//            {
//                var orderByClause = new StringBuilder();
//                var hasPrimaryKeyValue = OrderByFieldMappings.ContainsKey("PrimaryKey");
//                var firstOrderByColumnSortOrder = string.Empty;
//                var containPrimaryKeyValue = false;

//                // If no sortby field is supplied, use the default.
//                if (string.IsNullOrEmpty(sortData))
//                {
//                    return OrderByFieldMappings["default"] + " asc";
//                }

//                var sortItems = sortData.Split(';');

//                foreach (var sortItem in sortItems)
//                {
//                    if (!string.IsNullOrEmpty(sortItem))
//                    {
//                        var splitedSortItems = sortItem.Split(',');
//                        var orderByColumnName = splitedSortItems[0];

//                        if (hasPrimaryKeyValue && string.Compare(OrderByFieldMappings[orderByColumnName], OrderByFieldMappings["PrimaryKey"], StringComparison.CurrentCultureIgnoreCase) == 0)
//                        {
//                            containPrimaryKeyValue = true;
//                        }

//                        orderByClause.Append(OrderByFieldMappings[orderByColumnName]);

//                        if (splitedSortItems.Length == 2)
//                        {
//                            var sortOrder = splitedSortItems[1].Equals("desc") ? " desc," : " asc,";
//                            orderByClause.Append(sortOrder);
//                            if (string.IsNullOrEmpty(firstOrderByColumnSortOrder))
//                            {
//                                firstOrderByColumnSortOrder = sortOrder;
//                            }
//                        }
//                        else
//                        {
//                            orderByClause.Append(" asc,");  // Default sort order is asc
//                        }
//                    }
//                }

//                if (hasPrimaryKeyValue && !containPrimaryKeyValue)
//                {
//                    orderByClause.Append(OrderByFieldMappings["PrimaryKey"]);
//                    orderByClause.Append(!string.IsNullOrEmpty(firstOrderByColumnSortOrder)
//                        ? firstOrderByColumnSortOrder
//                        : " asc,");
//                }

//                return orderByClause.ToString().TrimEnd(',');
//            }
//            catch (Exception)
//            {
//                throw new AppException("Invalid sort parameters: {0}", sortData);
//            }
//        }

//        /// <summary>
//        /// Virtual base property to be overridden by the derived service with a list of
//        /// mappings between DTO fields and EF column names.
//        /// 
//        /// Multiple database columns can be specified for the sort separated by semi-colon e.g.
//        /// 
//        ///     "ProductName", "Name;Category.Name"
//        /// 
//        /// This would specify that when the ProductName DTO field is specified as the
//        /// sort column, the results are sorted by Name and then by Category.Name
//        /// </summary>
//        protected virtual Dictionary<string, string> OrderByFieldMappings => new Dictionary<string, string>();
//    }
//}
