using System;
using System.Collections.Generic;
using System.Text;
using ADJ.DataModel.Core;

namespace ADJ.DataModel
{
    public class PurchaseOrderItem : EntityBase
    {
        public string Test { get; set; }

        public PurchaseOrder PurchaseOrder { get; set; }

        public int PurchaseOrderId { get; set; }
    }
}
