using ADJ.BusinessService.Core;
using ADJ.BusinessService.Dtos;
using ADJ.BusinessService.Interfaces;
using ADJ.Common;
using ADJ.DataModel.OrderTrack;
using ADJ.Repository.Core;
using ADJ.Repository.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ADJ.BusinessService.Implementations
{
    public class ProgressCheckService : ServiceBase, IProgressCheckService
    {
        private readonly IDataProvider<Order> _odDataProvider;
        private readonly IProgressCheckRepository _prcRepository;
        private readonly IDataProvider<OrderDetail> _oddDataProvider;
        public ProgressCheckService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext, IDataProvider<Order> odDataProvider,IDataProvider<OrderDetail> oddDataProvider, IProgressCheckRepository prcRepository) : base(unitOfWork, mapper, appContext)
        {
            _odDataProvider = odDataProvider;
            _prcRepository = prcRepository;
            _oddDataProvider = oddDataProvider;
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
