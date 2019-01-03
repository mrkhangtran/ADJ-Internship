using ADJ.BusinessService.Core;
using ADJ.Common;
using ADJ.DataModel.Core;
using ADJ.DataModel.ShipmentTrack;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static ADJ.BusinessService.Validators.ContainerDtoValidators;

namespace ADJ.BusinessService.Dtos
{
	public class ShipmentManifestsDtos : EntityDtoBase, ICreateMapping
	{
		[Required]
		public string Name { get; set; }

		public OrderStatus Status { get; set; }

		public string Size { get; set; }

		public string Loading { get; set; }

		public string PackType { get; set; }

		public List<ItemManifest> Manifests { get; set; }

		public bool selectedContainer { get; set; }

		public void CreateMapping(Profile profile)
		{
			profile.CreateMap<Container, ShipmentManifestsDtos>().IncludeBase<EntityBase, EntityDtoBase>();
			profile.CreateMap<ShipmentManifestsDtos, Container>().IncludeBase<EntityDtoBase, EntityBase>();
		}
	}
	public class ItemManifest : EntityDtoBase, ICreateMapping
	{
		public bool selectedItem { get; set; }

		public int BookingId { get; set; }

		public string Supplier { get; set; }

		public string Carrier { get; set; }

		public string PONumber { get; set; }

		public string ItemNumber { get; set; }

		public decimal BookingQuantity { get; set; }

		public decimal OpenQuantity { get; set; }     //= Booking Quantity - Total Ship Quantity (recorded in database)

		[Required]
		[QuantityValidation("OpenQuantity")]
		public decimal ShipQuantity { get; set; }

		public decimal BookingCartons { get; set; } //= Booking Qty* Carton(of 1 item)

		public decimal ShipCartons { get; set; } // Ship Qty*Carton(of 1 item)

		public decimal BookingCube { get; set; } //= Booking Qty * Cube(of 1 item)

		public decimal ShipCube { get; set; } //=Ship Qty*Cube(of 1 item)

		public decimal NetWeight { get; set; } //= Ship Qty*KGS(of 1 item)

		public DateTime ETDDate { get; set; } //value taken from shipment booking

		[QuantityValidation("NetWeight")]
		public decimal GrossWeight { get; set; }

		public string Manifested { get; set; }

		public float Cube { get; set; }

		public float KGS { get; set; }

		public float Carton { get; set; }

		public string FreightTerms { get; set; }

		public void CreateMapping(Profile profile)
		{
			profile.CreateMap<Manifest, ItemManifest>().IncludeBase<EntityBase, EntityDtoBase>();
			profile.CreateMap<ItemManifest, Manifest>().IncludeBase<EntityDtoBase, EntityBase>();
		}
	}
	public class filterItem
	{
		public string DestinationPort { get; set; }
		public string OriginPort { get; set; }
		public string Carrier { get; set; }
		public string Status { get; set; }
		public string Vendor { get; set; }
		public DateTime? ETDFrom { get; set; }
		public DateTime? ETDTo { get; set; }
		public string PONumber { get; set; }
		public string ItemNumber { get; set; }

	}
	public class SearchingManifestItem
	{
		public IEnumerable<string> DestinationPort { get; set; }
		public IEnumerable<string> OriginPorts { get; set; }
		public IEnumerable<string> Carriers { get; set; }
		public IEnumerable<string> Status { get; set; }
	}
	public class ViewManifestDto
	{
		public PagedListResult<ShipmentManifestsDtos> pagedListResult { get; set; }
		public filterItem filterItem { get; set; }
		public int pageIndex { get; set; }
	}
}