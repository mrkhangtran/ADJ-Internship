using ADJ.DataModel.DeliveryTrack;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADJ.DataAccess.EntityConfigurations.DeliveryTrack
{
    class DCBookingDetailConfiguration: IEntityTypeConfiguration<DCBookingDetail>
    {
        public void Configure(EntityTypeBuilder<DCBookingDetail> builder)
        {
            builder.ToTable("DCBookingDetails");
        }
    }
}
