using ADJ.BusinessService.Core;
using ADJ.BusinessService.Dtos;
using ADJ.BusinessService.Interfaces;
using ADJ.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ADJ.BusinessService.Implementations
{
    public class ProgressCheckService : ServiceBase, IProgressCheckService
    {
        public PurchaseOrderService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext, IDataProvider<PurchaseOrder> poDataProvider, IPurchaseOrderRepository poRepository) : base(unitOfWork, mapper, appContext)
        {
            _poDataProvider = poDataProvider;
            _poRepository = poRepository;
        }
        public void checkComplete(int progressId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckOrderHaventProgress(int orderId)
        {
            throw new NotImplementedException();
        }

        public void CreateDefaultModel(int orderId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedListResult<ProgressCheckDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<GetItemSearchDto> SearchItem()
        {
            throw new NotImplementedException();
        }

        public void Update(ProgressCheckDto progressCheckDto)
        {
            throw new NotImplementedException();
        }
    }
}
