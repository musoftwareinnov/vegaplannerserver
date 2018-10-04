using vega.Extensions;

namespace vega.Core.Models
{
    public class CustomerQuery : IQueryObject
    {
        public int? Id { get; set; }
        public string SortBy { get; set; }
        public bool IsSortAscending { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public bool isSummary { get; set; }
        public string SearchCriteria { get; set; }
    }
}