namespace ADJ.BusinessService.Core
{
    public abstract class EntityDtoBase
    {
        public int Id { get; set; }

        public string RowVersion { get; set; }
    }
}
