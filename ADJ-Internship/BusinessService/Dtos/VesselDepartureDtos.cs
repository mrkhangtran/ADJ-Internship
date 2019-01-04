using ADJ.BusinessService.Core;
using ADJ.Common;
using ADJ.DataModel.Core;
using ADJ.DataModel.ShipmentTrack;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADJ.BusinessService.Dtos
{
	public class VesselDepartureDtos : EntityDtoBase, ICreateMapping
	{
		public FilterDto filterDto { get; set; }

		public List<ContainerDto> lstContainerDto { get; set; }

		public void CreateMapping(Profile profile)
		{
			profile.CreateMap<Container, ContainerDto>().IncludeBase<EntityBase, EntityDtoBase>();
			profile.CreateMap<ContainerDto, Container>().IncludeBase<EntityDtoBase, EntityBase>();
		}
	}

	public class ListContainerReslutDto
	{
		public List<ContainerDto> listContainerResult { get; set; }
	}

	public class FilterDto
	{
		public string origin { get; set; }
		public string originPort { get; set; }
		public string container { get; set; }
		public string status { get; set; }
		public DateTime? etdFrom { get; set; }
		public DateTime? etdTo { get; set; }
	}



	public class ContainerDto : EntityDtoBase, ICreateMapping
	{

		public string Name { get; set; }

		public string Size { get; set; }

		public string Loading { get; set; }

		public string PackType { get; set; }

		public ContainerStatus Status { get; set; }

		public string OriginPort { get; set; }

		public string DestPort { get; set; }

		public string Carrier { get; set; }

		public string Voyage { get; set; }

		public DateTime ETD { get; set; }

		public DateTime ETA { get; set; }

		public string originPortChange { get; set; }

		public string destPortChange { get; set; }

		public string modeChange { get; set; }

		public string carrierChange { get; set; }

		public bool checkClick { get; set; }

		public void CreateMapping(Profile profile)
		{
			profile.CreateMap<Container, ContainerDto>().IncludeBase<EntityBase, EntityDtoBase>();
			profile.CreateMap<ContainerDto, Container>().IncludeBase<EntityDtoBase, EntityBase>();
		}
	}

	public class BookingDto : EntityDtoBase, ICreateMapping
	{

		public DateTime ETD { get; set; }

		public DateTime ETA { get; set; }


		public void CreateMapping(Profile profile)
		{
			profile.CreateMap<Booking, BookingDto>().IncludeBase<EntityBase, EntityDtoBase>();
			profile.CreateMap<BookingDto, Booking>().IncludeBase<EntityDtoBase, EntityBase>();
		}
	}

	public class ManifestDto : EntityDtoBase, ICreateMapping
	{

		public int ContainerId { get; set; }

		public int BookingId { get; set; }

		public void CreateMapping(Profile profile)
		{
			profile.CreateMap<Manifest, ManifestDto>().IncludeBase<EntityBase, EntityDtoBase>();
			profile.CreateMap<ManifestDto, Manifest>().IncludeBase<EntityDtoBase, EntityBase>();
		}
	}

	public class ArriveOfDespatchDto : EntityDtoBase, ICreateMapping
	{
		public int BookingId { get; set; }

		public int ContainerId { get; set; }

		public string Carrier { get; set; }

		public string Vessel { get; set; }

		public string Voyage { get; set; }

		public DateTime ETD { get; set; }

		public DateTime ETA { get; set; }

		public string OriginPort { get; set; }

		public string DestinationPort { get; set; }

		public string Mode { get; set; }

		public string Confirmed { get; set; }

		public void CreateMapping(Profile profile)
		{
			profile.CreateMap<ArriveOfDespatch, ArriveOfDespatchDto>().IncludeBase<EntityBase, EntityDtoBase>();
			profile.CreateMap<ArriveOfDespatchDto, ArriveOfDespatch>().IncludeBase<EntityDtoBase, EntityBase>();
		}
	}

	public class SearchItem
	{
		public IEnumerable<string> Origins { get; set; }
		public IEnumerable<string> OriginPorts { get; set; }
		public IEnumerable<string> Dest { get; set; }
		public IEnumerable<ContainerStatus> Status { get; set; }
		public IEnumerable<String> DestPorts { get; set; }
		public IEnumerable<String> Modes { get; set; }
		public IEnumerable<String> Voyages { get; set; }
		public IEnumerable<String> Carriers { get; set; }
		public IEnumerable<String> Loadings { get; set; }
		public IEnumerable<String> Vendors { get; set; }
	}
}
