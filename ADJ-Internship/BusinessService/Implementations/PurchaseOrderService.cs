using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ADJ.BusinessService.Core;
using ADJ.BusinessService.Dtos;
using ADJ.BusinessService.Interfaces;
using ADJ.BusinessService.Validators;
using ADJ.Common;
using ADJ.DataModel;
using ADJ.Repository.Core;
using ADJ.Repository.Interfaces;
using AutoMapper;
using FluentValidation;

namespace ADJ.BusinessService.Implementations
{
	public class PurchaseOrderService : ServiceBase, IPurchaseOrderService
	{
		private readonly IDataProvider<PurchaseOrder> _poDataProvider;
		private readonly IPurchaseOrderRepository _poRepository;

		public PurchaseOrderService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext, IDataProvider<PurchaseOrder> poDataProvider, IPurchaseOrderRepository poRepository) : base(unitOfWork, mapper, appContext)
		{
			_poDataProvider = poDataProvider;
			_poRepository = poRepository;
		}

		public async Task<PagedListResult<PurchaseOrderDto>> ListPurchaseOrdersAsync(string searchTerm)
		{
			var poResult = await _poDataProvider.ListAsync();
			var result = new PagedListResult<PurchaseOrderDto>
			{
				TotalCount = poResult.TotalCount,
				PageCount = poResult.PageCount,
				Items = Mapper.Map<List<PurchaseOrderDto>>(poResult.Items)
			};

			return result;
		}

		public async Task<PurchaseOrderDto> CreateOrUpdatePurchaseOrderAsync(CreateOrUpdatePurchaseOrderRq rq)
		{
			var validator = new CreateOrUpdatePurchaseOrderRqValidator();
			await validator.ValidateAndThrowAsync(rq);

			PurchaseOrder entity;
			if (rq.Id > 0)
			{
				entity = await _poRepository.GetByIdAsync(rq.Id, true);
				if (entity == null)
				{
					throw new AppException("Purchase Order Not Found");
				}

				entity = Mapper.Map(rq, entity);
				_poRepository.Update(entity);
			}


			else
			{
				entity = Mapper.Map<PurchaseOrder>(rq);
				_poRepository.Insert(entity);
			}

			await UnitOfWork.SaveChangesAsync();

			var rs = Mapper.Map<PurchaseOrderDto>(entity);
			return rs;
		}

		public async Task DeletePurchaseOrderAsync(int id)
		{
			_poRepository.Delete(id);
			await UnitOfWork.SaveChangesAsync();
		}
	}
}
