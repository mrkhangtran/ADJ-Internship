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
        Task<int> CheckOrderHaventProgress(int orderId);
        void CreateDefaultModel(int orderId);
        void checkComplete(ProgressCheck progressCheck);
        Task<PagedListResult<ProgressCheckDto>> ListProgressCheckDtoAsync();
        void Update(ProgressCheckDto progressCheckDto);

    }
}
