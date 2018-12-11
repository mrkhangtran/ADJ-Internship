using System;
using System.Collections.Generic;
using System.Text;
using ADJ.DataModel.Core;

namespace ADJ.DataModel
{
    public class PurchaseOrder : EntityBase
    {
        public string Test { get; set; }

        public ICollection<PurchaseOrderItem> Items { get; set; }
    }
}
