using System;
using System.Collections.Generic;
using System.Text;
using ADJ.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ADJ.DataAccess.EntityConfigurations
{
    class PurchaseOrderItemConfiguration : IEntityTypeConfiguration<PurchaseOrderItem>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrderItem> builder)
        {
            builder.ToTable("PurchaseOrderItems");
        }
    }
}
