using ADJ.DataModel.DeliveryTrack;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADJ.DataAccess.EntityConfigurations.DeliveryTrack
{
    public class DCConfirmationDetailConfiguration : IEntityTypeConfiguration<DCConfirmationDetail>
    {
        public void Configure(EntityTypeBuilder<DCConfirmationDetail> builder)
        {
            builder.ToTable("DCConfirmationDetails");
        }
    }
}
