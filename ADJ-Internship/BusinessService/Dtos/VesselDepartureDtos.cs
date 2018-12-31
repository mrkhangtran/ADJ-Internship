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
	public class VesselDepartureDto : EntityDtoBase, ICreateMapping
	{
		public string Test { get; set; }

		public void CreateMapping(Profile profile)
		{	
		}
	}

	public class BookingDto : EntityDtoBase, ICreateMapping
	{
		public bool selectedItem { get; set; }

		public Guid ShipmentID { get; set; }

		public string PONumber { get; set; }

		public string Line { get; set; }

		public string ItemNumber { get; set; }

		public string Factory { get; set; }

		public string Carrier { get; set; }

		public string Vessel { get; set; }

		public DateTime ETD { get; set; }

		public DateTime ETA { get; set; }

		public string Voyage { get; set; }

		public decimal Quantity { get; set; }

		public float Cartons { get; set; }

		public float Cube { get; set; }

		public string PackType { get; set; }

		public string PortOfLoading { get; set; }

		public string PortOfDelivery { get; set; }

		public string LoadingType { get; set; }

		public string Mode { get; set; }

		public string FreightTerms { get; set; }

		public string Consignee { get; set; }

		public decimal GrossWeight { get; set; }

		public DateTime BookingDate { get; set; }

		public string BookingType { get; set; }

		public OrderStatus Status { get; set; }

		public int OrderId { get; set; }

		public int ContainerId { get; set; }

		public void CreateMapping(Profile profile)
		{
			profile.CreateMap<Booking, BookingDto>().IncludeBase<EntityBase, EntityDtoBase>();
			profile.CreateMap<BookingDto, Booking>().IncludeBase<EntityDtoBase, EntityBase>();
		}
	}

	public class ContainerDto : EntityDtoBase, ICreateMapping
	{

		public string Name { get; set; }

		public string Size { get; set; }

		public string Loading { get; set; }

		public string PackType { get; set; }

		public ContainerStatus Status { get; set; }

		public void CreateMapping(Profile profile)
		{
			profile.CreateMap<Container, ContainerDto>().IncludeBase<EntityBase, EntityDtoBase>();
			profile.CreateMap<ContainerDto, Container>().IncludeBase<EntityDtoBase, EntityBase>();
		}
	}

	public class SearchItem
	{
		public IEnumerable<string> Origins { get; set; }
		public IEnumerable<string> OriginPorts { get; set; }
		public IEnumerable<string> Dest { get; set; }
		public IEnumerable<string> Status { get; set; }
		public IEnumerable<String> DestPorts { get; set; }
		public IEnumerable<String> Modes { get; set; }
		public IEnumerable<String> Voyages { get; set; }
		public IEnumerable<String> Carriers { get; set; }
		public IEnumerable<String> Loadings { get; set; }
		public IEnumerable<String> Vendors { get; set; }
	}
}
