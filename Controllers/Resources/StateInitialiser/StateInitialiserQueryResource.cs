namespace vega.Controllers.Resources.StateInitialser
{
    public class StateInitialiserQueryResource
    {
        public int? Id { get; set; }
        public string SortBy { get; set; }
        public bool IsSortAscending { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }  
        public bool includeDeleted { get; set; }         
    }
}