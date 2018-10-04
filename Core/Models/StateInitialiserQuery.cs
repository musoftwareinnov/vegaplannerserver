namespace vega.Core.Models
{
    using vega.Extensions;
    public class StateInitialiserQuery : IQueryObject
    {
        public int? Id { get; set; }
        public string SortBy { get; set; }
        public bool IsSortAscending { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }  

        public bool includeDeleted { get; set; }   
    }
}