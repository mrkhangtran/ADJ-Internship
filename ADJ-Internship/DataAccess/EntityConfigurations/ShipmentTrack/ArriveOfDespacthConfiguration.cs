using System;
using System.Collections.Generic;
using System.Text;
using ADJ.DataModel;
using ADJ.DataModel.ShipmentTrack;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ADJ.DataAccess.EntityConfigurations.ShipmentTrack
{
    public class ArriveOfDespacthConfiguration: IEntityTypeConfiguration<ArriveOfDespacth>
    {
        public void Configure(EntityTypeBuilder<ArriveOfDespacth> builder)
        {
            builder.ToTable("ArriveOfDepacths");
        }
    }
}
