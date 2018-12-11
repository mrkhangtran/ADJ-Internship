namespace ADJ.BusinessService.Core
{
    public class ListRequest
    {
        private string _searchTerm;
        public string SearchTerm
        {
            get => _searchTerm ?? string.Empty;
            set => _searchTerm = value;
        }

        public int? PageIndex { get; set; }

        public int? PageSize { get; set; }

        public string SortData { get; set; }
    }
}
