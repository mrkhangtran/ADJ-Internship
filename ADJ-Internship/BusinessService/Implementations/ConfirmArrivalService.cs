using ADJ.BusinessService.Core;
using ADJ.BusinessService.Interfaces;
using ADJ.Common;
using ADJ.DataModel.ShipmentTrack;
using ADJ.Repository.Core;
using AutoMapper;
using System;
using System.Collections.Generic;
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

    public async Task<List<Container>> ListContainerFilterAsync()
    {
      return null;
    }
  }
}
