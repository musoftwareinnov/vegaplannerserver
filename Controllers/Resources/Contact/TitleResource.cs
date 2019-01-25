using System.ComponentModel.DataAnnotations;

namespace vegaplannerserver.Controllers.Resources.Contact
{
    public class TitleResource
    {
        public int Id { get; set; }

        [StringLength(5)]
        public string Name { get; set;}
    }
}