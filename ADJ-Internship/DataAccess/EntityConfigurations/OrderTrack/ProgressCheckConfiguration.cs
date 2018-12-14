using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ADJ.DataModel.OrderTrack;

namespace ADJ.DataAccess.EntityConfigurations.DeliveryTrack
{
    public class ProgressCheckConfiguration : IEntityTypeConfiguration<ProgressCheck>
    {
        public void Configure(EntityTypeBuilder<ProgressCheck> builder)
        {
            builder.ToTable("ProgressChecks");
        }
    }
}
