using ADJ.BusinessService.Core;
using ADJ.BusinessService.Interfaces;
using ADJ.Common;
using ADJ.DataModel.ShipmentTrack;
using ADJ.Repository.Core;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ADJ.BusinessService.Implementations
{
  public class ConfirmArrivalService : ServiceBase, IConfirmArrivalService
  {
    private readonly IDataProvider<Container> _containerProvider;

    public ConfirmArrivalService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext,
      IDataProvider<Container> containerDataProvider) : base(unitOfWork, mapper, appContext)
    {
      _containerProvider = containerDataProvider;
    }

    public async Task<List<Container>> ListContainerFilterAsync(int? page, DateTime? ETAFrom, DateTime? ETATo, string origin = null, string mode = null, 
      string vendor = null, string container = null, string status = null)
    {
      if (page == null) { page = 1; }

      Expression<Func<Container, bool>> All = x => x.Id > 0;

      if (origin != null)
      {
        //Expression<Func<Container, bool>> filter = x => x. == origin;
        //All = All.And(filter);
      }

      return null;
    }
  }
}
