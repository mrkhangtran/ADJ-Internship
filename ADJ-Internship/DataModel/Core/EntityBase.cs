using System;
using System.ComponentModel.DataAnnotations;

namespace ADJ.DataModel.Core
{
    public abstract class EntityBase : IAuditable
    {
        [Key]
        public int Id { get; set; }

        [Timestamp]
        [ConcurrencyCheck]
        public byte[] RowVersion { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDateUtc { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDateUtc { get; set; }
    }
}
