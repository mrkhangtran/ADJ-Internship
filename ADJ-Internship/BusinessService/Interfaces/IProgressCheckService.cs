using ADJ.BusinessService.Dtos;
using ADJ.Common;
using ADJ.DataModel.OrderTrack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ADJ.BusinessService.Interfaces
{
    public interface IProgressCheckService
    {
        Task<PagedListResult<ProgressCheckDto>> GetAll();
        void Update(ProgressCheckDto progressCheckDto);
        Task<GetItemSearchDto> SearchItem();
        void CreateDefaultModel(int orderId);
        Task<bool> CheckOrderHaventProgress(int orderId);
        void checkComplete( int progressId);

    }
}
