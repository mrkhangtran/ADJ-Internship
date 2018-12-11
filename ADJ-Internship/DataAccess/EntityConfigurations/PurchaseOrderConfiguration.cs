using System;
using System.Collections.Generic;
using System.Text;
using ADJ.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ADJ.DataAccess.EntityConfigurations
{
    class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
        {
            builder.ToTable("PurchaseOrders");
        }
    }
}
