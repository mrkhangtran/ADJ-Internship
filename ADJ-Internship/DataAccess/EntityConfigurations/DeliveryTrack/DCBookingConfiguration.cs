using ADJ.DataModel.DeliveryTrack;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADJ.DataAccess.EntityConfigurations.DeliveryTrack
{
    public class DCBookingConfiguration: IEntityTypeConfiguration<DCBooking>
    {
        public void Configure(EntityTypeBuilder<DCBooking> builder)
        {
            builder.ToTable("DCBookings");
        }
    }
}
