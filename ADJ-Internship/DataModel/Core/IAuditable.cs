using System;

namespace ADJ.DataModel.Core
{
    public interface IAuditable
    {
        string CreatedBy { get; set; }

        DateTime CreatedDateUtc { get; set; }

        string ModifiedBy { get; set; }

        DateTime? ModifiedDateUtc { get; set; }
    }
}
