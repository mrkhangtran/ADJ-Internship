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
		public void CreateMapping(Profile profile)
		{
			profile.CreateMap<Container, VesselDepartureDtos>().IncludeBase<EntityBase, EntityDtoBase>();
			profile.CreateMap<VesselDepartureDtos, Container>().IncludeBase<EntityDtoBase, EntityBase>();
		}

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

	}

	public class ArriveOfDespatchDto : EntityDtoBase, ICreateMapping
	{
		public int BookingId { get; set; }

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
