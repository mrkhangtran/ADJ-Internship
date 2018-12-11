using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ADJ.BusinessService.Core;
using ADJ.Common.Helpers;
using ADJ.DataModel.Core;
using Autofac;
using AutoMapper;

namespace ADJ.Infrastructure
{
    public class AutoMapperConfig
    {
        public static void Register(ContainerBuilder builder)
        {
            // Init mapping profile
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });

            // Register the mapper with DI container
            var mapper = mapperConfiguration.CreateMapper();
            builder.RegisterInstance(mapper).As<IMapper>();
        }
    }

    class MappingProfile : Profile
    {
        public MappingProfile()
        {
            var types = typeof(ICreateMapping).GetTypeInfo().Assembly.GetTypes();

            LoadEntityMappings(types);
            //LoadStandardMappings(types);
        }

        private void LoadEntityMappings(IEnumerable<Type> types)
        {
            // Define mapping for base class
            CreateMap<EntityDtoBase, EntityBase>().ForMember(x => x.RowVersion, c => c.MapFrom(dto => ByteArrayConverter.FromString(dto.RowVersion)));
            CreateMap<EntityBase, EntityDtoBase>().ForMember(x => x.RowVersion, c => c.MapFrom(entity => ByteArrayConverter.ToString(entity.RowVersion)));

            var maps = (from t in types
                        where typeof(ICreateMapping).IsAssignableFrom(t)
                              && !t.GetTypeInfo().IsAbstract
                              && !t.GetTypeInfo().IsInterface
                        select (ICreateMapping)Activator.CreateInstance(t)).ToArray();

            foreach (var map in maps)
            {
                map.CreateMapping(this);
            }
        }

        //private static void LoadStandardMappings(IEnumerable<Type> types)
        //{
        //    var maps = (from t in types
        //                from i in t.GetInterfaces()
        //                where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>) &&
        //                      !t.IsAbstract &&
        //                      !t.IsInterface
        //                select new
        //                {
        //                    Source = i.GetGenericArguments()[0],
        //                    Destination = t,
        //                }).ToArray();

        //    foreach (var map in maps)
        //    {
        //        Mapper.CreateMap(map.Source, map.Destination);
        //    }
        //}
    }
}
