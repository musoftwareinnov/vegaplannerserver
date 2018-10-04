namespace vega.Controllers.Resources.StateInitialser
{
    public class SaveStateInitialiserStateResource
    {
        public int StateInitialiserId { get; set; }
        public string Name { get; set; }
        public int CompletionTime { get; set; }
        public int AlertToCompletionTime { get; set; }
        public int OrderId { get; set; }

    }
}