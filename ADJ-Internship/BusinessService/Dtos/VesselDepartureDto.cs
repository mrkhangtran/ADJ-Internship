using ADJ.BusinessService.Core;
using ADJ.BusinessService.Validators;
using ADJ.Common;
using ADJ.DataModel.Core;
using ADJ.DataModel.OrderTrack;
using ADJ.DataModel.ShipmentTrack;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ADJ.BusinessService.Dtos
{
	public class ContainerDto : EntityDtoBase, ICreateMapping
	{
		public void CreateMapping(Profile profile)
		{
			profile.CreateMap<Container, ContainerDto>().IncludeBase<EntityBase, EntityDtoBase>();
			profile.CreateMap<ContainerDto, Container>().IncludeBase<EntityDtoBase, EntityBase>();
		}

		public string Name { get; set; }

		public string Size { get; set; }

		public OrderStatus Status { get; set; }


	}

	
}
