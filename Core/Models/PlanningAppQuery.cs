namespace vega.Core.Models
{
    using vega.Extensions;
    public class PlanningAppQuery : IQueryObject
    {
        public int? Id { get; set; }
        public string PlanningAppType { get; set; }   //1 = InProgress | 2 = Archived | 3 = Terminated
        public string PlanningAppStatusType { get; set; }
        public int CustomerId  { get; set; }
        public string SortBy { get; set; }
        public bool IsSortAscending { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string StateStatus { get; set; }
    }
}