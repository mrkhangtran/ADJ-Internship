using ADJ.DataModel.DeliveryTrack;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADJ.DataAccess.EntityConfigurations.DeliveryTrack
{
    public class DCConfirmationConfiguration : IEntityTypeConfiguration<DCConfirmation>
    {
        public void Configure(EntityTypeBuilder<DCConfirmation> builder)
        {
            builder.ToTable("DCConfirmations");
        }
    }
}
