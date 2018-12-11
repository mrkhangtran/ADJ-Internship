using System;
using System.Collections.Generic;
using System.Text;
using ADJ.Common;
using ADJ.Repository.Core;
using AutoMapper;

namespace ADJ.BusinessService.Core
{
    public abstract class ServiceBase
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IMapper Mapper;
        protected readonly ApplicationContext AppContext;

        protected ServiceBase(IUnitOfWork unitOfWork, IMapper mapper, ApplicationContext appContext)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
            AppContext = appContext;
        }
    }
}
